//Holds rooms and hallways

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLoader
{
    public List<List<Room>> roomQueue = new List<List<Room>>();
    public List<bool> roomLoadedList = new List<bool>();
    public List<List<Hallway>> hallwayQueue = new List<List<Hallway>>();
    public List<bool> hallwayLoadedList = new List<bool>();

    public RoomLoader()
    {
        //Initialize all rooms and rooms from the beginning 
        List<Room> initialRoomList = new List<Room>() { new Room() };
        List<Hallway> initialHallwayList = new List<Hallway>();
        foreach (Entrance e in initialRoomList[0].outEntrances)
        {
            Hallway h = new Hallway(e);
            initialHallwayList.Add(h);
            initialRoomList.Add(new Room(h));
        }
        //Initialize all Loader Variables
        this.roomQueue.Add(initialRoomList);
        this.roomLoadedList.Add(false);
        this.hallwayQueue.Add(initialHallwayList);
        this.hallwayLoadedList.Add(false);
    }

    public void loadNewRooms(int enterDirection)
    { //Called once Player leaves last dungeon
        //Push New Hallways and Rooms into according queues
    }
    public void unloadOldRooms()
    { //Called once Player enters new dungeon
        //Pop Old Rooms and Hallways from according queues
    }

    public void loadedRoom(int index)
    {
        this.roomLoadedList[index] = true;
    }

    public void loadedHallway(int index)
    {
        this.hallwayLoadedList[index] = true;
    }
}