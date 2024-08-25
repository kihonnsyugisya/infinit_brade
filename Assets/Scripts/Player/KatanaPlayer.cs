using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class KatanaPlayer : MonoBehaviour
{
    [SerializeField] private MMF_Player lToRPlayer; // 左から右斬り攻撃用のMMF_Player
    [SerializeField] private MMF_Player rToLPlayer; // 右から左斬り攻撃用のMMF_Player
    [SerializeField] private MMF_Player upperPlayer; // 上斬り攻撃用のMMF_Player
    [SerializeField] private MMF_Player lowerPlayer; // 下斬り攻撃用のMMF_Player
    [SerializeField] private MMF_Player dodgeLPlayer; // 左回避のMMF_Player
    [SerializeField] private MMF_Player dodgeRPlayer; // 右回避のMMF_Player
    [SerializeField] private MMF_Player kickPlayer;   // 蹴り攻撃用のMMF_Player

    [SerializeField] private Animator animator;       // プレイヤーのアニメーター
    [SerializeField] private Transform EnemyPos;      // 敵キャラクターの位置

    private Dictionary<KatanaAttacks, MMF_Player> attackFeedbacks; // 攻撃タイプとMMF_Playerの対応辞書

    public float ikWeight; // IKの重み（0～1の間で調整）

    /// <summary>
    /// 初期化処理を行います。攻撃タイプと対応するMMF_Playerのマッピングを設定します。
    /// </summary>
    private void Awake()
    {
        // 攻撃タイプとそれに対応するMMF_Playerのマッピングを初期化
        attackFeedbacks = new Dictionary<KatanaAttacks, MMF_Player>
        {
            { KatanaAttacks.L_TO_R, lToRPlayer },
            { KatanaAttacks.R_TO_L, rToLPlayer },
            { KatanaAttacks.UPPER, upperPlayer },
            { KatanaAttacks.LOWER, lowerPlayer },
            { KatanaAttacks.KICK, kickPlayer },
            { KatanaAttacks.DODGE_L, dodgeLPlayer },
            { KatanaAttacks.DODGE_R, dodgeRPlayer },
         };
    }

    /// <summary>
    /// フレームごとに呼び出される更新処理。プレイヤーの入力に応じて攻撃を実行します。
    /// </summary>
    private void Update()
    {
        // 旧InputManagerで攻撃を実行する場合のサンプルコード
        if (Input.GetKeyDown(KeyCode.L))
        {
            Attack(KatanaAttacks.R_TO_L); // 左斬り攻撃
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Attack(KatanaAttacks.L_TO_R); // 右斬り攻撃
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Attack(KatanaAttacks.UPPER); // 右斬り攻撃
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Attack(KatanaAttacks.LOWER); // 右斬り攻撃
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Attack(KatanaAttacks.KICK); // 蹴り攻撃
        }
    }

    /// <summary>
    /// アニメーションのIK（Inverse Kinematics）処理が行われるタイミングで呼び出されます。
    /// 敵キャラクターの方向を向くようにアニメーションを調整します。
    /// </summary>
    /// <param name="layerIndex">処理対象のレイヤーのインデックス</param>
    private void OnAnimatorIK(int layerIndex)
    {
        // どの部位がどのくらいターゲットを見るかを決める
        animator.SetLookAtWeight(1.0f, 0f, 1.0f, 0.0f, 0f);
        // どこを見るか（今回は敵キャラクターの位置）
        animator.SetLookAtPosition(EnemyPos.position);
    }

    /// <summary>
    /// 指定された攻撃タイプに応じて、対応するMMF_Playerを再生します。
    /// </summary>
    /// <param name="attack">実行する攻撃タイプ</param>
    public void Attack(KatanaAttacks attack)
    {
        if (attackFeedbacks.TryGetValue(attack, out MMF_Player player))
        {
            player.PlayFeedbacks(); // 攻撃フィードバックを再生
        }
        else
        {
            Debug.LogWarning($"指定された攻撃タイプ {attack} は認識されませんでした。"); // 攻撃タイプが認識されなかった場合の警告
        }
    }
}
