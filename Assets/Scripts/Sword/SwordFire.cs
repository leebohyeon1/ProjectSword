using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordFire : MonoBehaviour
{
    public PlayerStat playerStat;
    public MagicSword magicSword;
    // Start is called before the first frame update
    void Start()
    {
        Set();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Fire()
    {
        GameObject bullet = playerStat.GetBullet();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;
        bullet.SetActive(true);

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        bulletRb.velocity = Vector2.up * (playerStat.bulletSpeed + playerStat.upBulletSpeed[playerStat.weaponIndex]);

        EventManager.Instance.PostNotification(EVENT_TYPE.FIRE, this);
    }

    public virtual void Set()
    {
        playerStat = FindFirstObjectByType<PlayerStat>();
    }
}
