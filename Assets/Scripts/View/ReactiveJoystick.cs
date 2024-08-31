using UnityEngine;
using UniRx;
using UnityEngine.EventSystems;

public class ReactiveJoystick : Joystick
{
    // ドラッグ中かどうかを判定するための公開フィールド
    public BoolReactiveProperty IsDragging { get; private set; } = new BoolReactiveProperty(false);

    // タッチが開始されたときの処理
    public override void OnPointerDown(PointerEventData eventData)
    {
        IsDragging.Value = true; // ドラッグ中に設定
        base.OnPointerDown(eventData); // 親クラスの処理を呼び出す
    }

    // タッチが終了したときの処理
    public override void OnPointerUp(PointerEventData eventData)
    {
        IsDragging.Value = false; // ドラッグが終了したので設定解除
        base.OnPointerUp(eventData); // 親クラスの処理を呼び出す
    }
}
