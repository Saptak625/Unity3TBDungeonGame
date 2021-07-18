//Creates Rooms in Unity

using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomLoaderSpawner : MonoBehaviour
{
    RoomLoader roomLoader;
    public GameObject dungeonFloorTile;
    public GameObject dungeonWallTile;
    public GameObject dungeonEntranceTile;

    public List<GameObject> instantiateGrid(GameObject tileType, int posX, int posY, int iterX, int iterY, int spacing)
    {
        List<GameObject> gameObjects = new List<GameObject>();
        for (int i = 0; i < iterX; i++)
        {
            for (int j = 0; j < iterY; j++)
            {
                GameObject gridObject = Instantiate(tileType, new Vector3(posX + (spacing * i), posY + (spacing * j), 0), Quaternion.identity);
                gameObjects.Add(gridObject);
            }
        }
        return gameObjects;
    }

    public void instantiateRoom(Room r)
    {
        //Create floor
        List<GameObject> floorGrid = this.instantiateGrid(this.dungeonFloorTile, r.roomRect[0] + 1, r.roomRect[1] + 1, r.roomRect[2] - 2, r.roomRect[3] - 2, 1);
        //Create walls
        List<GameObject> wallGrid = this.instantiateGrid(this.dungeonWallTile, r.roomRect[0], r.roomRect[1], 1, r.roomRect[3], 1);
        wallGrid.AddRange(this.instantiateGrid(this.dungeonWallTile, r.roomRect[0] + r.roomRect[2] - 1, r.roomRect[1], 1, r.roomRect[3], 1));
        wallGrid.AddRange(this.instantiateGrid(this.dungeonWallTile, r.roomRect[0] + 1, r.roomRect[1], r.roomRect[2] - 2, 1, 1));
        wallGrid.AddRange(this.instantiateGrid(this.dungeonWallTile, r.roomRect[0] + 1, r.roomRect[1] + r.roomRect[3] - 1, r.roomRect[2] - 2, 1, 1));

        //Create Entrances with according tile
        List<GameObject> inEntranceGrid = new List<GameObject>();
        List<GameObject> outEntranceGrid = new List<GameObject>();
        foreach (Entrance e in r.inEntrances)
        {
            inEntranceGrid.AddRange(this.instantiateGrid(this.dungeonEntranceTile, e.entranceRect[0], e.entranceRect[1], e.entranceRect[2], e.entranceRect[3], 1));
        }
        foreach (Entrance e in r.outEntrances)
        {
            outEntranceGrid.AddRange(this.instantiateGrid(this.dungeonEntranceTile, e.entranceRect[0], e.entranceRect[1], e.entranceRect[2], e.entranceRect[3], 1));
        }

        //Remove Walls in Entrance Space
        List<GameObject> updatedWallGrid = new List<GameObject>();
        foreach (GameObject w in wallGrid)
        {
            bool positionUsed = false;
            foreach (GameObject e in inEntranceGrid)
            {
                if (e.transform.position.x == w.transform.position.x && e.transform.position.y == w.transform.position.y)
                {
                    positionUsed = true;
                }
            }
            foreach (GameObject e in outEntranceGrid)
            {
                if (e.transform.position.x == w.transform.position.x && e.transform.position.y == w.transform.position.y)
                {
                    positionUsed = true;
                }
            }
            if (!positionUsed)
            {
                updatedWallGrid.Add(w);
            }
        }
        wallGrid = updatedWallGrid;

        //Make walls solid
        foreach (GameObject w in wallGrid)
        {
            w.AddComponent(typeof(BoxCollider2D));
        }

        //Add boxColliders if needed

        //Combine all gameObjects and store
        floorGrid.AddRange(wallGrid);
        r.gameObjects = floorGrid;
    }

    public void instantiateHallway(Hallway h)
    {
        List<GameObject> floorGrid;
        List<GameObject> wallGrid;
        if (h.direction == Direction.Up || h.direction == Direction.Down)
        {
            //Create floor
            floorGrid = this.instantiateGrid(this.dungeonFloorTile, h.hallwayRect[0] + 1, h.hallwayRect[1], h.hallwayRect[2] - 1, h.hallwayRect[3], 1);
            //Create walls
            wallGrid = this.instantiateGrid(this.dungeonWallTile, h.hallwayRect[0], h.hallwayRect[1], 1, h.hallwayRect[3], 1);
            wallGrid.AddRange(this.instantiateGrid(this.dungeonWallTile, h.hallwayRect[0] + h.hallwayRect[2], h.hallwayRect[1], 1, h.hallwayRect[3], 1));
        }
        else
        {
            //Create floor
            floorGrid = this.instantiateGrid(this.dungeonFloorTile, h.hallwayRect[0], h.hallwayRect[1] + 1, h.hallwayRect[2], h.hallwayRect[3] - 1, 1);
            //Create walls
            wallGrid = this.instantiateGrid(this.dungeonWallTile, h.hallwayRect[0], h.hallwayRect[1], h.hallwayRect[2], 1, 1);
            wallGrid.AddRange(this.instantiateGrid(this.dungeonWallTile, h.hallwayRect[0], h.hallwayRect[1] + h.hallwayRect[3], h.hallwayRect[2], 1, 1));
        }
        foreach (GameObject w in wallGrid)
        {
            w.AddComponent(typeof(BoxCollider2D));
        }

        //Combine all gameObjects and store
        floorGrid.AddRange(wallGrid);
        h.gameObjects = floorGrid;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.roomLoader = new RoomLoader();
    }

    // Update is called once per frame
    void Update()
    {
        //Load all Rooms
        for (int i = 0; i < this.roomLoader.roomLoadedList.Count; i++)
        {
            if (!this.roomLoader.roomLoadedList[i])
            {
                //Corresponding Room List needs to be loaded.
                foreach (Room r in this.roomLoader.roomQueue[i])
                {
                    this.instantiateRoom(r);
                }
                //Set load state to true
                this.roomLoader.loadedRoom(i);
            }
        }
        //Load all Hallways   
        for (int i = 0; i < this.roomLoader.hallwayLoadedList.Count; i++)
        {
            if (!this.roomLoader.hallwayLoadedList[i])
            {
                //Corresponding Hallway List needs to be loaded.
                foreach (Hallway h in this.roomLoader.hallwayQueue[i])
                {
                    this.instantiateHallway(h);
                }
                //Set load state to true
                this.roomLoader.loadedHallway(i);
            }
        }

    }
}