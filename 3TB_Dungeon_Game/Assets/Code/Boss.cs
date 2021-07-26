using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss
{
    public bool alive = true;
    public bool active = false;
    public EnemyAttack attackType;
    public EnemyType enemyType;
    public int hp;
    public int attackDamage;
    public List<Enemy> spawnedEnemies = null;

    public Boss(EnemyAttack ea, EnemyType et)
    {
        this.attackType = ea;
        this.enemyType = et;

        //Put in stats here through dictionary

    }
}
