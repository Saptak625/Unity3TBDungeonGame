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
    public List<GameObject> gameObjects = null; //Defines gameObjects
    public static int variableChestCounter = 10; //Variable Counter for Rooms without chests
    public static int variableBossCounter = 10; //Variable Counter for Rooms without boss
    public Direction roomDirection = Direction.None; //Direction of subroom in term of parent room. Root room will have Direction.None.
    public GameObject trigger = null; //Trigger used to check if room is to be executed.
    public static System.Random random = new System.Random(); //Random object for proper generation

    public Room()
    {
        //Initialize Starter Room;
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
        this.roomDirection = hallway.direction;
        int xLength = random.Next(8, 12);
        int yLength = random.Next(8, 12);
        int roomPosX;
        int roomPosY;
        if (hallway.direction == Direction.Up || hallway.direction == Direction.Down)
        {
            roomPosX = hallway.hallwayRect[0] + 3;
            roomPosY = hallway.hallwayRect[1] + (hallway.direction == Direction.Down ? -yLength-1 : yLength + hallway.hallwayRect[3]);
        }
        else
        {
            roomPosX = hallway.hallwayRect[0] + (hallway.direction == Direction.Left ? -xLength-1 : xLength + hallway.hallwayRect[2]);
            roomPosY = hallway.hallwayRect[1] + 3;
        }
        this.roomRect = new int[4] { roomPosX-xLength, roomPosY-yLength, (xLength*2)+1, (yLength*2)+1};
        Direction[] directions = new Direction[4] { Direction.Up, Direction.Down, Direction.Left, Direction.Right };
        Direction inverseDirection = (hallway.direction == Direction.Up ? Direction.Down : (hallway.direction == Direction.Down ? Direction.Up : (hallway.direction == Direction.Right ? Direction.Left : Direction.Right)));
        foreach (Direction d in directions)
        {
            if (d == inverseDirection)
            {
                this.inEntrances.Add(new Entrance(this, d, false));
            }
            else
            {
                this.outEntrances.Add(new Entrance(this, d, true));
            }
        }

        //Randomly determine whether there is a boss
        int randomPrevalence = random.Next(1, 3);
        int numberToBeat1 = random.Next(1, 21);
        int numberToBeat2 = random.Next(1, 21);
        if (randomPrevalence == 1)
        {
            if(Room.variableBossCounter >= numberToBeat1)
            {
                this.isBossRoom = true;
            }
            else
            {
                this.isChestRoom = Room.variableChestCounter >= numberToBeat2;
            }
        }
        else
        {
            if (Room.variableChestCounter >= numberToBeat1)
            {
                this.isChestRoom = true;
            }
            else
            {
                this.isBossRoom = Room.variableBossCounter >= numberToBeat2;
            }
        }
    }

    public void replaceSlainEnemy(Enemy enemy)
    { //Called by Enemy once dead
        //Removes enemy from activeEnemies and adds it to slainEnemies
    }

    public static void roomStatsIncrement(bool chest, bool boss)
    {
        Room.variableChestCounter += (chest ? -2 : 1);
        Room.variableBossCounter += (boss ? -2 : 1);
        if(Room.variableChestCounter > 16)
        {
            Room.variableChestCounter = 16;
        }
        if(Room.variableBossCounter > 16)
        {
            Room.variableBossCounter = 16;
        }
    }

    public override string ToString()
    {
        return "Room: " + this.roomDirection + " " + this.roomRect[0]+", "+this.roomRect[1];
    }

    public void destroy()
    {
        //Destroy all gameObjects 
        foreach(GameObject g in this.gameObjects)
        {
            Object.Destroy(g);
        }
        //Call destroy on Entrances
        foreach(Entrance e in this.inEntrances)
        {
            e.destroy();
        }
        foreach(Entrance e in this.outEntrances)
        {
            e.destroy();
        }
    }
}