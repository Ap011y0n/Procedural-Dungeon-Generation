using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum  ROOM_TYPE
{
    NO_ROOM = 0,
    LOBBY,
    HALL,
    DOOR,
    MAX_ROOMS,
}

enum ORIENTATION
{
    NO_ORIENTATION,
    WEST,
    EAST,
    NORTH,
    SOUTH,
}

public class DungeonGenerator : MonoBehaviour
{

    // -- GameObject array that will store all prefabs to create rooms
    public GameObject[] Rooms = new GameObject[3];

    // -- Internal offset value given by prefabs width
    int spawnOffset;

    // -- Temporal bidimensional array to create random matrix
    public int[,] Dungeon = new int[10, 10];


    void Awake()
    { 
        // -- Temporal spawnOffset, we know exact value right now
        spawnOffset = 3;

        // -- Fills matrix with random values between 0 and 3 to spawn different room prefabs later
        for(int i = 0; i < 10; i++)
            for(int j = 0; j < 10; j++)
                Dungeon[i, j] = Random.Range(0,4);  
    }

    // Start is called before the first frame update
    void Start()
    {

        // -- Reads the matrix and calls instantiate function depending value in each cell
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
                InstantiateRoom(i, j, (ROOM_TYPE)Dungeon[i, j]);
    }

    void InstantiateRoom(int coordX, int coordY, ROOM_TYPE type)
    {
        // -- If room type non equals no room will create a room prefab
        if (type != ROOM_TYPE.NO_ROOM)
            Instantiate(Rooms[(int)type - 1], new Vector3(coordX * spawnOffset, 0, coordY * spawnOffset), Quaternion.identity);
    }
}
