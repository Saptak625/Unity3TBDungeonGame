using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items
{
    //Create all the weapons
    //public static item defaultWeapon = new item(0, 2, 2, 500, false, false); <-- Example
    public static item headphoneLauncher = new item(0, 10, 8, 25, false, false); //default
    public static item automaticEarplugGun = new item(1, 6, 12, 20, false, false);
    public static item solarRayGun = new item(2, 4, 20, 12, false, true); //future laser
    public static item alienGun = new item(3, 10, 10, 22, true, false);
    public static item fireworksLauncher = new item(4, 100, 3, 50, false, false);

    //Create all the shields
    //public static item defaultShield = new item(0.2f, 500, 500, 3); <-- Example
    public static item earplugShield = new item(5, 0.8f, 100, 100, 1);
    public static item headphoneShield = new item(6, 0.6f, 100, 100, 2);
    public static item noiseCancellingHPShield = new item(7, 0.4f, 100, 100, 3);
    public static item headphonesAndEarplugsShield = new item(8, 0.3f, 100, 125, 3);
    public static item noShield = new item(9, 1f, 25, 25, 1); //default

    //Put them in lists
    public static item[] items = new item[10] {headphoneLauncher, automaticEarplugGun, solarRayGun, alienGun, fireworksLauncher,
    earplugShield, headphoneShield, noiseCancellingHPShield, headphonesAndEarplugsShield, noShield}; //Fill in later
    public static int itemsListMax = items.Length - 1;
}

public class item
{
    public string type;
    public int ID;

    //for weapons

    public int damage; //Amount of damage dealt
    public int bulletSpeed; //Bullet travel Speed
    public int reloadSpeed;
    public bool bulletsBounce;
    public bool isLaser;
    public item(int identification, int attack, int speed, int cooldown, bool bouncingBullets, bool fireslaser)
    {
        this.type = "Weapon";
        this.ID = identification;
        this.damage = attack;
        this.bulletSpeed = speed;
        this.reloadSpeed = cooldown;
        this.bulletsBounce = bouncingBullets;
        this.isLaser = fireslaser;
    }

    //for shields

    public float resistance; //Damage reduction
    public int cooldown; //Time between use (seconds * 100)
    public int duration; //Time during use (seconds * 100)
    public int speedReduction; //Lowers speed when being used (Max is 4)
    public item(int identification, float protection, int reloadSpeed, int time, int lowerSpeed)
    {
        this.type = "Shield";
        this.ID = identification;
        this.resistance = protection;
        this.cooldown = reloadSpeed;
        this.duration = time;
        this.speedReduction = lowerSpeed;
    }
}

//TODO: Create more items and add them to the items list