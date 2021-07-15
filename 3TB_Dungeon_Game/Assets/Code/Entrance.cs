using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance
{
    bool doorState; //Controls whether door is open or close
    public int[] entranceRect; //Defines where entrance is

    public Entrance(Room r, Direction d, bool state)
    {
        //Initialize Entrances
        this.doorState = state;
        int[] roomRect = r.roomRect;
        int[] center = new int[2] { roomRect[0] + (roomRect[2] / 2), roomRect[1] + (roomRect[3] / 2) };
        if(d == Direction.Up || d == Direction.Down)
        {
            int[] entranceCenter = new int[2] { center[0], (d == Direction.Up ? roomRect[3] / 2 : -roomRect[3] / 2) };
            this.entranceRect = new int[4] { entranceCenter[0] - 2, entranceCenter[1], 5, 1 };
        }
        else
        {
            int[] entranceCenter = new int[2] { (d == Direction.Right ? roomRect[2] / 2 : -roomRect[2] / 2), center[1] };
            this.entranceRect = new int[4] { entranceCenter[0], entranceCenter[1] - 2, 1, 5 };
        }
    }

    public void toggle()
    { //Called when door needs to opened or closed
        //Change sprites to indicate open rather than closed and activate Box Collider or vice versa
    }

}
