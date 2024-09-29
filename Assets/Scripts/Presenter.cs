using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Presenter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //プレーヤ攻撃
        UI.attackButton.onClick.AsObservable().Subscribe(_=> {
            player.Attack();
        }).AddTo(this);

        //プレーヤ必殺技
        UI.specialAttackButton.onClick.AsObservable().Subscribe(_ => {
            UI.DispUI(false);
            player.PlayBigBradeMovie();
            UI.DispUI(true);
        }).AddTo(this);

        //ダッシュ攻撃
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

        //プレーヤが死んだら死亡ムービー流す
        player.isDead.Subscribe(value => {
            if (value)
            {
                UI.DispUI(false);
                gameManager.PlayGameOverMovie();
            }
        }).AddTo(this);

        // 10の倍数でKOカウントの表示
        gameManager.zombiePool.killCount
            .Where(killCount => killCount % 10 == 0)
            .Skip(1)  // 最初の通知をスキップ
            .Subscribe(killCount => UI.DisplayKOCount(killCount))
            .AddTo(this);
    }

    [SerializeField] private UI UI;

    [SerializeField] private KnightPlayer player;

    [SerializeField] private GameManager gameManager;
}
