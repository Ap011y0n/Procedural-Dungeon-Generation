using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Direction
{

    NONE,
    NORTH,
    SOUTH,
    WEST,
    EAST,

};

public class ProceduralDungeon : MonoBehaviour
{
    public int WIDTH = 30;
    public int HEIGHT = 30;

    public struct point
    {
        public int x;
        public int y;
    };

   public struct Door
    {
        public Direction direction;
        public bool used;
    };
   public struct Room
    {
        public List<Door> doors;
        public int x;
        public int y;
        public int size;
    };
   public struct Corridor
    {
        public Direction direction;
        public int posx;
        public int posy;

    };

    public GameObject Room3x3_1D;
    public GameObject Room5x5_1D;
    public GameObject[] Room3x3_2D;
    public GameObject[] Room5x5_2D;
    public GameObject Room3x3_3D;
    public GameObject Room5x5_3D;
    public GameObject Room3x3_4D;
    public GameObject Room5x5_4D;
    public GameObject CorridorPrefab;
    public List<Room> Rooms = new List<Room>();
    public List<Corridor> Corridors = new List<Corridor>();

    // Start is called before the first frame update
    void Start()
    {

        int[,] matrix = new int[HEIGHT + 1, WIDTH + 1];
    
       

        for (int i = 0; i < HEIGHT + 1; i++)
            for (int j = 0; j < WIDTH + 1; j++)
                matrix[i, j] = 0;
        Room FirstRoom;
        FirstRoom.doors = new List<Door>();
        FirstRoom.x = (HEIGHT + 1) / 2;
        FirstRoom.y = (WIDTH + 1) / 2;
        FirstRoom.size = 3;

        Door door;
        door.used = false;
        door.direction = Direction.NORTH;
        FirstRoom.doors.Add(door);

        Door door2;
        door2.used = false;
        door2.direction = Direction.WEST;
        FirstRoom.doors.Add(door2);
        Door door3;
        door3.used = false;
        door3.direction = Direction.SOUTH;
        FirstRoom.doors.Add(door3);
        Door door4;
        door4.used = false;
        door4.direction = Direction.EAST;
        FirstRoom.doors.Add(door4);

        //APUNTE al hacerlo en este orden las puertas no se borran
        if (FillRoom(matrix, FirstRoom))
        {
            CreateCorridor(matrix, FirstRoom);
            FillDoors(matrix, FirstRoom);
        }
        Rooms.Add(FirstRoom);
        Print(matrix);
        InstantiateMap();
    }

