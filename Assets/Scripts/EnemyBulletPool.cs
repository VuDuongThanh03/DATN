using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyBulletPool : MonoBehaviour
{
    public EnemyBulletController bulletPrefab;
    public int defaultCapacity = 10;

    public static EnemyBulletPool Instance;

    private ObjectPool<EnemyBulletController> pool;

    void Awake()
    {
        Instance = this;
        pool = new ObjectPool<EnemyBulletController>(
            CreateFunc,
            OnGet,
            OnRelease,
            OnDestroyBullet,
            false,
            defaultCapacity,
            30
        );
    }

    EnemyBulletController CreateFunc()
    {
        var bullet = Instantiate(bulletPrefab);
        bullet.SetPool(pool);
        return bullet;
    }

    void OnGet(EnemyBulletController bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    void OnRelease(EnemyBulletController bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    void OnDestroyBullet(EnemyBulletController bullet)
    {
        Destroy(bullet.gameObject);
    }

    public EnemyBulletController GetBullet()
    {
        return pool.Get();
    }

    public void ReturnToPool(EnemyBulletController bullet)
    {
        pool.Release(bullet);
    }
}
