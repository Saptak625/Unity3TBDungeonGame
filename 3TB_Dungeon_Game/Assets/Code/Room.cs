//Creates the Room class

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public bool isChestRoom = false; //Controls whether room is a chest room
    public bool isBossRoom = false; //Controls whether room is a boss room
    public List<List<Enemy>> activeEnemies = new List<List<Enemy>>(); //Enemies Waves that still are upcoming
    public List<Enemy> slainEnemies = new List<Enemy>(); //These enemies corpses are shown
    public List<Obstacle> obstacles = new List<Obstacle>(); //These are the obstacles in the room.
    public List<int[]> unwalkablePositions = new List<int[]>(); //Positions where enemies cannot spawn
    public List<Entrance> outEntrances = new List<Entrance>(); //Entrances that can open to lead out to other rooms
    public List<Entrance> inEntrances = new List<Entrance>(); //Entrances that can open to lead into room
    public int[] roomRect; //Defines where room is
    public List<GameObject> gameObjects = null; //Defines gameObjects
    public static int variableChestCounter = 0; //Variable Counter for Rooms without chests
    public static int variableBossCounter = 0; //Variable Counter for Rooms without boss
    public Direction roomDirection = Direction.None; //Direction of subroom in term of parent room. Root room will have Direction.None.
    public GameObject trigger = null; //Trigger used to check if room is to be executed.
    public EnemyType[] enemyTypeArray = null; //Indicates which types of enemies this room holds.
    public static System.Random random = new System.Random(); //Random object for proper generation

    public Room()
    {
        //Initialize Starter Room;
        int xPos = random.Next(8, 12);
        int yPos = random.Next(8, 12);
        this.roomRect = new int[4] { -xPos, -yPos, (xPos * 2) + 1, (yPos * 2) + 1 };
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
            roomPosY = hallway.hallwayRect[1] + (hallway.direction == Direction.Down ? -yLength - 1 : yLength + hallway.hallwayRect[3]);
        }
        else
        {
            roomPosX = hallway.hallwayRect[0] + (hallway.direction == Direction.Left ? -xLength - 1 : xLength + hallway.hallwayRect[2]);
            roomPosY = hallway.hallwayRect[1] + 3;
        }
        this.roomRect = new int[4] { roomPosX - xLength, roomPosY - yLength, (xLength * 2) + 1, (yLength * 2) + 1 };
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
        this.isChestRoom = Room.variableChestCounter >= random.Next(1, 21);
        /*
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
        }*/

        //Create obstacles and enemies only if not chest room or boss room
        if (!this.isChestRoom && !this.isBossRoom)
        {
            //Create Obstacles
            int numberOfObstacles = random.Next(4, 9);
            int obstacleCounter = 0;
            int numberOfIterations = 0;
            while (obstacleCounter < numberOfObstacles || numberOfIterations > 8)
            {
                int shapeType = random.Next(1, 3);
                int blockType = random.Next(1, 3);
                int[] startPos = new int[] { this.roomRect[0] + random.Next(5, this.roomRect[2] - 5), this.roomRect[1] + random.Next(5, this.roomRect[3] - 5) };
                bool valid = true;
                Obstacle o;
                if (shapeType == 1) // Wall Shape
                {
                    int wallLength = random.Next(3, 6);
                    int orientation = random.Next(1, 3);
                    if (orientation == 1) // Horizontal
                    {
                        o = new Obstacle((ObstacleType)blockType, startPos[0], startPos[1], wallLength, 1);
                    }
                    else //Vertical
                    {
                        o = new Obstacle((ObstacleType)blockType, startPos[0], startPos[1], 1, wallLength);
                    }
                }
                else //Block Shape
                {
                    int sideLength = random.Next(2, 4);
                    o = new Obstacle((ObstacleType)blockType, startPos[0], startPos[1], sideLength, sideLength);
                }
                List<int[]> objectPositions = new List<int[]>();
                foreach (int[] objectPos in objectPositions)
                {
                    if (objectPos[0] < this.roomRect[0] + 3 || objectPos[0] > this.roomRect[0] + this.roomRect[2] - 3 || objectPos[1] < this.roomRect[1] + 3 || objectPos[1] > this.roomRect[1] + this.roomRect[3] - 3)
                    {
                        valid = false;
                        break;
                    }
                    foreach (int[] usedPos in this.unwalkablePositions)
                    {
                        if (objectPos[0] == usedPos[0] && objectPos[1] == usedPos[1])
                        {
                            valid = false;
                            break;
                        }
                    }
                    if (!valid)
                    {
                        break;
                    }
                }
                if (valid)
                {
                    this.obstacles.Add(o);
                    this.unwalkablePositions.AddRange(o.getPositions());
                    obstacleCounter++;
                }
                numberOfIterations++;
            }

            //Assign enemy waves
            int numberOfWaves = random.Next(2, 4);
            EnemyAttack[] enemyAttackArray = new EnemyAttack[] { EnemyAttack.Melee, EnemyAttack.Range, EnemyAttack.Mage };
            this.enemyTypeArray = new EnemyType[] { (EnemyType)random.Next(1, 11), (EnemyType)random.Next(1, 11) };
            for (int i = 0; i < numberOfWaves; i++)
            {
                List<Enemy> wave = new List<Enemy>();
                int numberOfEnemies = random.Next(10, 16); //Spawns in 10 to 15 enemies.
                int attackIter = 0;
                int typeIter = 0;
                for (int j = 0; j < numberOfEnemies; j++)
                {
                    wave.Add(new Enemy(enemyAttackArray[attackIter], this.enemyTypeArray[typeIter], this));
                    if (attackIter < enemyAttackArray.Length - 1)
                    {
                        attackIter++;
                    }
                    else
                    {
                        attackIter = 0;
                        typeIter++;
                    }
                    if (typeIter >= this.enemyTypeArray.Length)
                    {
                        typeIter = 0;
                    }
                }
                this.activeEnemies.Add(wave);
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
        //Room.variableBossCounter += (boss ? -2 : 1);
        if (Room.variableChestCounter > 16)
        {
            Room.variableChestCounter = 16;
        }
        /*if(Room.variableBossCounter > 16)
        {
            Room.variableBossCounter = 16;
        }*/
    }

    public override string ToString()
    {
        return "Room: " + this.roomDirection + " " + this.roomRect[0] + ", " + this.roomRect[1];
    }

    public void destroy()
    {
        //Destroy all gameObjects 
        foreach (GameObject g in this.gameObjects)
        {
            Object.Destroy(g);
        }
        //Call destroy on Entrances
        foreach (Entrance e in this.inEntrances)
        {
            e.destroy();
        }
        foreach (Entrance e in this.outEntrances)
        {
            e.destroy();
        }
        //Call Enemy destroy property
        foreach (Enemy e in this.slainEnemies)
        {
            e.destroy = true;
        }
        //Destroy all Obstacles
        foreach (Obstacle o in this.obstacles)
        {
            if (o != null)
            {
                o.destroy();
            }
        }
    }
}