    bool CreateCorridor(int[,] matrix, Room room)
    {

        bool ret = false;


        for (int i = 0; i < room.doors.Count; i++)
        {
            if (room.doors[i].used)
                continue;

            int posx = room.x;
            int posy = room.y;
            switch (room.doors[i].direction)
            {
                case Direction.NONE:
                    break;
                case Direction.NORTH:
                    posx -= room.size / 2;
                    break;
                case Direction.SOUTH:
                    posx += room.size / 2;
                    break;
                case Direction.WEST:
                    posy -= room.size / 2;
                    break;
                case Direction.EAST:
                    posy += room.size / 2;
                    break;
            }

            int length = Random.Range(2, 7);
            //cout << length;
            bool corridor_completed = true;
            for (int f = 0; f < length; f++)
            {
                Corridor corridor;
                corridor.direction = room.doors[i].direction;
                corridor.posx = 0;
                corridor.posy = 0;

                switch (corridor.direction)
                {
                    case Direction.NONE:
                        break;
                    case Direction.NORTH:
                        corridor.posx = posx - 1;
                        corridor.posy = posy;
                        if (corridor.posx < HEIGHT + 1 && corridor.posx >= 0 && corridor.posy < WIDTH + 1 && corridor.posy >= 0 && matrix[corridor.posx, corridor.posy] == 0)
                        {
                            matrix[corridor.posx, corridor.posy] = 2;
                        }
                        else
                        {
                            length = f;
                            corridor_completed = false;
                        }
                        break;
                    case Direction.SOUTH:
                        corridor.posx = posx + 1;
                        corridor.posy = posy;
                        if (corridor.posx < HEIGHT + 1 && corridor.posx >= 0 && corridor.posy < WIDTH + 1 && corridor.posy >= 0 && matrix[corridor.posx, corridor.posy] == 0)
                        {
                            matrix[corridor.posx, corridor.posy] = 2;
                        }
                        else
                        {
                            length = f;
                            corridor_completed = false;
                        }
                        break;
                    case Direction.WEST:
                        corridor.posx = posx;
                        corridor.posy = posy - 1;
                        if (corridor.posx < HEIGHT + 1 && corridor.posx >= 0 && corridor.posy < WIDTH + 1 && corridor.posy >= 0 && matrix[corridor.posx, corridor.posy] == 0)
                        {

                            matrix[corridor.posx, corridor.posy] = 2;
                        }
                        else
                        {
                            length = f;
                            corridor_completed = false;
                        }
                        break;
                    case Direction.EAST:
                        corridor.posx = posx;
                        corridor.posy = posy + 1;
                        if (corridor.posx < HEIGHT + 1 && corridor.posx >= 0 && corridor.posy < WIDTH + 1 && corridor.posy >= 0 && matrix[corridor.posx, corridor.posy] == 0)
                        {
                            matrix[corridor.posx, corridor.posy] = 2;
                        }
                        else
                        {
                            length = f;
                            corridor_completed = false;
                        }
                        break;
                }
                posx = corridor.posx;
                posy = corridor.posy;
                Corridors.Add(corridor);
            }

            if (!corridor_completed)
            {
                RemoveCorridor(matrix, room, length, room.doors[i].direction);
                room.doors.RemoveAt(i);
                i--;
            }
            else
            {
                int size = 3 + Random.Range(0, 2) * 2;
                //int size = 3;
                switch (room.doors[i].direction)
                {
                    case Direction.NONE:
                        break;
                    case Direction.NORTH:
                        posx -= size / 2 + 1;
                        break;
                    case Direction.SOUTH:
                        posx += size / 2 + 1;
                        break;
                    case Direction.WEST:
                        posy -= size / 2 + 1;
                        break;
                    case Direction.EAST:
                        posy += size / 2 + 1;
                        break;
                }
                if (!CreateRoom(posx, posy, room.doors[i].direction, matrix, size))
                {
                    RemoveCorridor(matrix, room, length, room.doors[i].direction);
                    room.doors.RemoveAt(i);
                    i--;
                }

            }
        }
        ret = true;
        return ret;
    }

    bool CreateRoom(int posx, int posy, Direction direction, int[,] matrix, int size)
    {
        bool ret = true;
        Room newRoom;
        newRoom.doors = new List<Door>();
        newRoom.x = posx;
        newRoom.y = posy;
        newRoom.size = size;
        Door door;
        door.used = true;
        door.direction = Direction.NONE;

        switch (direction)
        {
            case Direction.NONE:
                break;
            case Direction.NORTH:
                door.direction = Direction.SOUTH;
                break;
            case Direction.SOUTH:
                door.direction = Direction.NORTH;
                break;
            case Direction.WEST:
                door.direction = Direction.EAST;
                break;
            case Direction.EAST:
                door.direction = Direction.WEST;
                break;
        }
        newRoom.doors.Add(door);
     
        int newdoors =  Random.Range(1, 4);
        int type = Random.Range(1, 5);

        for (int i = 0; i < newdoors; i++)
        {
            Door door2;
            door2.used = false;
            door2.direction = door.direction;

            while (door2.direction == door.direction)
            {

                switch (type)
                {
                    case 1:
                        door2.direction = Direction.NORTH;
                        break;
                    case 2:
                        door2.direction = Direction.SOUTH;
                        break;
                    case 3:
                        door2.direction = Direction.WEST;
                        break;
                    case 4:
                        door2.direction = Direction.EAST;
                        break;
                }
                type++;
                if (type > 4)
                    type = 0;
            }
            newRoom.doors.Add(door2);
        }
        ret = FillRoom(matrix, newRoom);
        if (ret)
        {
            CreateCorridor(matrix, newRoom);
            FillDoors(matrix, newRoom);
            Rooms.Add(newRoom);
        }

        return ret;
    }

  

