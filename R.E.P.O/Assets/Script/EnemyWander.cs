using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // NavMeshを使うために必要な機能

public class EnemyAI : MonoBehaviour
{
    // 敵の状態の種類を登録する箱
    public enum EnemyState { Wander, Alert, Chase }

    [Header("現在の状態（デバッグ用）")]
    // インスペクター上で、今敵が何の状態なのかをリアルタイムで見るための変数
    public EnemyState currentState = EnemyState.Wander;

    [Header("プレイヤーの設定")]
    public Transform playerTarget; // 追いかけるプレイヤーのTransform

    [Header("徘徊の設定 (Wander)")]
    public float wanderRadius = 10.0f;　// 目的地を探す範囲
    public float wanderWaitTime = 2.0f; // 目的地についた後の待ち時間
    public float wanderSpeed = 1.5f; // 徘徊するときの歩くスピード

    [Header("警戒の設定 (Alert)")]
    public float detectRadius = 6.0f; // プレイヤーを見つける半径
    public float viewAngle = 90.0f; // 敵の視界（扇形）
    public float alertLimitTime = 1.5f; // 敵に見つかるまでの猶予時間

    [Header("追跡の設定（Chase)")]
    public float chaseSpeed = 2.0f; // 追いかける時の走る速度
    public float loseRadius = 5.0f; // あきらめる距離

