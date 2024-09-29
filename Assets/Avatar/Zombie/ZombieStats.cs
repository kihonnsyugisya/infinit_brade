using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZombieStats", menuName = "ScriptableObjects/ZombieStats", order = 1)]
public class ZombieStats : ScriptableObject
{
    public float health = 100f;        // ゾンビのHP
    public float attackPower = 10f;    // ゾンビの攻撃力
    public float moveSpeed = 2.5f;  // ゾンビの移動速度
    public float attackRange = 1.5f; // 攻撃範囲の半径

    /// <summary>
    /// ダメージのenumをstringにしたリスト
    /// </summary>
    public List<string> damagedStates = new List<string>();

    //MonoBehabiourじゃないからStartが使えないため
    private void OnEnable()
    {
        // Enum のすべての値を文字列に変換してリストに格納
        damagedStates = new List<string>(Array.ConvertAll((ZombieDameged[])Enum.GetValues(typeof(ZombieDameged)), state => state.ToString()));
    }
}