    void RemoveCorridor(int[,] matrix, Room room, int length, Direction direction)
    {
        int posx = room.x;
        int posy = room.y;

        switch (direction)
        {
            case Direction.NONE:
                break;
            case Direction.NORTH:
                posx -= room.size / 2;
                break;
            case Direction.SOUTH:
                posx += room.size / 2;
                break;
            case Direction.WEST:
                posy -= room.size / 2;
                break;
            case Direction.EAST:
                posy += room.size / 2;
                break;
        }

        for (int f = 0; f <= length; f++)
        {
            Corridor corridor;
            corridor.direction = direction;
            corridor.posx = 0;
            corridor.posy = 0;

            switch (corridor.direction)
            {
                case Direction.NONE:
                    break;
                case Direction.NORTH:
                    corridor.posx = posx - 1;
                    corridor.posy = posy;
                    if (corridor.posx < HEIGHT + 1 && corridor.posx >= 0 && corridor.posy < WIDTH + 1 && corridor.posy >= 0)
                    {
                        matrix[corridor.posx, corridor.posy] = 0;
                    }
                    break;
                case Direction.SOUTH:
                    corridor.posx = posx + 1;
                    corridor.posy = posy;
                    if (corridor.posx < HEIGHT + 1 && corridor.posx >= 0 && corridor.posy < WIDTH + 1 && corridor.posy >= 0)
                    {
                        matrix[corridor.posx, corridor.posy] = 0;
                    }
                    break;
                case Direction.WEST:
                    corridor.posx = posx;
                    corridor.posy = posy - 1;
                    if (corridor.posx < HEIGHT + 1 && corridor.posx >= 0 && corridor.posy < WIDTH + 1 && corridor.posy >= 0)
                    {
                        matrix[corridor.posx, corridor.posy] = 0;
                    }
                    break;
                case Direction.EAST:
                    corridor.posx = posx;
                    corridor.posy = posy + 1;
                    if (corridor.posx < HEIGHT + 1 && corridor.posx >= 0 && corridor.posy < WIDTH + 1 && corridor.posy >= 0)
                    {
                        matrix[corridor.posx, corridor.posy] = 0;
                    }
                    break;
            }
            posx = corridor.posx;
            posy = corridor.posy;
            Corridors.Remove(corridor);
        }
    }

    bool FillDoors(int[,] matrix, Room room)
    {
        bool ret = true;
        for (int i = 0; i < room.doors.Count; i++)
        {
            switch (room.doors[i].direction)
            {
                case Direction.NONE:
                    break;
                case Direction.NORTH:
                    matrix[room.x - room.size / 2, room.y] = 3;
                    break;
                case Direction.SOUTH:
                    matrix[room.x + room.size / 2, room.y] = 3;
                    break;
                case Direction.WEST:
                    matrix[room.x, room.y - room.size / 2] = 3;
                    break;
                case Direction.EAST:
                    matrix[room.x, room.y + room.size / 2] = 3;
                    break;
            }
        }
        return ret;
    }

