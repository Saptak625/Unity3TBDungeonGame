using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLoader
{
    Queue<List<Room>> roomQueue;
    Queue<List<Hallway>> hallwayQueue;

    public RoomLoader()
    {
        //Initialize all rooms and rooms from the beginning 
        List<Room> initialRoomList = new List<Room>();
        List<Hallway> initialHallwayList = new List<Hallway>();

    }

    public void loadNewRooms(int enterDirection)
    { //Called once Player leaves last dungeon
        //Push New Hallways and Rooms into according queues
    }
    public void unloadOldRooms()
    { //Called once Player enters new dungeon
        //Pop Old Rooms and Hallways from according queues
    }


}
