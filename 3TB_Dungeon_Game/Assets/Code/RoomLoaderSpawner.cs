using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomLoaderSpawner : MonoBehaviour
{
    RoomLoader roomLoader;
    public GameObject dungeonFloor;
    public GameObject dungeonWall;

    public void instantiateRoom(Room r)
    {
        //Create floor
        Instantiate(this.dungeonFloor, new Vector3(r.roomRect[0] + (r.roomRect[2]/2), r.roomRect[1] + (r.roomRect[3]/2), 0), Quaternion.identity);
        //Create walls
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
