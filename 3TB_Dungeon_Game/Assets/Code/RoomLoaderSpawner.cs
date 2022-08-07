//Spawner behavior to convert virtual Room Loader state into Unity state

using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum TileType
{
    Floor,
    Wall
}

public class RoomLoaderSpawner : MonoBehaviour
{
    RoomLoader roomLoader; //Driver virtual state instance

    //Tile types
    public GameObject dungeonFloorTile;
    public GameObject dungeonWallTile;
    public GameObject dungeonEntranceTileOpen;
    public GameObject dungeonEntranceTileClosed;
    public GameObject roomTrigger;
    public GameObject dungeonDestroyableObstacle;

    //Game Objects
    public GameObject player;
    public GameObject chest;

    //Enemy Movement Containers
    public GameObject enemyMeleeContainer;
    public GameObject enemyRangeContainer;
    public GameObject enemyMageContainer;

    //Room Cleared Canvas
    public GameObject canvas;
    public GameObject RoomClearedPrefab;

    //States
    private bool loaded = false;

    public List<GameObject> instantiateGrid(GameObject tileType, int posX, int posY, int iterX, int iterY, int spacing, TileType t)
    {
        List<GameObject> gameObjects = new List<GameObject>();
        for (int i = 0; i < iterX; i++)
        {
            for (int j = 0; j < iterY; j++)
            {
                GameObject gridObject = Instantiate(tileType, new Vector3(posX + (spacing * i), posY + (spacing * j), ((posY + (spacing * j)) * 0.00001f) + (t == TileType.Floor ? 0 : -20)), Quaternion.identity);
                gameObjects.Add(gridObject);
            }
        }
        return gameObjects;
    }

