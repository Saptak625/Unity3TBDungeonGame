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
    List<Sprite> staticSprites;
    int[] roomRect; //Defines where room is
    public SpriteRenderer floorRenderer;

    public Room()
    {
        //Initialize Starter Room
        System.Random random = new System.Random();
        int xPos = random.Next(8, 12);
        int yPos = random.Next(8, 12);
        roomRect = new int[4] {xPos, yPos, xPos*2, yPos*2};
        //Create Sprite for floor 
        floorRenderer = RoomLoaderController.loaderObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        Sprite floor = Sprite.Create(RoomLoaderController.tex, new Rect(0f, 0f, roomRect[2], roomRect[3]), new Vector2(0.5f, 0.5f), 100f);
        floorRenderer.sprite = floor;
        staticSprites.Add(floor);
    }

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
