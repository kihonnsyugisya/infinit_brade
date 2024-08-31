using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

namespace InfinityBladePrototype
{
    /// <summary>
    /// プレイヤーキャラクターの攻撃アクションを管理するクラス。
    /// 各攻撃アクションに対応するMMF_Playerを設定し、攻撃入力に応じて適切なフィードバックを再生します。
    /// また、IK（Inverse Kinematics）を使用して敵キャラクターの方向を向く処理も行います。
    /// </summary>
    public class Player : MonoBehaviour
    {
        [SerializeField] private MMF_Player slashLPlayer; // 左斬り攻撃用のMMF_Player
        [SerializeField] private MMF_Player slashRPlayer; // 右斬り攻撃用のMMF_Player
        [SerializeField] private MMF_Player upperPlayer; // 右斬り攻撃用のMMF_Player
        [SerializeField] private MMF_Player lowerPlayer; // 右斬り攻撃用のMMF_Player
        [SerializeField] private MMF_Player kickPlayer;   // 蹴り攻撃用のMMF_Player
        [SerializeField] private MMF_Player tukiPlayer;   // 蹴り攻撃用のMMF_Player

        [SerializeField] private MMF_Player dodgeLPlayer;   // 回避LのMMF_Player
        [SerializeField] private MMF_Player dodgeRPlayer;   // 回避RのMMF_Player



        [SerializeField] private Animator animator;       // プレイヤーのアニメーター
        [SerializeField] private Transform EnemyPos;      // 敵キャラクターの位置

        private Dictionary<Attacks, MMF_Player> attackFeedbacks; // 攻撃タイプとMMF_Playerの対応辞書

        private readonly Dictionary<Attacks, string> clipNameMapping = new()
        {
            { Attacks.SLASH_L, "Slash2.com" },
            { Attacks.SLASH_R, "Slash.com" },
        };

        public float ikWeight; // IKの重み（0～1の間で調整）

        /// <summary>
        /// 初期化処理を行います。攻撃タイプと対応するMMF_Playerのマッピングを設定します。
        /// </summary>
        private void Awake()
        {
            // 攻撃タイプとそれに対応するMMF_Playerのマッピングを初期化
            attackFeedbacks = new Dictionary<Attacks, MMF_Player>
        {
            { Attacks.SLASH_L, slashLPlayer }, // 左斬り
            { Attacks.SLASH_R, slashRPlayer }, // 右斬り
            { Attacks.UPPER, upperPlayer }, // 右斬り
            { Attacks.LOWER, lowerPlayer }, // 右斬り
            { Attacks.KICK, kickPlayer },      // 蹴り
            { Attacks.TUKI, tukiPlayer }       // 蹴り
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
                Attack(Attacks.SLASH_L); // 左斬り攻撃
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                Attack(Attacks.SLASH_R); // 右斬り攻撃
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                ikWeight = 0f;
                Attack(Attacks.UPPER); // 左斬り攻撃
                ikWeight = 1f;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                ikWeight = 0f;
                Attack(Attacks.LOWER); // 右斬り攻撃
                ikWeight = 1f;
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                Attack(Attacks.KICK); // 蹴り攻撃
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                ikWeight = 0f;
                Attack(Attacks.TUKI); // 蹴り攻撃
                ikWeight = 1f;
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
            animator.SetLookAtWeight(ikWeight, 0f, ikWeight, 0.0f, 0f);
            // どこを見るか（今回は敵キャラクターの位置）
            animator.SetLookAtPosition(EnemyPos.position);
        }

        /// <summary>
        /// 指定された攻撃タイプに応じて、対応するMMF_Playerを再生します。
        /// </summary>
        /// <param name="attack">実行する攻撃タイプ</param>
        public void Attack(Attacks attack)
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

        [SerializeField] private List<ParticleSystem> attackEffects;
        public void PlayAttackParticle()
        {
            foreach (var effect in attackEffects)
            {
                effect.Play();
            }
        }

        public void Dodge(Dodges dodgeType)
        {
            switch (dodgeType)
            {
                case Dodges.LEFT:
                    dodgeLPlayer.PlayFeedbacks();
                    break;
                case Dodges.RIGHT:
                    dodgeRPlayer.PlayFeedbacks();
                    break;
                default:
                    Debug.LogWarning($"指定された回避タイプ {dodgeType} は認識されませんでした。"); // 攻撃タイプが認識されなかった場合の警告
                    break;
            }
        }
    }
}

