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
    public int hp = 100;
    public int attackDamage = 10;
    public int cooldown = 20;
    public int speed = 1;
    public List<Enemy> spawnedEnemies = null;
    public Vector3 position;
    public static List<int[]> positionsUsed = new List<int[]>();
    public static System.Random random = new System.Random();

    public Enemy(EnemyAttack ea, EnemyType et, Room r)
    {
        //Testing purposes
        /*
        this.attackType = EnemyAttack.Melee;
        this.enemyType = EnemyType.Firework;*/
        //Regular
        this.attackType = ea;
        this.enemyType = et;
        //Random position of enemy in room. System to ensure that 2 enemies cannot spawn in the same spot.
        int[] pos = null;
        bool exists = true;
        while(exists)
        {
            int xTrans = r.roomRect[0] + random.Next(4, r.roomRect[2] - 3);
            int yTrans = r.roomRect[1] + random.Next(4, r.roomRect[3] - 3);
            pos = new int[] { xTrans, yTrans };
            exists = false;
            foreach(int[] compare in positionsUsed)
            {
                if(compare[0] == pos[0] && compare[1] == pos[1])
                {
                    exists = true;
                }
            }
            foreach (int[] compare in r.unwalkablePositions)
            {
                if (compare[0] == pos[0] && compare[1] == pos[1])
                {
                    exists = true;
                }
            }
            this.position = new Vector3( xTrans, yTrans, -10);
            if (!exists)
            {
                positionsUsed.Add(pos);
            }
        }

        //Put in stats here through dictionary

    }

    public static void resetPositionsUsed()
    {
        positionsUsed.Clear();
    }

    public void takeDamage(int damage)
    {
        this.hp -= damage;
        this.alive = this.hp > 0;
    }
}
