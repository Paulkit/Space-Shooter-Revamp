using UnityEngine;
using System.Collections;

public class EnemyHpController : MonoBehaviour {

    public float enemyFullHp;

    private float enemyCurHp = 0f;
    private bool _isDead = false;

    void Start()
    {
        enemyCurHp = enemyFullHp;
    }

    public void minusEnemyHp(float playerDamage)
    {
        enemyCurHp -= playerDamage;
        if (enemyCurHp <= 0)
            _isDead = true;
    }

    public bool isDead()
    {
        return _isDead;
    }
}
