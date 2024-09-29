using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UniRx;

public class ZombiePool : MonoBehaviour
{
    [SerializeField] private int initialCapacity = 10; // 初期キャパシティ
    [SerializeField] private int maxCapacity = 20; // 最大キャパシティ
    [HideInInspector] public IntReactiveProperty killCount;

    // スポーンする位置のTransformを外部から設定可能にする
    [SerializeField] private Transform[] spawnPoints;

    private ObjectPool<GameObject> zombieObjectPool;

    /// <summary>
    /// シーンに配置されているゾンビをDIする
    /// ゾンビのフィールドがDIされた状態のコピーを作りたいから。
    /// PrefabからInstanteateしたらフィールドが埋まってない状態になるから。
    /// </summary>
    [SerializeField] private GameObject enemy;
    [SerializeField] private List<GameObject> mummys = new();

    // アクティブなゾンビの数をリアクティブに管理
    private IntReactiveProperty activeZombieCount = new(0);

    private void Awake()
    {
        // オブジェクトプールを作成
        zombieObjectPool = new ObjectPool<GameObject>
        (
            createFunc: CreateZombie,
            actionOnGet: OnZombieGet,
            actionOnRelease: OnZombieRelease,
            actionOnDestroy: OnZombieDestroy,
            collectionCheck: true,
            defaultCapacity: initialCapacity,
            maxSize: maxCapacity
        );

        // アクティブゾンビ数が特定の閾値を下回った場合に新しいゾンビをスポーン
        activeZombieCount
            .Where(count => count < maxCapacity)
            .Subscribe(_ => SpawnZombies(maxCapacity - activeZombieCount.Value))
            .AddTo(this);
    }

    private GameObject CreateZombie()
    {
        return Instantiate(enemy);
    }

    public void CreateMummy()
    {
        foreach (var mummy in mummys)
        {
            mummy.SetActive(true);
        }
    }

    private void OnZombieGet(GameObject zombie)
    {
        zombie.transform.position = GetRandomSpawnPoint();
        activeZombieCount.Value++;
        // ゾンビをプールから取得したときの処理
        zombie.SetActive(true);
    }

    private void OnZombieRelease(GameObject zombie)
    {
        // ゾンビをプールに返却したときの処理
        zombie.SetActive(false);
        killCount.Value++;
    }

    private void OnZombieDestroy(GameObject zombie)
    {
        // ゾンビが破棄されるときの処理
        Destroy(zombie);
    }

    private void SpawnZombies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            zombieObjectPool.Get();
        }
    }

    public void ReleaseZombie(GameObject zombie)
    {
        // ゾンビを返却
        activeZombieCount.Value--;
        zombieObjectPool.Release(zombie);
    }

    // スポーンポイントをランダムに取得する
    private Vector3 GetRandomSpawnPoint()
    {
        // 複数のスポーンポイントからランダムに選択する
        int randomIndex = Random.Range(0, spawnPoints.Length);
        return spawnPoints[randomIndex].position;
    }
}
