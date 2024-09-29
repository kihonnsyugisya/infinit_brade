using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UniRx;

public class GameManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        // killCountが10の倍数になるたびにdispKOTextSubjectに通知
        zombiePool.killCount
            .Where(killCount => killCount % 10 == 0)
            .Subscribe(_ => dispKOTextSubject.OnNext(Unit.Default))
            .AddTo(this);

        zombiePool.killCount
            .Where(killCount => killCount == 80)
            .Subscribe(_ => zombiePool.CreateMummy())
            .AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    [SerializeField] private PlayableDirector gameOverDirector;

    public void PlayGameOverMovie()
    {
        gameOverDirector.Play();
    }

    public ZombiePool zombiePool;

    // KOメッセージの表示命令を通知するためのチャンネル
    [SerializeField] private Subject<Unit> dispKOTextSubject = new Subject<Unit>();

}
