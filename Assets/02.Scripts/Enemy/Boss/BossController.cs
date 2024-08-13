using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    protected BossStat bossStat;

    protected float attackTimer = 0f;

    //==================================================================================

    private void Start()
    {
        bossStat = GetComponent<BossStat>();
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= bossStat.GetAttackSpeed())
        {
            Attack();
            attackTimer = 0f;
        }

    }

    //==================================================================================

    public virtual void Attack() { }

    public virtual void SKill() { }
}
