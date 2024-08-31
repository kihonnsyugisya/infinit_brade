using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class DashAciton : MonoBehaviour
{
    [SerializeField] private MMF_Player dashAttackfeedback;

    public void DashAtack()
    {
        dashAttackfeedback.PlayFeedbacks();
    }
}
