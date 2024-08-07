using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordFire : MonoBehaviour
{
    [SerializeField] protected PlayerStat playerStat;
    [SerializeField] protected MagicSword magicSword;

    //================================================================================================

    void Start()
    {
        Set();
    }

    void Update()
    {
        
    }

    //================================================================================================

    public virtual void Fire()
    {
        GameObject bullet = playerStat.GetBullet();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;
        bullet.SetActive(true);

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        bulletRb.velocity = Vector2.up * (playerStat.bulletSpeed + playerStat.upBulletSpeed[playerStat.GetWeaponIndex()]);

        EventManager.Instance.PostNotification(EVENT_TYPE.FIRE, this);
    }

    public virtual void Set()
    {
        playerStat = FindFirstObjectByType<PlayerStat>();
    }

    public virtual void SetMagicSword(MagicSword sword)
    {
        magicSword = sword;
    }
}
