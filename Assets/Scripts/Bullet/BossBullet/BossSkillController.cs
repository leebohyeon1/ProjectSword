using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillController : MonoBehaviour
{
    private float damage = 0;
    private BulletType bulletType;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void InitializeSkill(float _damage, BulletType type)
    {
        damage = _damage;
        bulletType = type;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStat>().TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