    private NavMeshAgent agent; // 敵の移動をコントロールするコンポーネントを入れる変数
    private Animator anim; // アニメーションを切り替えるコンポーネントを入れる変数
    private float wanderTimer; // 目的地に着いたあと、待ち時間を数えるためのタイマー
    private float alertTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // ゲーム開始直後、敵が棒立ちにならないように最初のランダムな目的地を決める
        SetRandomDestination();
    }

    void Update()
    {
        // アニメーターがちゃんと存在しているか確認
        if (anim != null)
        {
            // 現在の敵の「実際の移動速度（秒速）」を測って、Animatorの"Speed"に送る
            anim.SetFloat("Speed", agent.velocity.magnitude);
        }

        // 自身とプレイヤーとの間の移動速度を伝える処理
        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

        // 今の敵の状態によって、行動を変える
        switch (currentState)
        {
            case EnemyState.Wander:
                HandleWander(distanceToPlayer); // 徘徊状態
                break;
            case EnemyState.Alert:
                HandleAlert(distanceToPlayer);
                break;
            case EnemyState.Chase:
                HandleChase(distanceToPlayer);
                break;
        }
    }

    // --- 徘徊状態のときの具体的な行動 ---
    void HandleWander(float distanceToPlayer)
    {
        if (IsPlayerInFOV(distanceToPlayer))
        {
            currentState = EnemyState.Alert; // 状態を「警戒」に切り替える
            alertLimitTime = 0.0f;
            agent.ResetPath(); // 今目指していた目的地を消去して、その場でピタッと足を止める
            return;
        }

        // --- 通常の徘徊移動の処理 ---
        agent.speed = wanderSpeed;
        // 敵が目的地に無事に到着したかを3つの条件でチェック
        // !agent.pathPending : ルートを計算中ではない
        // agent.hasPath : ちゃんと目指すルートを持っている
        // agent.remainingDistance <= agent.stoppingDistance : 目的地までの残り距離が、停止する距離以下になった
        if (!agent.pathPending && agent.hasPath && agent.remainingDistance <= agent.stoppingDistance)
        {
            // 時間の経過をタイマーにどんどん足していく
            wanderTimer += Time.deltaTime;

            // もしタイマーが、設定した待ち時間を超えたら
            if (wanderTimer >= wanderWaitTime)
            {
                // 次の新しいランダムな目的地を決めて、そこへ向かわせる
                SetRandomDestination();

                // 次回のカウントのためにタイマーをゼロにリセットする
                wanderTimer = 0;
            }
        }
    }

    // --- 警戒状態のときの具体的な行動 ---
    void HandleAlert(float distanceToPlayer)
    {
        // プレイヤーの方をスムーズにじっと見つめ続ける
        LookAtTarget(playerTarget.position);

        // 警戒中も、プレイヤーが視界の中にいるのかチェック
        if (IsPlayerInFOV(distanceToPlayer))
        {
            alertTimer += Time.deltaTime; // 視界内にいたらタイマーが進む

            if (alertTimer >= alertLimitTime)
            {
                currentState = EnemyState.Chase; // 時間切れで追跡モード
            }
        }
        else
        {
            // 視界から外れたらタイマーが減る
            alertTimer -= Time.deltaTime;

            if (alertTimer <= 0)
            {
                currentState = EnemyState.Wander; // 状態を「徘徊」に戻す
                SetRandomDestination(); // またトコトコ歩き回るために、新しい目的地を決める
            }
        }
    }

    // --- 
    void HandleChase(float distanceToPlayer)
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(playerTarget.position); // プレイヤーを追う

        // 諦める円より遠くに離れたら見失う
        if(distanceToPlayer > loseRadius)
        {
            currentState = EnemyState.Wander;
            SetRandomDestination();
        }
    }

    // --- ランダムな目的地を設定する関数 ---
    void SetRandomDestination()
    {
        // 自分の今の立ち位置を中心とした、設定した半径の球体の中のランダムな1点を決める
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        // そのままだと中心基準になってしまうので、自分の今の位置を足して「自分の周り」にする
        randomDirection += transform.position;

        // NavMesh上の正しい位置のデータを一時的に保存するための箱
        NavMeshHit hit;

        // さっき決めたランダムな点が、ちゃんとNavMesh上にあるかチェックする
        // もしエリア外だった場合、そこから一番近い「歩けるポイント」を自動で探して hit に入れる
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    // --- ターゲットの方をスムーズに向くための関数 ---
    void LookAtTarget(Vector3 targetPosition)
    {
        // ターゲットがいる方向のベクトルを計算する
        Vector3 direction = (targetPosition - transform.position).normalized;

        // 敵が上下にナナメに傾いてしまわないように、Y軸（縦方向）の回転はカットする
        direction.y = 0;

        // 向きの計算がゼロ（真上など）でなければ回転を適用する
        if (direction != Vector3.zero)
        {
            // 「そっちの方向を向くための回転データ」を作る
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Quaternion.Slerp(今の向き, 目標の向き, スピード) で、ガクッと向くのではなく
            // ぬるっとスムーズにプレイヤーの方へ向き直らせる
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    // --- プレイヤーが敵の視界（扇形）に入っているかを判定する ---
    bool IsPlayerInFOV(float distanceToPlayer)
    {
        // 距離チェック
        if (distanceToPlayer > detectRadius) return false;

        // 角度チェック
        Vector3 directionToPlayer = (playerTarget.position - transform.position).normalized;

        // 敵の正面とプレイヤーへの方向の「間の角度」を計算する
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        // 計算した角度が「視界の角度の半分」の中におさまっているかのチェック
        if(angleToPlayer <= viewAngle * 0.5f)
        {
            return true;
        }

        return false;
    }

    // --- 「索敵円」を描く機能 ---
    void OnDrawGizmosSelected()
    {
        // 円の色を「赤」にする
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, loseRadius);

        // 敵の視界の扇形を描く
        Gizmos.color = Color.blue;

        // 扇形の中心線を引く
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * detectRadius);

        // 扇形の左端と右端の境界線を計算して線を引く
        Quaternion leftRotation = Quaternion.AngleAxis(-viewAngle * 0.5f, Vector3.up);
        Quaternion RightRotation = Quaternion.AngleAxis(viewAngle * 0.5f, Vector3.up);

        Vector3 leftDirection = leftRotation * transform.forward;
        Vector3 rightDirection = RightRotation * transform.forward;

        Gizmos.DrawLine(transform.position, transform.position + leftDirection * detectRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightDirection * detectRadius);
    }
}