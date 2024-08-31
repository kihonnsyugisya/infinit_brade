using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MoreMountains.Feedbacks;
using UniRx;

public class KnightPlayer : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineTransposer cinemachineTransposer;

    // Start is called before the first frame update
    void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        animator = attackFeedback.GetFeedbackOfType<MMF_Animation>().BoundAnimator;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    [Range(2.0f, 4.0f)]
    [SerializeField] private float moveSpeed;

    [Range(0f, 1f)]
    [SerializeField] private float damping = 0.1f; // ダンピング係数

    /// <summary>
    /// ダッシュしているか判定するための境界値
    /// joystickのverticalの値がこの値以上であればダッシュ中とみなす
    /// </summary>
    [Range(0.5f, 1f)]
    [SerializeField] readonly private float dashRange;

    public BoolReactiveProperty isDash = new();

    private const string HorizontalAnimationName = "X";
    private const string VerticalAnimationName = "Y";

    private float currentVerticalSpeed = 0f;
    private float currentHorizontalSpeed = 0f;

    public void Move(float vertical, float horizontal)
    {
        if (vertical > dashRange) isDash.Value = true;
        else isDash.Value = false;

        // 現在の速度と目標の速度を補間して、滑らかに移動させる
        currentVerticalSpeed = Mathf.Lerp(currentVerticalSpeed, vertical, damping);
        currentHorizontalSpeed = Mathf.Lerp(currentHorizontalSpeed, horizontal, damping);

        // 左スティックでの縦移動と横移動
        transform.position += currentVerticalSpeed * moveSpeed * Time.deltaTime * transform.forward;
        transform.position += currentHorizontalSpeed * moveSpeed * Time.deltaTime * transform.right;

        // アニメーションのパラメータを更新（スムーズに切り替わるように補間）
        animator.SetFloat(HorizontalAnimationName, currentHorizontalSpeed);
        animator.SetFloat(VerticalAnimationName, currentVerticalSpeed);
        //Debug.Log(currentHorizontalSpeed + " :Horizontal");
        //Debug.Log(currentVerticalSpeed + " :Vertical");

    }



    [Range(1f, 2.0f)]
    [SerializeField] private float horizontalRotateSpeed;

    [Range(0f, 1.0f)]
    [SerializeField] private float verticalRotateSpeed;

    [SerializeField] private float verticalRotateMinLange;
    [SerializeField] private float verticalRotateMaxLange;

    private Vector3 currentFollowOffset = new();

    /// <summary>
    /// カメラ回転速度調整用
    /// </summary>
    private const float FillerSpeed = 0.1f;

    public void Rotate(float vertical, float horizontal)
    {
        // 現在の Follow Offset を取得
        currentFollowOffset = cinemachineTransposer.m_FollowOffset;

        // Follow Offset の y 値を変更
        float newFollowOffsetY = currentFollowOffset.y + vertical * verticalRotateSpeed * FillerSpeed;

        // y 値が範囲内に収まるように制限
        newFollowOffsetY = Mathf.Clamp(newFollowOffsetY, verticalRotateMinLange, verticalRotateMaxLange);

        // 制限された y 値を Follow Offset に適用
        currentFollowOffset.y = newFollowOffsetY;
        cinemachineTransposer.m_FollowOffset = currentFollowOffset;

        // 水平方向の回転
        transform.Rotate(new Vector3(0, horizontal * horizontalRotateSpeed * FillerSpeed, 0));
    }


    [SerializeField] private MMF_Player attackFeedback;

    public void Attack()
    {
        //アニメーションステイトか数値をとってダッシュ状態か判定して、ダッシュ中ならDAのfeedback呼ぶ
        attackFeedback.PlayFeedbacks();
    }

    [SerializeField] private DashAciton dashAciton;

    public void DashAction(DashActionType　dashAction)
    {
        switch (dashAction)
        {
            case DashActionType.DASH_ATTACK:
                dashAciton.DashAtack();
                break;
            default:
                break;
        }
    }

    
}
