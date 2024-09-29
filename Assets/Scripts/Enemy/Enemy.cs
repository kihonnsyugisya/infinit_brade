using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected ZombiePool zombiePool;
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform playerPos;
    [SerializeField] private Collider zombieCollider;
    //[SerializeField] private Rigidbody rigidbody;
    
    private float HP;

    /// <summary>
    /// 攻撃範囲に入っているか
    /// </summary>
    private BoolReactiveProperty isAttackRange = new();

    [SerializeField] private ZombieStats zombieStats;

    private bool isDie = false;

    private void OnEnable()
    {
        //パラメータをリセット
        ResetValues();

        //プレーヤが攻撃範囲に入っているか監視する
        isAttackRange.Subscribe(value => {
            if (isDie) return;
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
    }

    private void OnDisable()
    {
        isAttackRange.Value = false;
        isDie = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDie) return;

        // 衝突したオブジェクトのレイヤーが PlayerAttack レイヤーと一致するかチェック
        if (collision.gameObject.tag == "PlayerAttack")
        {
            ZombieDamaged(); // ダメージを受ける処理
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDie) return;

        // プレイヤーとの距離を計算
        float distanceToPlayer = Vector3.Distance(transform.position, playerPos.position);
        isAttackRange.Value = distanceToPlayer <= zombieStats.attackRange;

        // プレイヤーを追跡
        if (!isAttackRange.Value && navMeshAgent.pathStatus != NavMeshPathStatus.PathInvalid)
        {
            navMeshAgent.SetDestination(playerPos.position);
        }

    }

    /// <summary>
    /// プレイヤーが攻撃範囲内にいる場合の処理を行います。
    /// </summary>
    private void StartAttackMode()
    {
        navMeshAgent.isStopped = true; // ナビメッシュエージェントを停止
        ZombieRun(false); // ゾンビを走らせない
        ZombieAttack(true); // 攻撃アニメーションを開始
    }

    /// <summary>
    /// プレイヤーが攻撃範囲外に出た場合の処理を行います。
    /// </summary>
    private void ChasePlayerMode()
    {
        ZombieRun(true); // ゾンビを走らせる
        ZombieAttack(false); // 攻撃アニメーションを停止
        navMeshAgent.isStopped = false; // ナビメッシュエージェントを再開
    }

    /// <summary>
    /// ノックダウンになった場合の処理
    /// </summary>
    protected void DieMode()
    {
        ZombieRun(false); // ゾンビを走らせる
        ZombieAttack(false); // 攻撃アニメーションを停止
        //navMeshAgent.isStopped = true; // ナビメッシュエージェントを再開
        isDie = true; // ノックダウンのフラグを立てる
        zombieCollider.enabled = false;
        navMeshAgent.ResetPath();
        navMeshAgent.enabled = false;

    }

    /// <summary>
    /// パラメータを最初の状態にリセットする
    /// リスポンした時に使う
    /// </summary>
    protected virtual void ResetValues()
    {
        zombieCollider.enabled = true;
        navMeshAgent.enabled = true;
        HP = zombieStats.health;
        //rigidbody.velocity = Vector3.zero;
        //rigidbody.angularVelocity = Vector3.zero;
    }

    /// <summary>
    /// ゾンビの走るアニメーションを管理します。
    /// すでに設定されているアニメーションパラメータと新しい値を比較し、
    /// 異なる場合のみ変更を行います。
    /// </summary>
    /// <param name="isRun">走る状態かどうかを示すブール値</param>
    private void ZombieRun(bool isRun)
    {
        if (animator.GetBool("Run") != isRun)
        {
            animator.SetBool("Run", isRun);
        }
    }

    /// <summary>
    /// ゾンビの攻撃アニメーションを管理します。
    /// すでに設定されているアニメーションパラメータと新しい値を比較し、
    /// 異なる場合のみ変更を行います。
    /// </summary>
    /// <param name="isAttack">攻撃状態かどうかを示すブール値</param>
    protected virtual void ZombieAttack(bool isAttack)
    {
        if (animator.GetBool(ZombieMotion.Attack.ToString()) != isAttack)
        {
            animator.SetBool(ZombieMotion.Attack.ToString(), isAttack);
        }
    }

    /// <summary>
    /// ランダムにダメージのアニメーションを実行する。
    /// 全部同じリアクションだときもいから
    /// </summary>
    private void ZombieDamaged()
    {
        // ランダムに選んだダメージステートの文字列を使用してアニメーターのブール値を設定
        var randomDamagedState = zombieStats.damagedStates[UnityEngine.Random.Range(0, zombieStats.damagedStates.Count)];
        animator.SetBool(randomDamagedState, true);

        if (!isDie)
        {
            DieMode();
            //一定時間を開けてからゾンビをプールに返却
            DelayedReleaseZombie();
        }
    }

    /// <summary>
    /// ゾンビが非アクティブになった後、指定した範囲内のランダムな時間が経過した後にプールに返却します。
    /// </summary>
    protected void DelayedReleaseZombie()
    {
        // 1秒から3秒の範囲でランダムな時間を設定
        float randomDelay = UnityEngine.Random.Range(2f, 3.5f);

        Observable.Timer(TimeSpan.FromSeconds(randomDelay))
            .Subscribe(_ =>
            {
                zombiePool.ReleaseZombie(gameObject);
            })
            .AddTo(this); // Observableを破棄する際に自動的に解除される
    }


}
