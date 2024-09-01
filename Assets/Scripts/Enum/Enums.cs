/// <summary>
/// 言語を表すenumです。
/// </summary>
public enum Language
{
    ja = 0, // 日本語
    en = 1, // 英語
    es = 2, // スペイン語
    fr = 3, // フランス語
    de = 4, // ドイツ語
    pt = 5, // ポルトガル語
    ru = 6, // ロシア語
}

/// <summary>
/// プレーヤーの攻撃の種類
/// </summary>
public enum Attacks
{
    SLASH_L,
    SLASH_R,
    KICK,
    TUKI,
    LOWER,
    UPPER
}

/// <summary>
/// 刀プレーヤの攻撃の種類
/// </summary>
public enum KatanaAttacks
{
    L_TO_R,
    R_TO_L,
    UPPER,
    LOWER,
    KICK,
    DODGE_L,
    DODGE_R
}

public enum Dodges
{
    LEFT,
    RIGHT
}

public enum DashActionType
{
    DASH_ATTACK,
}

public enum ZombieMotion
{
    Attack,
    AttackL,
    AttackR,
    Run
}

/// <summary>
/// プレーヤのアニメータのステートの名前
/// 今のステートの状態を取得するときにこれと一致してるか比較して使うので
/// アニメーションのステイトに変更を加える場合ここにも変更を加えること
/// </summary>
public enum PlayerAttackState
{
    Dash_Attack_ver_B,
    Attack_4Combo_1_Inplace,
    Attack_4Combo_2_Inplace,
    Attack_4Combo_3_Inplace,
    Attack_4Combo_4_Inplace
}