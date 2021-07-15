using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class Hallway
{
    int[] hallwayRect; //Defines where hallway is

    public Hallway(Room r, Direction d)
    {
        //Get a random length for a hallway
        System.Random random = new System.Random();
        int xPos = random.Next(8, 12);
        int yPos = random.Next(8, 12);
        this.hallwayRect = new int[4] { -xPos, -yPos, (xPos * 2) + 1, (yPos * 2) + 1 };
    }

    public void deleteSprites()
    {
        //Control deleting sprites for hallway
    }
}
