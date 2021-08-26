//Virtual room and hallway state loader

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLoader
{
    public Room activeRoom = null; //Defines room which is active and battle is occuring
    public List<List<Room>> roomQueue = new List<List<Room>>(); //Queue of RoomLists based on custom FIFO. One room from first list will be selected and be used to generate next queue item before popping.
    public List<bool> roomLoadedList = new List<bool>(); //Room Queue load states. Used by RoomLoaderSpawner to keep track of roomQueue batch loading.
    public List<List<List<Hallway>>> hallwayQueue = new List<List<List<Hallway>>>(); //Queue of Lists of HallwayLists based on custom FIFO. One set of hallwayLists will be selected and be used to generate next queue item before popping. 
    public List<bool> hallwayLoadedList = new List<bool>(); //Hallway Queue load states. Used by RoomLoaderSpawner to keep track of lists of hallwayQueues batch loading.
    public List<Room> unloadRoomQueue = new List<Room>();
    public List<Hallway> unloadHallwayQueue = new List<Hallway>();
    public Hallway staticHallway = null;

    public RoomLoader()
    {
        //Initialize all rooms and rooms from the beginning 
        List<Room> initialRoomList = new List<Room>() { new Room() };
        List<List<Hallway>> initialHallwayList = new List<List<Hallway>>();
        foreach (Entrance e1 in initialRoomList[0].outEntrances) //Generating room and hallway subtree.
        {
            //Create parent hallway for specific direction
            Hallway parentHallway = new Hallway(e1);
            //Create Set of hallways in particular direction and initialize with parent at index 0.
            List<Hallway> hallwayList = new List<Hallway>();
            hallwayList.Add(parentHallway);
            //Create subroom from parentHallway and add it to initialRoomList
            Room subRoom = new Room(parentHallway);
            initialRoomList.Add(subRoom);
            //Find all sub-hallways from new sub room
            foreach (Entrance e2 in subRoom.outEntrances)
            {
                //Add sub hallways in particular direction into hallwayList
                hallwayList.Add(new Hallway(e2));
            }
            //Add Set of Hallways into initialHallwayList
            initialHallwayList.Add(hallwayList);
        }
        //Initialize Loader State
        this.roomQueue.Add(initialRoomList);
        this.roomLoadedList.Add(false);
        this.hallwayQueue.Add(initialHallwayList);
        this.hallwayLoadedList.Add(false);
    }

    public void loadAndUnloadRoomsAndHallways()
    { //Called once Player clears a dungeon
        //Pop and Push New Hallways and Rooms into according queues
        Direction keep = this.activeRoom.roomDirection;

        //Push staticHallway to unload if exists
        if (this.staticHallway != null)
        {
            this.unloadHallwayQueue.Add(this.staticHallway);
        }

        //Get adjacent rooms and hallways
        Room newParentRoom = null;
        this.unloadRoomQueue.Add(this.roomQueue[0][0]);
        for (int i = 1; i < this.roomQueue[0].Count; i++)
        {
            Room r = this.roomQueue[0][i];
            if (r.roomDirection == keep)
            {
                newParentRoom = r;
            }
            else
            {
                this.unloadRoomQueue.Add(r);
            }
        }
        newParentRoom.trigger = null;
        List<Hallway> newParentHallways = null;
        foreach (List<Hallway> lh in this.hallwayQueue[0])
        {
            if (lh[0].direction == keep)
            {
                newParentHallways = lh;
            }
            else
            {
                this.unloadHallwayQueue.AddRange(lh);
            }
        }

        //Make first entry of newParentHallways into new staticHallway and pop from beginning of list.
        this.staticHallway = newParentHallways[0];
        newParentHallways.RemoveAt(0);

        //Pop all old entries and Push new entries
        //Pop first
        this.roomQueue.RemoveAt(0);
        this.roomLoadedList.RemoveAt(0);
        this.hallwayQueue.RemoveAt(0);
        this.hallwayLoadedList.RemoveAt(0);
        //Push new queues
        List<Room> newRoomQueueItem = new List<Room>() { newParentRoom };
        List<List<Hallway>> newHallwayQueueItem = new List<List<Hallway>>();
        foreach (Hallway h in newParentHallways)
        {
            Room newSubRoom = new Room(h);
            List<Hallway> newHallwaySet = new List<Hallway>();
            newHallwaySet.Add(h);
            foreach (Entrance e in newSubRoom.outEntrances)
            {
                newHallwaySet.Add(new Hallway(e));
            }
            newRoomQueueItem.Add(newSubRoom);
            newHallwayQueueItem.Add(newHallwaySet);
        }
        this.roomQueue.Add(newRoomQueueItem);
        this.roomLoadedList.Add(false);
        this.hallwayQueue.Add(newHallwayQueueItem);
        this.hallwayLoadedList.Add(false);
    }

    //Used to update Room Loader states indirectly
    public void loadedRoom(int index)
    {
        this.roomLoadedList[index] = true;
    }

    public void loadedHallway(int index)
    {
        this.hallwayLoadedList[index] = true;
    }
}