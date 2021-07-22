using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyAttack
{
    Melee,
    Range,
    Mage
}

public enum EnemyType
{
    Firework=1,
    GarageTool=2,
    KitchenAppliance=3,
    Headphone=4,
    VehicleMusic=5,
    Vehicle=6,
    Party=7,
    Sports=8,
    School=9,
    PowerTools=10
}

public class Enemy
{
    public bool alive = true;
    public bool active = false;
    public EnemyAttack attackType;
    public EnemyType enemyType;
    public int hp;
    public int attackDamage;
    public List<Enemy> spawnedEnemies = null;

    public Enemy(EnemyAttack ea, EnemyType et)
    {
        this.attackType = ea;
        this.enemyType = et;

        //Put in stats here through dictionary

    }
}
