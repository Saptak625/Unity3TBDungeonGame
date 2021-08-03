using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items
{
    //Create all the weapons
    public static item defaultWeapon = new item(2, 2, 2); //temporary numbers

    //Create all the shields
    public static item defaultShield = new item(0.2f, 500, 500, 3); //temporary numbers

    //Put them in lists
    public static item[] items = new item[2] {defaultWeapon, defaultShield}; //Fill in later
    public static int itemsListMax = items.Length - 1;
}

public class item
{
    public string type;

    //for weapons

    public int damage; //Amount of damage dealt
    public int reloadSpeed; //Time between use
    public int bulletSpeed; //Bullet travel Speed
    public item(int attack, int attackSpeed, int speed)
    {
        this.type = "Weapon";
        this.damage = attack;
        this.reloadSpeed = attackSpeed;
        this.bulletSpeed = speed;
    }

    //for shields

    public float resistance; //Damage reduction
    public int cooldown; //Time between use (seconds * 100)
    public int duration; //Time during use (seconds * 100)
    public int speedReduction; //Lowers speed when being used (Max is 4)
    public item(float protection, int reloadSpeed, int time, int lowerSpeed)
    {
        this.type = "Shield";
        this.resistance = protection;
        this.cooldown = reloadSpeed;
        this.duration = time;
        this.speedReduction = lowerSpeed;
    }
}

//TODO: Create more items and add them to the items list