using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

namespace InfinityBladePrototype
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private MMF_Player damagedMmfPlayer;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform playerPos;

        // レイヤー名を定数として定義
        private const string PlayerWeaponLayerName = "PlayerWeapon";

        private int playerWeaponLayer;

        private void Awake()
        {
            // レイヤー名からレイヤー番号を取得
            playerWeaponLayer = LayerMask.NameToLayer(PlayerWeaponLayerName);
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnAnimatorIK(int layerIndex)
        {
            // どの部位がどのくらいターゲットを見るかを決める
            animator.SetLookAtWeight(1.0f, 0f, 1.0f, 0.0f, 0f);
            // どこを見るか（今回は敵キャラクターの位置）
            animator.SetLookAtPosition(playerPos.position);
        }

        void OnTriggerEnter(Collider other)
        {
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
}

