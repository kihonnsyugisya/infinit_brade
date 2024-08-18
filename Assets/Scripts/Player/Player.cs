using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Player : MonoBehaviour
{
    [SerializeField] private MMF_Player slashPlayer;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform EnemyPos;

    private const string slashR_parameter_name = "SlashR";
    private const string slashL_parameter_name = "SlashL";
    private const string kick_parameter_name = "Kick";

    public float ikWeight; // IKの重み（0～1の間で調整）

    public void Slash(string parameterName)
    {
        animator.SetTrigger(parameterName);
        slashPlayer?.PlayFeedbacks();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 旧InputManagerでやりたい人用
        if (Input.GetKeyDown(KeyCode.L))
        {
            Slash(slashL_parameter_name);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Slash(slashR_parameter_name);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Slash(kick_parameter_name);
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        //どの部位がどのくらい見るかを決める
        animator.SetLookAtWeight(1.0f, 0f, 1.0f, 0.0f, 0f);
        //どこを見るか（今回はカメラの位置）
        animator.SetLookAtPosition(EnemyPos.position);
    }
}
