using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Presenter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {


        //Update
        Observable.EveryUpdate().Subscribe(_ => {
            player.Move(UI.moveJoystick.Vertical, UI.moveJoystick.Horizontal);
            player.Rotate(UI.aimJoystick.Vertical, UI.aimJoystick.Horizontal);
        }).AddTo(gameObject);

    }

    public UI UI;

    public KnightPlayer player;
}
