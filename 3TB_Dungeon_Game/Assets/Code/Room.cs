//Creates the Room class

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public bool isChestRoom = false; //Controls whether room is a chest room
    public bool isBossRoom = false; //Controls whether room is a boss room
    public bool active = false; //Controls whether Enemies become Hostile
    public List<Enemy> activeEnemies = new List<Enemy>(); //These enemies can still attack
    public List<Enemy> slainEnemies = new List<Enemy>(); //These enemies corpses are shown
    public List<Entrance> outEntrances = new List<Entrance>(); //Entrances that can open to lead out to other rooms
    public List<Entrance> inEntrances = new List<Entrance>(); //Entrances that can open to lead into room
    public int[] roomRect; //Defines where room is
    public List<GameObject> gameObjects; //Defines gameObjects
    public static int roomsPassedWithoutChest = 0; //Counter for Rooms without chests
    public static int roomsPassedWithoutBoss = 0; //Counter for Rooms without boss

    public Room()
    {
        //Initialize Starter Room
        System.Random random = new System.Random();
        int xPos = random.Next(8, 12);
        int yPos = random.Next(8, 12);
        this.roomRect = new int[4] { -xPos, -yPos, (xPos * 2) + 1, (yPos * 2) + 1 };
        this.isBossRoom = false;
        this.isChestRoom = false;
        Direction[] directions = new Direction[4] { Direction.Up, Direction.Down, Direction.Left, Direction.Right };
        foreach (Direction d in directions)
        {
            this.outEntrances.Add(new Entrance(this, d, false));
        }
    }


    public Room(Hallway hallway)
    {
        //Initialize Room
        System.Random random = new System.Random();
        int xPos = random.Next(8, 12);
        int yPos = random.Next(8, 12);
        if (hallway.direction == Direction.Up)
            this.roomRect = new int[4] { -xPos, -yPos, (xPos * 2) + 1, (yPos * 2) + 1 };
        if (!this.isBossRoom)
        {

        }
        else if (!this.isChestRoom)
        {
            roomsPassedWithoutChest++;
        }
    }

    public void replaceSlainEnemy(Enemy enemy)
    { //Called by Enemy once dead
        //Removes enemy from activeEnemies and adds it to slainEnemies
    }

    public void startBattle()
    {
        //Lock dungeon entrances and start battle
    }
}