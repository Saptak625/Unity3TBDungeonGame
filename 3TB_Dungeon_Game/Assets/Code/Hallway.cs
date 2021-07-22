using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    None
}

public class Hallway
{
    public int[] hallwayRect; //Defines where hallway is
    public List<GameObject> gameObjects = null; //Defines gameObjects
    public Direction direction; //Direction of the Hallway from Initial Room
    public static System.Random random = new System.Random(); //Random Number Generator

    public Hallway(Entrance adjacentEntrance)
    {
        //Get a random length for a hallway
        this.direction = adjacentEntrance.direction;
        int length = random.Next(20, 24);
        if (adjacentEntrance.direction == Direction.Up || adjacentEntrance.direction == Direction.Down)
        {
            this.hallwayRect = new int[4] { adjacentEntrance.entranceRect[0] - 1, adjacentEntrance.entranceRect[1] - (adjacentEntrance.direction == Direction.Up ? -1 : length), adjacentEntrance.entranceRect[2] + 1, length };
        }
        else
        {
            this.hallwayRect = new int[4] { adjacentEntrance.entranceRect[0] - (adjacentEntrance.direction == Direction.Right ? -1 : length), adjacentEntrance.entranceRect[1] - 1, length, adjacentEntrance.entranceRect[3] + 1 };
        }
    }

    public void destroy()
    {
        foreach(GameObject g in this.gameObjects)
        {
            Object.Destroy(g);
        }
    }

    public override string ToString()
    {
        return "Hallway: " + this.direction + " " + this.hallwayRect[0]+", "+this.hallwayRect[1];
    }
}