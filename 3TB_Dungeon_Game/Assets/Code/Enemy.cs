using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyAttack
{
    Melee,
    Range,
    Mage,
    Mini
}

public enum EnemyType
{
    Firework = 1,
    GarageTool = 2,
    KitchenAppliance = 3,
    Headphone = 4,
    VehicleMusic = 5,
    Vehicle = 6,
    Party = 7,
    Sports = 8,
    School = 9,
    PowerTools = 10
}

public class Enemy
{
    public bool destroy = false;
    public bool alive = true;
    public EnemyAttack attackType;
    public EnemyType enemyType;
    public float hp;
    public float attackDamage;
    public float cooldown;
    public float speed;
    public float projectileSpeed;
    public List<Enemy> spawnedEnemies = null;
    public Vector3 position;
    public static List<int[]> positionsUsed = new List<int[]>();
    public static System.Random random = new System.Random();
    public static Dictionary<string, Dictionary<string, float>> enemyStats = new Dictionary<string, Dictionary<string, float>>() { { "Melee_1", new Dictionary<string, float>() { { "hp", 15.0f }, { "atk", 5.0f }, { "spd", 2.0f }, { "prspd", 0f }, { "cooldown", 125.0f } } }, { "Range_1", new Dictionary<string, float>() { { "hp", 45.0f }, { "atk", 10.0f }, { "spd", 1.0f }, { "prspd", 1.0f }, { "cooldown", 250.0f } } }, { "Mage_1", new Dictionary<string, float>() { { "hp", 35.0f }, { "atk", 0f }, { "spd", 1.0f }, { "prspd", 0f }, { "cooldown", 350.0f } } }, { "Mini_1", new Dictionary<string, float>() { { "hp", 5.0f }, { "atk", 15.0f }, { "spd", 1.75f }, { "prspd", 0f }, { "cooldown", 150.0f } } }, { "Melee_2", new Dictionary<string, float>() { { "hp", 30.0f }, { "atk", 15.0f }, { "spd", 1.0f }, { "prspd", 0f }, { "cooldown", 175.0f } } }, { "Range_2", new Dictionary<string, float>() { { "hp", 30.0f }, { "atk", 5.0f }, { "spd", 1.5f }, { "prspd", 1.0f }, { "cooldown", 175.0f } } }, { "Mage_2", new Dictionary<string, float>() { { "hp", 35.0f }, { "atk", 0f }, { "spd", 1.0f }, { "prspd", 0f }, { "cooldown", 350.0f } } }, { "Mini_2", new Dictionary<string, float>() { { "hp", 10.0f }, { "atk", 15.0f }, { "spd", 2.0f }, { "prspd", 0f }, { "cooldown", 175.0f } } }, { "Melee_3", new Dictionary<string, float>() { { "hp", 15.0f }, { "atk", 10.0f }, { "spd", 1.5f }, { "prspd", 0f }, { "cooldown", 125.0f } } }, { "Range_3", new Dictionary<string, float>() { { "hp", 50.0f }, { "atk", 10.0f }, { "spd", 0.75f }, { "prspd", 25.0f }, { "cooldown", 200.0f } } }, { "Mage_3", new Dictionary<string, float>() { { "hp", 40.0f }, { "atk", 25.0f }, { "spd", 1.0f }, { "prspd", 150.0f }, { "cooldown", 300.0f } } }, { "Melee_4", new Dictionary<string, float>() { { "hp", 15.0f }, { "atk", 5.0f }, { "spd", 2.0f }, { "prspd", 0f }, { "cooldown", 200.0f } } }, { "Range_4", new Dictionary<string, float>() { { "hp", 30.0f }, { "atk", 15.0f }, { "spd", 1.0f }, { "prspd", 150.0f }, { "cooldown", 200.0f } } }, { "Mage_4", new Dictionary<string, float>() { { "hp", 40.0f }, { "atk", 12.0f }, { "spd", 1.0f }, { "prspd", 1.25f }, { "cooldown", 200.0f } } }, { "Melee_5", new Dictionary<string, float>() { { "hp", 15.0f }, { "atk", 18.0f }, { "spd", 2.5f }, { "prspd", 0f }, { "cooldown", 200.0f } } }, { "Range_5", new Dictionary<string, float>() { { "hp", 40.0f }, { "atk", 10.0f }, { "spd", 1.0f }, { "prspd", 1.5f }, { "cooldown", 200.0f } } }, { "Mage_5", new Dictionary<string, float>() { { "hp", 35.0f }, { "atk", 0f }, { "spd", 1.0f }, { "prspd", 0f }, { "cooldown", 200.0f } } }, { "Mini_5", new Dictionary<string, float>() { { "hp", 10.0f }, { "atk", 5.0f }, { "spd", 1.5f }, { "prspd", 0f }, { "cooldown", 200.0f } } }, { "Melee_6", new Dictionary<string, float>() { { "hp", 25.0f }, { "atk", 5.0f }, { "spd", 2.0f }, { "prspd", 0f }, { "cooldown", 200.0f } } }, { "Range_6", new Dictionary<string, float>() { { "hp", 45.0f }, { "atk", 2.0f }, { "spd", 1.5f }, { "prspd", 1.5f }, { "cooldown", 200.0f } } }, { "Mage_6", new Dictionary<string, float>() { { "hp", 30.0f }, { "atk", 0f }, { "spd", 2.0f }, { "prspd", 0f }, { "cooldown", 200.0f } } }, { "Mini_6", new Dictionary<string, float>() { { "hp", 10.0f }, { "atk", 5.0f }, { "spd", 1.5f }, { "prspd", 0f }, { "cooldown", 200.0f } } }, { "Melee_7", new Dictionary<string, float>() { { "hp", 15.0f }, { "atk", 5.0f }, { "spd", 2.0f }, { "prspd", 0f }, { "cooldown", 200.0f } } }, { "Range_7", new Dictionary<string, float>() { { "hp", 30.0f }, { "atk", 20.0f }, { "spd", 1.0f }, { "prspd", 2.0f }, { "cooldown", 200.0f } } }, { "Mage_7", new Dictionary<string, float>() { { "hp", 40.0f }, { "atk", 25.0f }, { "spd", 1.0f }, { "prspd", 150.0f }, { "cooldown", 200.0f } } }, { "Melee_8", new Dictionary<string, float>() { { "hp", 0f }, { "atk", 20.0f }, { "spd", 1.0f }, { "prspd", 3.0f }, { "cooldown", 200.0f } } }, { "Range_8", new Dictionary<string, float>() { { "hp", 28.0f }, { "atk", 13.0f }, { "spd", 1.0f }, { "prspd", 1.5f }, { "cooldown", 200.0f } } }, { "Mage_8", new Dictionary<string, float>() { { "hp", 35.0f }, { "atk", 0f }, { "spd", 1.0f }, { "prspd", 0f }, { "cooldown", 200.0f } } }, { "Mini_8", new Dictionary<string, float>() { { "hp", 10.0f }, { "atk", 10.0f }, { "spd", 1.0f }, { "prspd", 3.0f }, { "cooldown", 200.0f } } }, { "Melee_9", new Dictionary<string, float>() { { "hp", 18.0f }, { "atk", 9.0f }, { "spd", 1.0f }, { "prspd", 1.0f }, { "cooldown", 200.0f } } }, { "Range_9", new Dictionary<string, float>() { { "hp", 45.0f }, { "atk", 2.0f }, { "spd", 2.0f }, { "prspd", 1.75f }, { "cooldown", 200.0f } } }, { "Mage_9", new Dictionary<string, float>() { { "hp", 35.0f }, { "atk", 4.0f }, { "spd", 1.0f }, { "prspd", 1.0f }, { "cooldown", 200.0f } } }, { "Mini_9", new Dictionary<string, float>() { { "hp", 10.0f }, { "atk", 10.0f }, { "spd", 1.0f }, { "prspd", 1.0f }, { "cooldown", 200.0f } } }, { "Melee_10", new Dictionary<string, float>() { { "hp", 15.0f }, { "atk", 15.0f }, { "spd", 1.5f }, { "prspd", 0f }, { "cooldown", 200.0f } } }, { "Range_10", new Dictionary<string, float>() { { "hp", 26.0f }, { "atk", 15.0f }, { "spd", 1.0f }, { "prspd", 3.0f }, { "cooldown", 200.0f } } }, { "Mage_10", new Dictionary<string, float>() { { "hp", 35.0f }, { "atk", 15.0f }, { "spd", 1.0f }, { "prspd", 150.0f }, { "cooldown", 200.0f } } } };

