using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform playerPos;

    [SerializeField] private float attackRange = 2.0f; // 攻撃範囲の半径
    private BoolReactiveProperty isAttackRange = new();
    private BoolReactiveProperty hasPath = new();

    private void OnEnable()
    {
        isAttackRange.Subscribe(value => {
            if (value)
            {
                // プレイヤーが攻撃範囲内にいる場合、攻撃アニメーションを開始
                StartAttackMode();
            }
            else {
                // プレイヤーが追跡範囲外の場合、
                ChasePlayerMode();
            }
        }).AddTo(this);

        hasPath.Subscribe(value =>
        {
            if (!value && !isAttackRange.Value)
            {
                navMeshAgent.ResetPath();
                navMeshAgent.SetDestination(playerPos.position);
            }
        }).AddTo(this);
    }

    private void OnDisable()
    {
        isAttackRange.Value = false;
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーとの距離を計算
        float distanceToPlayer = Vector3.Distance(transform.position, playerPos.position);
        if (distanceToPlayer <= attackRange)
        {
            isAttackRange.Value = true;
        }
        else {
            isAttackRange.Value = false;
        }

        // 経路が見つからない場合
        if (!navMeshAgent.hasPath)
        {
            hasPath.Value = false;
        }
        else {
            hasPath.Value = true;
        }
    }

    /// <summary>
    /// プレイヤーが攻撃範囲内にいる場合の処理を行います。
    /// </summary>
    private void StartAttackMode()
    {
        // プレイヤーが攻撃範囲内にいる場合、攻撃アニメーションを開始
        navMeshAgent.ResetPath(); // 現在の経路をリセット
        navMeshAgent.isStopped = true; // ナビメッシュエージェントを停止
        ZombieRun(false); // ゾンビを走らせない
        ZombieAttack(true); // 攻撃アニメーションを開始
    }

    /// <summary>
    /// プレイヤーが攻撃範囲外に出た場合の処理を行います。
    /// </summary>
    private void ChasePlayerMode()
    {
        // プレイヤーが追跡範囲外にいる場合、ゾンビの動作を追跡状態に戻す
        ZombieRun(true); // ゾンビを走らせる
        ZombieAttack(false); // 攻撃アニメーションを停止
        navMeshAgent.isStopped = false; // ナビメッシュエージェントを再開
        navMeshAgent.SetDestination(playerPos.position); // プレイヤーの位置に向かって移動
    }

    /// <summary>
    /// 走るアニメーション
    /// </summary>
    /// <param name="isRun"></param>
    private void ZombieRun(bool isRun)
    {
        animator.SetBool("Run", isRun);
    }

    /// <summary>
    /// 殴るアニメーション
    /// </summary>
    /// <param name="isAttack"></param>
    private void ZombieAttack(bool isAttack)
    {
        animator.SetBool(ZombieMotion.Attack.ToString(), isAttack);
    }
}
