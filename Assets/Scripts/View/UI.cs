using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// カメラの視点移動用
    /// </summary>
    public Joystick aimJoystick;

    /// <summary>
    /// 移動用
    /// </summary>
    public Joystick moveJoystick;

    /// <summary>
    /// 攻撃ようボタン
    /// </summary>
    public Button attackButton;

    /// <summary>
    /// ダッシュ中に表示されるアクションボタン
    /// 処理内容はユーザがカスタムできるようにしたい。
    /// 例えばスキルアップで、モンハンの回転回避に変更できたりとか（デフォでDA）
    /// </summary>
    public Button dashActionButton;

    /// <summary>
    /// ダッシュ中はダッシュアクションボタンを表示、
    /// そうでなければアタックボタンを表示
    /// </summary>
    /// <param name="isDash">ダッシュ中かどうか</param>
    public void ToggleAttackButton(bool isDash)
    {
        attackButton.gameObject.SetActive(!isDash);
        dashActionButton.gameObject.SetActive(isDash);
    }

}
