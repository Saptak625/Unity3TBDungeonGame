using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items
{
    //Create all the weapons
    //public static item defaultWeapon = new item(0, 2, 2, false, false); <-- Example
    public static item headphoneLauncher = new item(0, 10, 8, false, false); //default
    public static item automaticEarplugGun = new item(0, 6, 12, false, false);
    public static item solarRayGun = new item(0, 4, 20, false, true); //future laser
    public static item alienGun = new item(0, 10, 10, true, false);
    public static item fireworksLauncher = new item(0, 100, 3, false, false);

    //Create all the shields
    //public static item defaultShield = new item(0.2f, 500, 500, 3); <-- Example
    public static item earplugShield = new item(0.8f, 400, 400, 1);
    public static item headphoneShield = new item(0.6f, 400, 400, 2);
    public static item noiseCancellingHPShield = new item(0.4f, 400, 400, 3);
    public static item headphonesAndEarplugsShield = new item(0.3f, 400, 500, 3);
    public static item noShield = new item(1f, 100, 100, 1); //default

    //Put them in lists
    public static item[] items = new item[10] {headphoneLauncher, automaticEarplugGun, solarRayGun, alienGun, fireworksLauncher,
    earplugShield, headphoneShield, noiseCancellingHPShield, headphonesAndEarplugsShield, noShield}; //Fill in later
    public static int itemsListMax = items.Length - 1;
}

public class item
{
    public string type;

    //for weapons

    public int damage; //Amount of damage dealt
    public int bulletSpeed; //Bullet travel Speed
    public bool bulletsBounce;
    public bool isLaser;
    public item(int zero, int attack, int speed, bool bouncingBullets, bool fireslaser) //Zero is just to make constructor parameter numbers different
    {
        this.type = "Weapon";
        this.damage = attack;
        this.bulletSpeed = speed;
        this.bulletsBounce = bouncingBullets;
        this.isLaser = fireslaser;
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