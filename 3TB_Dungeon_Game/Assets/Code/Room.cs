using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room 
{
    bool isChestRoom; //Controls whether room is a chest room
    bool active; //Controls whether Enemies become Hostile
    List<Enemy> activeEnemies; //These enemies can still attack
    List<Enemy> slainEnemies; //These enemies corpses are shown
    List<Entrances> outEntrances; //Entrances that can open to lead out to other rooms
    List<Entrances> inEntrances; //Entrances that can open to lead into room
    int[] roomRect; //Defines where room is

    public Room(Hallway hallway)
    {
        //Initialize Room
    }

    public void replaceSlainEnemy(Enemy enemy)
    { //Called by Enemy once dead
        //Removes enemy from activeEnemies and adds it to slainEnemies
    }

    public void startBattle() 
    { 
        //Lock dungeon entrances and start battle
    }
}
