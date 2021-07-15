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
    List<Entrances> outEntrances; //Entrances that can open to lead out to other rooms
    List<Entrances> inEntrances; //Entrances that can open to lead into room
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
        this.roomRect = new int[4] {-xPos, -yPos, xPos*2, yPos*2};
        this.isBossRoom = false;
        this.isChestRoom = false;
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

    public void setGameObjects() //
    {

    }
}
