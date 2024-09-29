using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Mummy : Enemy
{
    [SerializeField] private MMF_Player mMF_Player;

    protected override void ResetValues()
    {
        base.ResetValues();
        mMF_Player.gameObject.SetActive(false);
    }


    /// <summary>
    /// ゾンビの攻撃アニメーションを管理します。
    /// すでに設定されているアニメーションパラメータと新しい値を比較し、
    /// 異なる場合のみ変更を行います。
    /// </summary>
    /// <param name="isAttack">攻撃状態かどうかを示すブール値</param>
    protected override async void ZombieAttack(bool isAttack)
    {
        
        if (isAttack)
        {
            Debug.Log(isAttack);
            mMF_Player.gameObject.SetActive(true);
            await mMF_Player.PlayFeedbacksTask();
            DieMode();
            DelayedReleaseZombie();
            //mMF_Player.gameObject.SetActive(false);
        }

    }
}
