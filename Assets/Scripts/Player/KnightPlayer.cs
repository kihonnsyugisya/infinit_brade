using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class KnightPlayer : MonoBehaviour
{
    [Range(2.0f, 4.0f)]
    [SerializeField] private float moveSpeed;

    [Range(1f, 2.0f)]
    [SerializeField] private float horizontalRotateSpeed;

    [Range(0f, 1.0f)]
    [SerializeField] private float verticalRotateSpeed;

    [SerializeField] private float verticalRotateMinLange;
    [SerializeField] private float verticalRotateMaxLange;


    [SerializeField] private Animator animator;

    private const string HorizontalAnimationName = "X";
    private const string VerticalAnimationName = "Y";

    [Range(0f, 1f)]
    [SerializeField] private float damping = 0.1f; // ダンピング係数

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineTransposer cinemachineTransposer;

    private float currentVerticalSpeed = 0f;
    private float currentHorizontalSpeed = 0f;

    private Vector3 currentFollowOffset = new();

    // Start is called before the first frame update
    void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(float vertical, float horizontal)
    {
        // 現在の速度と目標の速度を補間して、滑らかに移動させる
        currentVerticalSpeed = Mathf.Lerp(currentVerticalSpeed, vertical, damping);
        currentHorizontalSpeed = Mathf.Lerp(currentHorizontalSpeed, horizontal, damping);

        // 左スティックでの縦移動と横移動
        transform.position += transform.forward * currentVerticalSpeed * moveSpeed * Time.deltaTime;
        transform.position += transform.right * currentHorizontalSpeed * moveSpeed * Time.deltaTime;

        // アニメーションのパラメータを更新（スムーズに切り替わるように補間）
        animator.SetFloat(HorizontalAnimationName, currentHorizontalSpeed);
        animator.SetFloat(VerticalAnimationName, currentVerticalSpeed);
    }

    public void Rotate(float vertical, float horizontal)
    {
        // 現在の Follow Offset を取得
        currentFollowOffset = cinemachineTransposer.m_FollowOffset;

        // Follow Offset の y 値を変更
        float newFollowOffsetY = currentFollowOffset.y + vertical * verticalRotateSpeed;

        // y 値が範囲内に収まるように制限
        newFollowOffsetY = Mathf.Clamp(newFollowOffsetY, verticalRotateMinLange, verticalRotateMaxLange);

        // 制限された y 値を Follow Offset に適用
        currentFollowOffset.y = newFollowOffsetY;
        cinemachineTransposer.m_FollowOffset = currentFollowOffset;

        // 水平方向の回転
        transform.Rotate(new Vector3(0, horizontal * horizontalRotateSpeed, 0));
    }
}