    bool FillRoom(int[,] matrix, Room room)
    {
        bool room_completed = true;


        List<point> usedpositions = new List<point>();

        for (int i = -room.size / 2; i <= room.size / 2; i++)
        {
            for (int j = -room.size / 2; j <= room.size / 2; j++)
            {
                if (room.x + i < HEIGHT + 1 && room.x + i >= 0 && room.y + j < WIDTH + 1 && room.y + j >= 0 && matrix[room.x + i, room.y + j] == 0)
                {
                    matrix[room.x + i, room.y + j] = 1;
                    point temp;
                    temp.x = room.x + i;
                    temp.y = room.y + j;
                    usedpositions.Add(temp);
                }
                else
                {
                    room_completed = false;
                    break;
                }
            }
            if (room_completed == false)
                break;
        }

        if (!room_completed)
        {
            for (int i = 0; i < usedpositions.Count; i++)
            {
                if (usedpositions[i].x < HEIGHT + 1 && usedpositions[i].x >= 0 && usedpositions[i].y < WIDTH + 1 && usedpositions[i].y >= 0)
                    matrix[usedpositions[i].x, usedpositions[i].y] = 0;
            }
        }

        return room_completed;
    }

    void Print(int[,] matrix)
    {

        int rowLength = matrix.GetLength(0);
        int colLength = matrix.GetLength(1);

        for (int i = 0; i < rowLength; i++)
        {
            string arrayString = "";

            for (int j = 0; j < colLength; j++)
            {
                if(matrix[i, j] == 0)
                    arrayString += string.Format("  ");
                else
                arrayString += string.Format("{0} ", matrix[i, j]);
            }

            Debug.Log(arrayString); 
        }


    }

