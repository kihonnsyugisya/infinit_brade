using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using System;

public class UI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Listの初期化
        controllerUI = new List<GameObject>
        {
            aimJoystick.gameObject,  // JoystickをGameObjectとして追加
            moveJoystick.gameObject, // 同上
            attackButton.gameObject,  // ButtonをGameObjectとして追加
            dashActionButton.gameObject,
            specialAttackButton.gameObject,
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ムービー中に非表示にしたいUIのリスト
    /// </summary>
    private List<GameObject> controllerUI;

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
    /// 必殺技ようボタン
    /// </summary>
    public Button specialAttackButton;

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

    /// <summary>
    /// UIの表示非表示を切り替える
    /// </summary>
    /// <param name="isDisp"></param>
    public void DispUI(bool isDisp)
    {
        foreach (var ui in controllerUI)
        {
            ui.SetActive(isDisp);
        }
        Debug.Log("call :" + isDisp);
    }

    [SerializeField] private TextMeshProUGUI koText; // KOテキストのUI
    [SerializeField] private float displayDuration = 2f; // KOテキストの表示時間（秒）

    /// <summary>
    /// KO数を表示し、指定された秒数後に非表示にするメソッド
    /// </summary>
    /// <param name="koCount">表示するKO数</param>
    public void DisplayKOCount(int koCount)
    {
        // KOテキストの表示
        string message = $"{koCount} K.O";
        koText.gameObject.SetActive(true);
        koText.text = message;

        // 指定された秒数後に非表示にする
        Observable.Timer(TimeSpan.FromSeconds(displayDuration))
            .Subscribe(_ =>
            {
                koText.gameObject.SetActive(false);
            })
            .AddTo(this); // ObservableをGameObjectのライフサイクルにバインド
    }

}
