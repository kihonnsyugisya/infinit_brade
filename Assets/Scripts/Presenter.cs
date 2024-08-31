using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Presenter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UI.attackButton.onClick.AsObservable().Subscribe(_=> {
            player.Attack();
        }).AddTo(this);

        UI.dashActionButton.onClick.AsObservable().Subscribe(_=> {
            //TODO: ゆくゆくはユーザの設定を保存したprefesの情報モデルから引数を読み込む
            player.DashAction(DashActionType.DASH_ATTACK);
        }).AddTo(this);

        //Update
        Observable.EveryUpdate().Subscribe(_ => {
            player.Move(UI.moveJoystick.Vertical, UI.moveJoystick.Horizontal);
            player.Rotate(UI.aimJoystick.Vertical, UI.aimJoystick.Horizontal);
        }).AddTo(gameObject);

        //プレーヤがダッシュ中かどうかでアクションボタンの切り替え
        player.isDash.Subscribe(value => {
            UI.ToggleAttackButton(value);
        }).AddTo(this);
    }

    public UI UI;

    public KnightPlayer player;
}