    void InstantiateMap()
    {

        float offsetx = transform.position.x - HEIGHT / 2, offsety = transform.position.z - WIDTH / 2, height = transform.position.y;
        GameObject newroom;
        for (int i = 0; i < Rooms.Count; i++)
        {
            if(Rooms[i].size > 3)
            {
                switch (Rooms[i].doors.Count)
                {
                    case 1:
                        newroom = Instantiate(Room5x5_1D, new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                        switch (Rooms[i].doors[0].direction)
                        {
                            case Direction.NONE:
                                break;
                            case Direction.SOUTH:
                                newroom.transform.Rotate(-90, 90, 0);
                                break;
                            case Direction.NORTH:
                                newroom.transform.Rotate(-90, -90, 0);
                                break;
                            case Direction.EAST:
                                newroom.transform.Rotate(-90, 0, 0);
                                break;
                            case Direction.WEST:
                                newroom.transform.Rotate(-90, 180, 0);
                                break;
                        }
                        break;
                    case 2:
                        switch (Rooms[i].doors[0].direction)
                        {
                            case Direction.NONE:
                                break;
                            case Direction.SOUTH:
                                switch (Rooms[i].doors[1].direction)
                                {
                                    case Direction.NONE:
                                        break;
                                    case Direction.NORTH:
                                        newroom = Instantiate(Room5x5_2D[0], new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                                        newroom.transform.Rotate(-90, 90, 0);
                                        break;
                                    case Direction.EAST:
                                        newroom = Instantiate(Room5x5_2D[1], new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                                        newroom.transform.Rotate(-90, -90, 0);
                                        break;
                                    case Direction.WEST:
                                        newroom = Instantiate(Room5x5_2D[1], new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                                        newroom.transform.Rotate(-90, 0, 0);
                                        break;
                                }
                                break;
                            case Direction.NORTH:
                                switch (Rooms[i].doors[1].direction)
                                {
                                    case Direction.NONE:
                                        break;
                                    case Direction.SOUTH:
                                        newroom = Instantiate(Room5x5_2D[0], new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                                        newroom.transform.Rotate(-90, 90, 0);
                                        break;
                                    case Direction.EAST:
                                        newroom = Instantiate(Room5x5_2D[1], new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                                        newroom.transform.Rotate(-90, 180, 0);
                                        break;
                                    case Direction.WEST:
                                        newroom = Instantiate(Room5x5_2D[1], new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                                        newroom.transform.Rotate(-90, 90, 0);
                                        break;
                                }
                                break;
                            case Direction.EAST:
                                switch (Rooms[i].doors[1].direction)
                                {
                                    case Direction.NONE:
                                        break;
                                    case Direction.SOUTH:
                                        newroom = Instantiate(Room5x5_2D[1], new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                                        newroom.transform.Rotate(-90, -90, 0);
                                        break;
                                    case Direction.NORTH:
                                        newroom = Instantiate(Room5x5_2D[1], new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                                        newroom.transform.Rotate(-90, 180, 0);
                                        break;
                                    case Direction.WEST:
                                        newroom = Instantiate(Room5x5_2D[0], new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                                        newroom.transform.Rotate(-90, 0, 0);
                                        break;
                                }
                                break;
                            case Direction.WEST:
                                switch (Rooms[i].doors[1].direction)
                                {
                                    case Direction.NONE:
                                        break;
                                    case Direction.SOUTH:
                                        newroom = Instantiate(Room5x5_2D[1], new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                                        newroom.transform.Rotate(-90, 0, 0);
                                        break;
                                    case Direction.NORTH:
                                        newroom = Instantiate(Room5x5_2D[1], new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                                        newroom.transform.Rotate(-90, 90, 0);
                                        break;
                                    case Direction.EAST:
                                        newroom = Instantiate(Room5x5_2D[0], new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                                        newroom.transform.Rotate(-90, 0, 0);
                                        break;

                                }
                                break;
                        }

                        break;
                    case 3:
                        newroom = Instantiate(Room5x5_3D, new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                        int n = Rooms[i].doors.Count;
                        int direction = (n + 1) * (n + 2) / 2;

                        for (int k = 0; k < n; k++)
                            direction -= (int)Rooms[i].doors[k].direction;

                        Direction tempDirection = (Direction)direction;

                        switch (tempDirection)
                        {
                            case Direction.NONE:
                                break;
                            case Direction.SOUTH:
                                newroom.transform.Rotate(-90, 90, 0);
                                break;
                            case Direction.NORTH:
                                newroom.transform.Rotate(-90, -90, 0);
                                break;
                            case Direction.EAST:
                                newroom.transform.Rotate(-90, 0, 0);
                                break;
                            case Direction.WEST:
                                newroom.transform.Rotate(-90, 180, 0);
                                break;
                        }


                        break;
                    case 4:
                        newroom = Instantiate(Room5x5_4D, new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                        newroom.transform.Rotate(-90, 0, 0);

                        break;
                }

            }
            else
            {
                switch(Rooms[i].doors.Count)
                {
                    case 1:
                       newroom = Instantiate(Room3x3_1D, new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                        switch(Rooms[i].doors[0].direction)
                        {
                            case Direction.NONE:
                                break;
                            case Direction.SOUTH:
                                newroom.transform.Rotate(-90, 90, 0);
                                break;
                            case Direction.NORTH:
                                newroom.transform.Rotate(-90, -90, 0);
                                break;
                            case Direction.EAST:
                                newroom.transform.Rotate(-90, 0, 0);
                                break;
                            case Direction.WEST:
                                newroom.transform.Rotate(-90, 180, 0);
                                break;
                        }
                        break;
                    case 2:
                        switch (Rooms[i].doors[0].direction)
                        {
                            case Direction.NONE:
                                break;
                            case Direction.SOUTH:
                                switch (Rooms[i].doors[1].direction)
                                {
                                    case Direction.NONE:
                                        break;
                                    case Direction.NORTH:
                                        newroom = Instantiate(Room3x3_2D[0], new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                                        newroom.transform.Rotate(-90, 90, 0);
                                        break;
                                    case Direction.EAST:
                                        newroom = Instantiate(Room3x3_2D[1], new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                                        newroom.transform.Rotate(-90, -90, 0);
                                        break;
                                    case Direction.WEST:
                                        newroom = Instantiate(Room3x3_2D[1], new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                                        newroom.transform.Rotate(-90, 0, 0);
                                        break;
                                }
                                break;
                            case Direction.NORTH:
                                switch (Rooms[i].doors[1].direction)
                                {
                                    case Direction.NONE:
                                        break;
                                    case Direction.SOUTH:
                                        newroom = Instantiate(Room3x3_2D[0], new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                                        newroom.transform.Rotate(-90, 90, 0);
                                        break;
                                    case Direction.EAST:
                                        newroom = Instantiate(Room3x3_2D[1], new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                                        newroom.transform.Rotate(-90, 180, 0);
                                        break;
                                    case Direction.WEST:
                                        newroom = Instantiate(Room3x3_2D[1], new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                                        newroom.transform.Rotate(-90, 90, 0);
                                        break;
                                }
                                break;
                            case Direction.EAST:
                                switch (Rooms[i].doors[1].direction)
                                {
                                    case Direction.NONE:
                                        break;
                                    case Direction.SOUTH:
                                        newroom = Instantiate(Room3x3_2D[1], new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                                        newroom.transform.Rotate(-90, -90, 0);
                                        break;
                                    case Direction.NORTH:
                                        newroom = Instantiate(Room3x3_2D[1], new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                                        newroom.transform.Rotate(-90, 180, 0);
                                        break;
                                    case Direction.WEST:
                                        newroom = Instantiate(Room3x3_2D[0], new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                                        newroom.transform.Rotate(-90, 0, 0);
                                        break;
                                }
                                break;
                            case Direction.WEST:
                                switch (Rooms[i].doors[1].direction)
                                {
                                    case Direction.NONE:
                                        break;
                                    case Direction.SOUTH:
                                        newroom = Instantiate(Room3x3_2D[1], new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                                        newroom.transform.Rotate(-90, 0, 0);
                                        break;
                                    case Direction.NORTH:
                                        newroom = Instantiate(Room3x3_2D[1], new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                                        newroom.transform.Rotate(-90, 90, 0);
                                        break;
                                    case Direction.EAST:
                                        newroom = Instantiate(Room3x3_2D[0], new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                                        newroom.transform.Rotate(-90, 0, 0);
                                        break;

                                }
                                break;
                        }
                      
                        break;
                    case 3:
                        newroom = Instantiate(Room3x3_3D, new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                        int n = Rooms[i].doors.Count;
                        int direction = (n + 1) * (n + 2) / 2;

                        for (int k = 0; k < n; k++)
                            direction -= (int)Rooms[i].doors[k].direction;

                        Direction tempDirection = (Direction)direction;

                        switch (tempDirection)
                        {
                            case Direction.NONE:
                                break;
                            case Direction.SOUTH:
                                newroom.transform.Rotate(-90, 90, 0);
                                break;
                            case Direction.NORTH:
                                newroom.transform.Rotate(-90, -90, 0);
                                break;
                            case Direction.EAST:
                                newroom.transform.Rotate(-90, 0, 0);
                                break;
                            case Direction.WEST:
                                newroom.transform.Rotate(-90, 180, 0);
                                break;
                        }
                    
                       
                            break;
                    case 4:
                        newroom = Instantiate(Room3x3_4D, new Vector3(Rooms[i].x + offsetx, height, Rooms[i].y + offsety), Quaternion.identity);
                        newroom.transform.Rotate(-90, 0, 0);

                        break;
                }
            }

        }
        for (int i = 0; i < Corridors.Count; i++)
        {
            GameObject newcorridor = Instantiate(CorridorPrefab, new Vector3(Corridors[i].posx + offsetx, height, Corridors[i].posy + offsety), Quaternion.identity);
            if (Corridors[i].direction == Direction.EAST || Corridors[i].direction == Direction.WEST)
                newcorridor.transform.Rotate(-90, 0, 0);
            else
            {
                newcorridor.transform.Rotate(-90, 90, 0);

            }


        }

    }
    // Update is called once per frame
    //void Update()
    //{

    //}
}
