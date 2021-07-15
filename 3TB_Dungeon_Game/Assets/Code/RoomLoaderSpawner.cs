using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomLoaderSpawner : MonoBehaviour
{
    RoomLoader roomLoader;
    public GameObject dungeonFloorTile;
    public GameObject dungeonWallTile;

    public List<GameObject> instantiateGrid(GameObject tileType, int posX, int posY, int iterX, int iterY, int spacing)
    {
        List<GameObject> gameObjects = new List<GameObject>();
        for(int i = 0; i < iterX; i++)
        {
            for (int j = 0; j < iterY; j++)
            {
                GameObject gridObject=Instantiate(tileType, new Vector3(posX+(spacing*i), posY+(spacing*j), 0), Quaternion.identity);
                gameObjects.Add(gridObject);
            }
        }
        return gameObjects;
    }

    public void instantiateRoom(Room r)
    {
        foreach(int i in r.roomRect)
        {
            Debug.Log(i);
        }
        //Create floor
        List<GameObject> floorGrid = this.instantiateGrid(this.dungeonFloorTile, r.roomRect[0]+1, r.roomRect[1]+1, r.roomRect[2]-2, r.roomRect[3]-2, 1);
        //Create walls
        List<GameObject> wallGrid = this.instantiateGrid(this.dungeonWallTile, r.roomRect[0], r.roomRect[1], 1, r.roomRect[3], 1);
        wallGrid.AddRange(this.instantiateGrid(this.dungeonWallTile, r.roomRect[0]+r.roomRect[2]-1, r.roomRect[1], 1, r.roomRect[3], 1));
        wallGrid.AddRange(this.instantiateGrid(this.dungeonWallTile, r.roomRect[0]+1, r.roomRect[1], r.roomRect[2]-2, 1, 1));
        wallGrid.AddRange(this.instantiateGrid(this.dungeonWallTile, r.roomRect[0]+1, r.roomRect[1]+r.roomRect[3]-1, r.roomRect[2]-2, 1, 1));
        //Make walls solid
        foreach(GameObject w in wallGrid)
        {
            w.AddComponent(typeof(BoxCollider2D));
        }
    }

    public void instantiateHallway(Hallway h)
    {
        //Create floor
        
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
        for(int i=0; i < this.roomLoader.roomLoadedList.Count; i++)
        {
            if (!this.roomLoader.roomLoadedList[i])
            {
                //Corresponding Room List needs to be loaded.
                foreach(Room r in this.roomLoader.roomQueue[i])
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
