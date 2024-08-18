using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Enemy : MonoBehaviour
{
    [SerializeField] private MMF_Player damagedMmfPlayer;

    // レイヤー名を定数として定義
    private const string PlayerWeaponLayerName = "PlayerWeapon";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        // レイヤー名からレイヤー番号を取得
        int playerWeaponLayer = LayerMask.NameToLayer(PlayerWeaponLayerName);

        // 他のオブジェクトが "PlayerWeaponLayerName" に属しているかを確認
        if (other.gameObject.layer == playerWeaponLayer)
        {
            damagedMmfPlayer.PlayFeedbacks();
            // 当たったときの処理をここに記述
        }

        Debug.Log(gameObject.name + "に　");
        Debug.Log(other.name + "に当たった！");

    }
}