    public void instantiateRoom(Room r)
    {
        //Create floor
        List<GameObject> floorGrid = this.instantiateGrid(this.dungeonFloorTile, r.roomRect[0] + 1, r.roomRect[1] + 1, r.roomRect[2] - 2, r.roomRect[3] - 2, 1, TileType.Floor);
        //Create walls
        List<GameObject> wallGrid = this.instantiateGrid(this.dungeonWallTile, r.roomRect[0], r.roomRect[1], 1, r.roomRect[3], 1, TileType.Wall);
        wallGrid.AddRange(this.instantiateGrid(this.dungeonWallTile, r.roomRect[0] + r.roomRect[2] - 1, r.roomRect[1], 1, r.roomRect[3], 1, TileType.Wall));
        wallGrid.AddRange(this.instantiateGrid(this.dungeonWallTile, r.roomRect[0] + 1, r.roomRect[1], r.roomRect[2] - 2, 1, 1, TileType.Wall));
        wallGrid.AddRange(this.instantiateGrid(this.dungeonWallTile, r.roomRect[0] + 1, r.roomRect[1] + r.roomRect[3] - 1, r.roomRect[2] - 2, 1, 1, TileType.Wall));

        //Create Entrances with according tile and add boxColliders for entrances if needed
        List<GameObject> entranceGrid = new List<GameObject>();
        foreach (Entrance e in r.inEntrances)
        {
            e.gameObjects = this.instantiateGrid((e.doorClosed ? this.dungeonEntranceTileClosed : this.dungeonEntranceTileOpen), e.entranceRect[0], e.entranceRect[1], e.entranceRect[2], e.entranceRect[3], 1, (e.doorClosed ? TileType.Wall : TileType.Floor));
            entranceGrid.AddRange(e.gameObjects);
        }
        foreach (Entrance e in r.outEntrances)
        {
            e.gameObjects = this.instantiateGrid((e.doorClosed ? this.dungeonEntranceTileClosed : this.dungeonEntranceTileOpen), e.entranceRect[0], e.entranceRect[1], e.entranceRect[2], e.entranceRect[3], 1, (e.doorClosed ? TileType.Wall : TileType.Floor));
            entranceGrid.AddRange(e.gameObjects);
        }

        //Remove Walls in Entrance Space
        List<GameObject> updatedWallGrid = new List<GameObject>();
        foreach (GameObject w in wallGrid)
        {

            bool positionUsed = false;
            foreach (GameObject e in entranceGrid)
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
            else
            {
                Destroy(w);
            }
        }
        wallGrid = updatedWallGrid;

        //Create Obstacles
        List<Obstacle> newObstacles = new List<Obstacle>();
        foreach(Obstacle o in r.obstacles)
        {
            if(o.type == ObstacleType.Destructable)
            {
                o.gameObjects = this.instantiateGrid(this.dungeonDestroyableObstacle, o.obstacleRect[0], o.obstacleRect[1], o.obstacleRect[2], o.obstacleRect[3], 1, TileType.Wall);
                foreach(GameObject g in o.gameObjects)
                {
                    g.transform.parent = this.gameObject.transform;
                }
                newObstacles.Add(o);
            }
            else
            {
                wallGrid.AddRange(this.instantiateGrid(this.dungeonWallTile, o.obstacleRect[0], o.obstacleRect[1], o.obstacleRect[2], o.obstacleRect[3], 1, TileType.Wall));
            }
        }
        r.obstacles = newObstacles;

        //Add Room Trigger if room is a subroom
        if (r.roomDirection != Direction.None)
        {
            //Get room's inEntrance
            Entrance inEntrance = r.inEntrances[0];
            if (inEntrance.direction == Direction.Up || inEntrance.direction == Direction.Down)
            {
                int[] translationCenter = new int[2] { inEntrance.entranceRect[0] + 2, inEntrance.entranceRect[1] + (inEntrance.direction == Direction.Up ? -2 : 2) };
                r.trigger = Instantiate(this.roomTrigger, new Vector3(translationCenter[0], translationCenter[1], 200), Quaternion.identity);
            }
            else
            {
                int[] translationCenter = new int[2] { inEntrance.entranceRect[0] + (inEntrance.direction == Direction.Right ? -2 : 2), inEntrance.entranceRect[1] + 2 };
                r.trigger = Instantiate(this.roomTrigger, new Vector3(translationCenter[0], translationCenter[1], 200), Quaternion.identity);
                r.trigger.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            r.trigger.transform.parent = this.gameObject.transform;
        }

        //Check if chest room 
        if (r.isChestRoom)
        {
            int centerX=r.roomRect[0] + (r.roomRect[2]/2);
            int centerY=r.roomRect[1] + (r.roomRect[3]/2);
            floorGrid.Add(Instantiate(chest, new Vector3(centerX, centerY, (centerY * 0.00001f)-20), Quaternion.identity));
        }

        //Combine all gameObjects and store
        floorGrid.AddRange(wallGrid);
        floorGrid.AddRange(entranceGrid);
        r.gameObjects = floorGrid;
    }

    public void instantiateHallway(Hallway h)
    {
        List<GameObject> floorGrid;
        List<GameObject> wallGrid;
        if (h.direction == Direction.Up || h.direction == Direction.Down)
        {
            //Create floor
            floorGrid = this.instantiateGrid(this.dungeonFloorTile, h.hallwayRect[0] + 1, h.hallwayRect[1], h.hallwayRect[2] - 1, h.hallwayRect[3], 1, TileType.Floor);
            //Create walls
            wallGrid = this.instantiateGrid(this.dungeonWallTile, h.hallwayRect[0], h.hallwayRect[1], 1, h.hallwayRect[3], 1, TileType.Wall);
            wallGrid.AddRange(this.instantiateGrid(this.dungeonWallTile, h.hallwayRect[0] + h.hallwayRect[2], h.hallwayRect[1], 1, h.hallwayRect[3], 1, TileType.Wall));
        }
        else
        {
            //Create floor
            floorGrid = this.instantiateGrid(this.dungeonFloorTile, h.hallwayRect[0], h.hallwayRect[1] + 1, h.hallwayRect[2], h.hallwayRect[3] - 1, 1, TileType.Floor);
            //Create walls
            wallGrid = this.instantiateGrid(this.dungeonWallTile, h.hallwayRect[0], h.hallwayRect[1], h.hallwayRect[2], 1, 1, TileType.Wall);
            wallGrid.AddRange(this.instantiateGrid(this.dungeonWallTile, h.hallwayRect[0], h.hallwayRect[1] + h.hallwayRect[3], h.hallwayRect[2], 1, 1, TileType.Wall));
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
        if (this.roomLoader.activeRoom == null)
        {
            //No Battle is occuring. Just load rooms.
            if (!this.loaded)
            {
                this.reloadRoomsAndHallwayState();
            }
        }
        else
        {
            //Battle occuring. Only focus on battle-related updates such as enemy movements.
            if(this.roomLoader.activeRoom.activeEnemies.Count > 0)
            {
                if(this.roomLoader.activeRoom.activeEnemies[0].Count == 0)
                {
                    this.roomLoader.activeRoom.activeEnemies.RemoveAt(0);
                    if(this.roomLoader.activeRoom.activeEnemies.Count > 0)
                    {
                        this.spawnNextEnemyWave();
                    }
                    else
                    {
                        this.dungeonCleared();
                    }
                }
            }
        }
    }

    public void reloadRoomsAndHallwayState()
    {
        //Load all Rooms
        for (int i = 0; i < this.roomLoader.roomLoadedList.Count; i++)
        {
            if (!this.roomLoader.roomLoadedList[i])
            {
                //Corresponding Room List needs to be loaded.
                foreach (Room r in this.roomLoader.roomQueue[i])
                {
                    if (r.gameObjects == null)
                    {
                        this.instantiateRoom(r);
                    }
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
                foreach (List<Hallway> l in this.roomLoader.hallwayQueue[i])
                {
                    foreach (Hallway h in l)
                    {
                        if (h.gameObjects == null)
                        {
                            this.instantiateHallway(h);
                        }
                    }
                }
                //Set load state to true
                this.roomLoader.loadedHallway(i);
            }
        }
        //Unload all Rooms and Hallways
        foreach (Room r in this.roomLoader.unloadRoomQueue)
        {
            r.destroy();
        }
        foreach (Hallway h in this.roomLoader.unloadHallwayQueue)
        {
            h.destroy();
        }
        this.roomLoader.unloadRoomQueue.Clear();
        this.roomLoader.unloadHallwayQueue.Clear();

        //Set loaded to True to finish update
        this.loaded = true;
    }

    public void enteredDungeon(GameObject triggerObject)
    {
        //Getting room that was selected
        Room selectedRoom = null;
        foreach (Room r in this.roomLoader.roomQueue[0])
        {
            if (r.trigger == triggerObject)
            {
                selectedRoom = r;
                break;
            }
        }
        Debug.Log(selectedRoom);
        //Update Room Stats
        Room.roomStatsIncrement(selectedRoom.isChestRoom, selectedRoom.isBossRoom);

        //Close entrance behind player
        this.toggleEntrance(selectedRoom.inEntrances[0]);

        //Destroy Trigger Objects
        for (int i = 1; i < this.roomLoader.roomQueue[0].Count; i++)
        {
            Destroy(this.roomLoader.roomQueue[0][i].trigger);
        }

        //Pause Game and show UI Screens
        List<int> enemyTypes = new List<int>() { (int) selectedRoom.enemyTypeArray[0], (int)selectedRoom.enemyTypeArray[1] };
        player.GetComponent<PlayerController>().isPaused = true;
        Time.timeScale = 0f;
        GameObject enemyInfoScreen = canvas.transform.GetChild(2).gameObject;
        enemyInfoScreen.SetActive(true);
        enemyInfoScreen.GetComponent<InfoScreen>().setupScreen(selectedRoom, player, gameObject, enemyTypes);
    }

    public void startDungeon(Room selectedRoom)
    {
        //Set activeRoom
        this.roomLoader.activeRoom = selectedRoom;

        //Set AStar grid to room coordinates and scan if enemy room
        if (!this.roomLoader.activeRoom.isChestRoom)
        {
            this.reloadAStarGrid();
        }

        //Spawn in Enemies and set triggers to open room once ready
        if (!this.roomLoader.activeRoom.isChestRoom && !this.roomLoader.activeRoom.isBossRoom)
        {
            Debug.Log(this.roomLoader.activeRoom);
            Debug.Log(this.roomLoader.activeRoom.activeEnemies.Count);
            this.spawnNextEnemyWave();

        }
        else if (this.roomLoader.activeRoom.isBossRoom)
        {
            //Initialize Boss
            //Uncomment bottom line for testing
            Invoke("dungeonCleared", 2.0f);
        }
        else if (this.roomLoader.activeRoom.isChestRoom)
        {
            player.GetComponent<PlayerController>().ableToOpenChests = true;
            Invoke("dungeonCleared", 2.0f);
        }

        //-------------------------------------------------Uncomment this line to open dungeon after 5 seconds-----------------------------------------------------------
        //Invoke("dungeonCleared", 5.0f);
    }

    public void spawnNextEnemyWave()
    {
        GameObject player = GameObject.FindWithTag("Player");
        foreach (Enemy enemy in this.roomLoader.activeRoom.activeEnemies[0])
        {
            //Use resource loader in real code
            GameObject enemyPrefab = Resources.Load($"Enemy Prefabs/{enemy.attackType}_{(int)enemy.enemyType}") as GameObject;
            GameObject enemyGameObject;
            if (enemy.attackType == EnemyAttack.Melee)
            {
                enemyGameObject = Instantiate(this.enemyMeleeContainer, enemy.position, Quaternion.identity);
                //Do Melee Container specific init
                enemyGameObject.GetComponent<AIDestinationSetter>().target = player.transform;
                enemyGameObject.GetComponent<AIPath>().maxSpeed = enemy.speed;
            }
            else if ((enemy.attackType == EnemyAttack.Range && !(enemy.enemyType == (EnemyType)4)) || (enemy.attackType == EnemyAttack.Mage && enemy.enemyType == (EnemyType)4))
            {
                enemyGameObject = Instantiate(this.enemyRangeContainer, enemy.position, Quaternion.identity);
                //Do Range Container specific init
                GenericRangeAI pathfindingTarget = enemyGameObject.GetComponent<GenericRangeAI>();
                pathfindingTarget.target = player.transform;
                pathfindingTarget.speed = enemy.speed * 80; // Scaling factor due to custom AI
            }
            else
            {
                enemyGameObject = Instantiate(this.enemyMageContainer, enemy.position, Quaternion.identity);
                //Do Mage Container specific init
                enemyGameObject.GetComponent<MageAI>().roomRect = this.roomLoader.activeRoom.roomRect;
                enemyGameObject.GetComponent<AIPath>().maxSpeed = enemy.speed;
            }

            enemyGameObject.transform.parent = this.gameObject.transform;
            GameObject enemyGraphics = Instantiate(enemyPrefab, enemyGameObject.transform);
            Animator animator = enemyGraphics.GetComponent<Animator>();

            EnemyController controller = enemyGraphics.GetComponent<EnemyController>();
            controller.enemy = enemy;
            controller.player = player;
            controller.animator = animator;
            controller.roomLoaderObject = gameObject;

            EnemyMovementController em = enemyGameObject.GetComponent<EnemyMovementController>();
            em.animator = animator;
            em.t = enemyGraphics.transform;
            em.player = player;
        }
    }

    public void spawnMiniMelee(Enemy parent, Vector3 parentPosition)
    {
        GameObject player = GameObject.FindWithTag("Player");
        Enemy enemy = new Enemy(EnemyAttack.Mini, parent.enemyType, parentPosition);
        //Use resource loader in real code
        GameObject enemyPrefab = Resources.Load($"Enemy Prefabs/{enemy.attackType}_{(int)enemy.enemyType}") as GameObject;
        GameObject enemyGameObject = Instantiate(this.enemyMeleeContainer, enemy.position, Quaternion.identity);
        //Do Melee Container specific init
        enemyGameObject.GetComponent<AIDestinationSetter>().target = player.transform;
        enemyGameObject.GetComponent<AIPath>().maxSpeed = enemy.speed;
        enemyGameObject.GetComponent<Rigidbody2D>().mass = 0.25f;
        enemyGameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.25f, 0.25f);

        enemyGameObject.transform.parent = this.gameObject.transform;
        GameObject enemyGraphics = Instantiate(enemyPrefab, enemyGameObject.transform);
        Animator animator = enemyGraphics.GetComponent<Animator>();

        EnemyController controller = enemyGraphics.GetComponent<EnemyController>();
        controller.enemy = enemy;
        controller.player = player;
        controller.animator = animator;
        controller.roomLoaderObject = gameObject;

        EnemyMovementController em = enemyGameObject.GetComponent<EnemyMovementController>();
        em.animator = animator;
        em.t = enemyGraphics.transform;
        em.player = player;

        this.roomLoader.activeRoom.activeEnemies[0].Add(enemy);
    }

    public void removeEnemy(Enemy e)
    {
        this.roomLoader.activeRoom.activeEnemies[0].Remove(e);
        this.roomLoader.activeRoom.slainEnemies.Add(e);
    }

    public void dungeonCleared()
    {
        //Show UI Screen
        GameObject clearedInstance = Instantiate(RoomClearedPrefab);
        clearedInstance.transform.SetParent(canvas.transform, false);

        //Reset Enemy Position System
        Enemy.resetPositionsUsed();

        //Load and Unload new and old rooms
        this.roomLoader.loadAndUnloadRoomsAndHallways();

        //Open out entrances
        foreach (Entrance e in this.roomLoader.activeRoom.outEntrances)
        {
            this.toggleEntrance(e);
        }

        //Reset active room
        this.roomLoader.activeRoom = null;

        //Set loaded to false to load everything in next update
        this.loaded = false;

        //Increment Rooms Cleared
        player.GetComponent<PlayerController>().roomsCleared += 1;
    }

    public void toggleEntrance(Entrance e)
    {
        e.doorClosed = !e.doorClosed;
        foreach (GameObject g in e.gameObjects)
        {
            Destroy(g);
        }
        e.gameObjects = this.instantiateGrid((e.doorClosed ? this.dungeonEntranceTileClosed : this.dungeonEntranceTileOpen), e.entranceRect[0], e.entranceRect[1], e.entranceRect[2], e.entranceRect[3], 1, (e.doorClosed ? TileType.Wall : TileType.Floor));
    }

    public void reloadAStarGrid()
    {
        GridGraph graphToScan = AstarPath.active.data.gridGraph;
        graphToScan.center = new Vector3(this.roomLoader.activeRoom.roomRect[0] + (this.roomLoader.activeRoom.roomRect[2] / 2), this.roomLoader.activeRoom.roomRect[1] + (this.roomLoader.activeRoom.roomRect[3] / 2), 0);
        graphToScan.SetDimensions((this.roomLoader.activeRoom.roomRect[2] / 2) * 4, (this.roomLoader.activeRoom.roomRect[3] / 2) * 4, 0.5f);
        AstarPath.active.Scan(graphToScan);
    }
}