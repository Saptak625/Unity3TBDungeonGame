using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room 
{
    bool isChestRoom; //Controls whether room is a chest room
    bool isBossRoom; //Controls whether room is a boss room
    bool active; //Controls whether Enemies become Hostile
    List<Enemy> activeEnemies; //These enemies can still attack
    List<Enemy> slainEnemies; //These enemies corpses are shown
    List<Entrance> outEntrances; //Entrances that can open to lead out to other rooms
    List<Entrance> inEntrances; //Entrances that can open to lead into room
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
        this.roomRect = new int[4] {-xPos, -yPos, (xPos*2)+1, (yPos*2)+1};
        this.isBossRoom = false;
        this.isChestRoom = false;
        this.inEntrances = new List<Entrance>();
        this.outEntrances = new List<Entrance>();
        Direction[] directions = new Direction[4] { Direction.Up, Direction.Down, Direction.Left, Direction.Right };
        foreach(Direction d in directions)
        {
            this.outEntrances.Add(new Entrance(this, d, false));
        }
    }
        

    public Room(Hallway hallway)
    {
        //Initialize Room

        if (!this.isChestRoom)
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

    public void setGameObjects() //Used to set game objects 
    {

    }
}
