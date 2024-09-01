using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

/// <summary>
/// Animator の攻撃ステートを管理するクラスです。
/// このクラスは Animator の現在のステートが攻撃ステートであるかどうかをチェックし、
/// 一致する攻撃ステートの enum 値を返します。ステートのハッシュ値を使用することで、
/// 文字列比較よりもパフォーマンスを向上させています。
/// </summary>
public class AnimatorAttackStateTracker
{
    private readonly Animator animator;
    private readonly Dictionary<int, PlayerAttackState> attackStateHashToEnumMap;

    /// <summary>
    /// コンストラクタ。Animator を受け取ります。
    /// </summary>
    /// <param name="animator">攻撃ステートをチェックするための Animator インスタンス</param>
    public AnimatorAttackStateTracker(Animator animator)
    {
        this.animator = animator;

        // AttackState enum のすべての値のハッシュを取得してマップに格納
        attackStateHashToEnumMap = System.Enum.GetValues(typeof(PlayerAttackState))
            .Cast<PlayerAttackState>()
            .ToDictionary(
                state => Animator.StringToHash(state.ToString()),
                state => state
            );
    }

    /// <summary>
    /// 現在のアニメーションステートをチェックし、
    /// 攻撃ステートに一致する場合、そのステートの enum 値を返します。
    /// 一致するステートがない場合、文字列比較で探します。それでも一致しない場合は null を返します。
    /// </summary>
    /// <returns>現在のアニメーションステートの enum 値。攻撃ステートが一致しない場合は null</returns>
    public PlayerAttackState? GetCurrentAttackAnimationState()
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        int currentStateShortHash = stateInfo.shortNameHash;
        

        // shortNameHash で一致するステートがあるか確認
        if (attackStateHashToEnumMap.TryGetValue(currentStateShortHash, out var attackState))
        {
            //Debug.Log("一致する攻撃アニメーションステートが shortNameHash で見つかりました: " + attackState);
            return attackState;
        }

        //なぜか一致しないから力技
        if (stateInfo.IsName(PlayerAttackState.Attack_4Combo_1_Inplace.ToString()))
        {
            return PlayerAttackState.Attack_4Combo_1_Inplace;
        }

        //なぜか一致しないから力技
        if (stateInfo.IsName(PlayerAttackState.Attack_4Combo_4_Inplace.ToString()))
        {
            return PlayerAttackState.Attack_4Combo_4_Inplace;
        }

        return null;
    }
}
