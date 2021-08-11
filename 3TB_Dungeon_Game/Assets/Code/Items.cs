using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items
{
    //Create all the weapons
    //public static item defaultWeapon = new item(2, 0, 2, false, false); <-- Example
    public static item headphoneLauncher = new item(10, 0, 8, false, false); //Regular weapon
    public static item automaticEarplugGun = new item(6, 0, 16, false, false); //Fast attack
    public static item solarRayGun = new item(4, 0, 20, false, true); //Will be laser in updates, but now its a super fast
    public static item alienGun = new item(8, 0, 10, true, false); //Bouncing
    public static item fireworkLauncher = new item(100, 0, 3, false, false); //Slow One-hit obliterator

    //Create all the shields
    //public static item defaultShield = new item(0.2f, 500, 500, 3); <-- Example
    public static item earplugShield = new item(0.8f, 400, 400, 1);
    public static item headphoneShield = new item(0.6f, 400, 400, 2);
    public static item headphoneNCShield = new item(0.4f, 400, 400, 3); //NC = Noise Cancelling
    public static item headphoneAndEarplugs = new item(0.2f, 400, 400, 3);
    public static item noShield = new item(1.0f, 100, 100, 1); //Default

    //Puts them in lists
    public static item[] items = new item[10] {headphoneLauncher, automaticEarplugGun, solarRayGun, alienGun, fireworkLauncher,
    earplugShield, headphoneShield, headphoneNCShield, headphoneAndEarplugs, noShield};
    public static int itemsListMax = items.Length - 1;
}

public class item
{
    public string type;

    //for weapons

    public int damage; //Amount of damage dealt
    public int zero; //Placeholder
    public int bulletSpeed; //Bullet travel Speed
    public bool bulletsBounce;
    bool isLaser;
    public item(int attack, int placeholder, int speed, bool hasBouncingBullets, bool ShootsALaser)
    {
        this.type = "Weapon";
        this.damage = attack;
        this.zero = placeholder;
        this.bulletSpeed = speed;
        this.bulletsBounce = hasBouncingBullets;
        this.isLaser = ShootsALaser;
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