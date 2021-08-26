using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType
{
    Destructable = 1,
    Undestructable = 2
}

public class Obstacle
{
    public ObstacleType type;
    public int[] obstacleRect; //Defines where obstacle is
    public List<GameObject> gameObjects = null; //Defines gameObjects

    public Obstacle(ObstacleType ot, int posX, int posY, int width, int height)
    {
        this.type = ot;
        this.obstacleRect = new int[4] { posX, posY, width, height };
    }

    public void destroy()
    {
        foreach (GameObject g in this.gameObjects)
        {
            Object.Destroy(g);
        }
    }

    public List<int[]> getPositions()
    {
        List<int[]> l = new List<int[]>();
        for (int i = 0; i < this.obstacleRect[2]; i++)
        {
            for (int j = 0; j < this.obstacleRect[3]; j++)
            {
                l.Add(new int[2] { i + this.obstacleRect[0], j + this.obstacleRect[1] });
            }
        }
        return l;
    }
}