    public Enemy(EnemyAttack ea, EnemyType et, Room r)
    {
        //Testing purposes
        this.attackType = EnemyAttack.Mage;
        this.enemyType = EnemyType.Party;
        //Regular
        /*this.attackType = ea;
        this.enemyType = et;*/
        //Random position of enemy in room. System to ensure that 2 enemies cannot spawn in the same spot.
        int[] pos = null;
        bool exists = true;
        while (exists)
        {
            int xTrans = r.roomRect[0] + random.Next(4, r.roomRect[2] - 3);
            int yTrans = r.roomRect[1] + random.Next(4, r.roomRect[3] - 3);
            pos = new int[] { xTrans, yTrans };
            exists = false;
            foreach (int[] compare in positionsUsed)
            {
                if (compare[0] == pos[0] && compare[1] == pos[1])
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
            this.position = new Vector3(xTrans, yTrans, -10);
            if (!exists)
            {
                positionsUsed.Add(pos);
            }
        }

        //Put in stats here through dictionary
        Dictionary<string, float> stats = Enemy.enemyStats[$"{this.attackType}_{(int)this.enemyType}"];
        this.hp = stats["hp"];
        this.attackDamage = stats["atk"];
        this.cooldown = stats["cooldown"];
        this.speed = stats["spd"];
        this.projectileSpeed = stats["prspd"];
    }

    public Enemy(EnemyAttack ea, EnemyType et, Vector3 position)
    {
        //Testing purposes
        /*
        this.attackType = EnemyAttack.Melee;
        this.enemyType = EnemyType.PowerTools;*/
        //Regular
        this.attackType = ea;
        this.enemyType = et;
        this.position = position;

        //Put in stats here through dictionary
        Debug.Log($"Key: {this.attackType}_{(int)this.enemyType}");
        Dictionary<string, float> stats = Enemy.enemyStats[$"{this.attackType}_{(int)this.enemyType}"];
        this.hp = stats["hp"];
        this.attackDamage = stats["atk"];
        this.cooldown = stats["cooldown"];
        this.speed = stats["spd"];
        this.projectileSpeed = stats["prspd"];
    }

    public static void resetPositionsUsed()
    {
        positionsUsed.Clear();
    }

    public void takeDamage(float damage)
    {
        this.hp -= damage;
        this.alive = this.hp > 0f;
    }
}
