#include "iostream"
#include <stdlib.h>    
#include <time.h>
#include "MapGenerator.h"
using namespace std;


void main() {
	srand(time(NULL));

	int matrix[100][100];

	for (int i = 0; i < 100; i++)
		for (int j = 0; j < 100; j++)
			matrix[i][j] = 0;

	Room FirstRoom;
	FirstRoom.x = (HEIGHT + 1) / 2;
	FirstRoom.y = (WIDTH + 1) / 2;
	Door door;
	door.used = false;
	door.direction = NORTH;
	FirstRoom.doors.push_back(&door);
	Door door2;
	door2.used = false;
	door2.direction = WEST;
	FirstRoom.doors.push_back(&door2);
	Door door3;
	door3.used = false;
	door3.direction = SOUTH;
	FirstRoom.doors.push_back(&door3);
	Door door4;
	door4.used = false;
	door4.direction = EAST;
	//FirstRoom.doors.push_back(&door4);
	
	
	//APUNTE al hacerlo en este orden las puertas no se borran
	if(FillRoom(matrix, &FirstRoom));
	CreateCorridor(matrix, &FirstRoom);
	Print(matrix);
	

	system("pause");
}

bool CreateCorridor(int(&matrix)[HEIGHT + 1][WIDTH + 1], Room* room)
{
	bool ret = false;


	for (int i = 0; i < room->doors.size(); i++)
	{
		if (room->doors[i]->used)
			continue;

		int posx = room->x;
		int posy = room->y;
		switch (room->doors[i]->direction)
		{
		case NONE:
			break;
		case NORTH:
			posx -= room->size/2;
			break;
		case SOUTH:
			posx+= room->size / 2;
			break;
		case WEST:
			posy-= room->size / 2;
			break;
		case EAST:
			posy+= room->size / 2;
			break;
		}

		int length = 1 + rand() % 7;
		//cout << length;
		bool corridor_completed = true;
		for (int f = 0; f < length; f++)
		{
			Corridor corridor;
			corridor.direction = room->doors[i]->direction;

			switch (corridor.direction)
			{
			case NONE:
				break;
			case NORTH:
				corridor.posx = posx - 1;
				corridor.posy = posy;
				if (corridor.posx < 100 && corridor.posx >= 0 && corridor.posy < 100 && corridor.posy >= 0 && matrix[corridor.posx][corridor.posy] == 0)
				{
				matrix[corridor.posx][corridor.posy] = 2;
				}
				else
				{
					length = f;
					corridor_completed = false;
				}
				break;
			case SOUTH:
				corridor.posx = posx + 1;
				corridor.posy = posy;
				if (corridor.posx < 100 && corridor.posx >= 0 && corridor.posy < 100 && corridor.posy >= 0 && matrix[corridor.posx][corridor.posy] == 0)
				{
				 matrix[corridor.posx][corridor.posy] = 2;
				}
				else {
					length = f;
					corridor_completed = false;
				}
				break;
			case WEST:
				corridor.posx = posx;
				corridor.posy = posy - 1;
				if (corridor.posx < 100 && corridor.posx >= 0 && corridor.posy < 100 && corridor.posy >= 0 && matrix[corridor.posx][corridor.posy] == 0)
				{

					matrix[corridor.posx][corridor.posy] = 2;
				}
				else
				{
					length = f;
					corridor_completed = false;
				}
				break;
			case EAST:
				corridor.posx = posx;
				corridor.posy = posy + 1;
				if (corridor.posx < 100 && corridor.posx >= 0 && corridor.posy < 100 && corridor.posy >= 0 && matrix[corridor.posx][corridor.posy] == 0)
				{
					matrix[corridor.posx][corridor.posy] = 2;
				}
				else {
					length = f;
					corridor_completed = false;
				}
				break;
			}
			//devuelve falso, hay que borrar el pasillo y la puerta 
			posx = corridor.posx;
			posy = corridor.posy;
		}

		if (!corridor_completed)
		{
			RemoveCorridor(matrix, room, length, room->doors[i]->direction);
			room->doors.erase(room->doors.begin() + i);
			i--;
		}
		else
		{
			switch (room->doors[i]->direction)
			{
			case NONE:
				break;
			case NORTH:
				posx -= room->size / 2 + 1;
				break;
			case SOUTH:
				posx += room->size / 2 + 1;
				break;
			case WEST:
				posy -= room->size / 2 + 1;
				break;
			case EAST:
				posy += room->size / 2 + 1;
				break;
			}
			if (!CreateRoom(posx, posy, room->doors[i]->direction, matrix))
			{
				RemoveCorridor(matrix, room, length, room->doors[i]->direction);
				room->doors.erase(room->doors.begin() + i);
				i--;
			}//so devuelve falso, hay que borrar el pasillo y la puerta
			
		}	
	}
	ret = true;
	return ret;
}

bool CreateRoom(int posx, int posy, Direction direction, int(&matrix)[HEIGHT + 1][WIDTH + 1])
{
	//para hacer el tamaño aleatorio, pasar a esta funcion la direccion, y se coloca la posicion conforme lo al tamaño de esta, en vez de hacerlo en la funcion de los pasillos
	bool ret = true;
	Room newRoom;
	newRoom.x = posx;
	newRoom.y = posy;
	newRoom.size = 3;
	Door door;
	door.used = true;
	switch (direction)
	{
	case NONE:
		break;
	case NORTH:
		door.direction = SOUTH;
		break;
	case SOUTH:
		door.direction = NORTH;		
		break;
	case WEST:
		door.direction = EAST;
		break;
	case EAST:
		door.direction = WEST;
		break;
	}
	newRoom.doors.push_back(&door);
	int newdoors = 1; // rand() % 2;
	for (int i = 0; i < newdoors; i++)
	{
		Door door2;
		door2.used = false;
		door2.direction = door.direction;
		
		while (door2.direction == door.direction)
		{
			int type = 1 + rand() % 4;
		
			switch (type)
			{
			case 1:
				door2.direction = NORTH;
				break;
			case 2:
				door2.direction = SOUTH;
				break;
			case 3:
				door2.direction = WEST;
				break;
			case 4:
				door2.direction = EAST;
				break;
			}
		}
		newRoom.doors.push_back(&door2);
	}
	ret = FillRoom(matrix, &newRoom);
	if(ret) // si este falla, la funcion devovlera falso y no se crea la habitacion
	CreateCorridor(matrix, &newRoom);
	

	return ret;
}

bool FillRoom(int(&matrix)[HEIGHT+1][WIDTH + 1], Room* room)
{
	bool room_completed = true;
	int max_y, max_x;
	max_y = max_x = 0;

	int printed_pos[3][3];
	//APUNTE si se quiere retocar tamaño variable esto hay que cambiarlo
	for (int i = -room->size/2; i <= room->size/2; i++)
	{
		for (int j = -room->size / 2; j <= room->size / 2; j++)
		{
			if (room->x + i < 100 && room->x + i >= 0 && room->y + j < 100 && room->y + j >= 0 && matrix[room->x + i][room->y + j] == 0)
			matrix[room->x + i][room->y + j] = 1;
			else {
				room_completed = false;
				max_y = j;
				max_x = i;
				break;
			}
		}
		if (room_completed == false)
			break;
	}
	if (!room_completed)
	{
		for (int i = -room->size / 2; i <= max_x / 2; i++)
		{
			for (int j = -room->size / 2; j <= max_y; j++)
			{
				if (room->x + i < 100 && room->x + i >= 0 && room->y + j < 100 && room->y + j >= 0 && (i != max_x || j != max_y))
					matrix[room->x + i][room->y + j] = 0;
			}
		
		}
	}
	else
	for (int i = 0; i < room->doors.size(); i++)
	{
		switch (room->doors[i]->direction)
		{
		case NONE:
			break;
		case NORTH:
			matrix[room->x - room->size/2][room->y] = 3;
			break;
		case SOUTH:
			matrix[room->x + room->size / 2][room->y] = 3;
			break;
		case WEST:
			matrix[room->x][room->y - room->size / 2] = 3;
			break;
		case EAST:
			matrix[room->x][room->y + room->size / 2] = 3;
			break;
		}
	}
	return room_completed;
}

void RemoveCorridor(int(&matrix)[HEIGHT + 1][WIDTH + 1], Room* room, int length, Direction direction)
{
	int posx = room->x;
	int	posy = room->y; 
	
	switch (direction)
	{
	case NONE:
		break;
	case NORTH:
		posx -= room->size / 2;
		break;
	case SOUTH:
		posx += room->size / 2;
		break;
	case WEST:
		posy -= room->size / 2;
		break;
	case EAST:
		posy += room->size / 2;
		break;
	}

	for (int f = 0; f < length; f++)
	{
		Corridor corridor;
		corridor.direction = direction;

		switch (corridor.direction)
		{
		case NONE:
			break;
		case NORTH:
			corridor.posx = posx - 1;
			corridor.posy = posy;
			if (corridor.posx < 100 && corridor.posx >= 0 && corridor.posy < 100 && corridor.posy >= 0)
			{
				matrix[corridor.posx][corridor.posy] = 0;
			}
			break;
		case SOUTH:
			corridor.posx = posx + 1;
			corridor.posy = posy;
			if (corridor.posx < 100 && corridor.posx >= 0 && corridor.posy < 100 && corridor.posy >= 0)
			{
				matrix[corridor.posx][corridor.posy] = 0;
			}
			break;
		case WEST:
			corridor.posx = posx;
			corridor.posy = posy - 1;
			if (corridor.posx < 100 && corridor.posx >= 0 && corridor.posy < 100 && corridor.posy >= 0)
			{
				matrix[corridor.posx][corridor.posy] = 0;
			}
			break;
		case EAST:
			corridor.posx = posx;
			corridor.posy = posy + 1;
			if (corridor.posx < 100 && corridor.posx >= 0 && corridor.posy < 100 && corridor.posy >= 0)
			{
				matrix[corridor.posx][corridor.posy] = 0;
			}
			break;
		}
		posx = corridor.posx;
		posy = corridor.posy;
	}
}

void Print(int(&matrix)[HEIGHT + 1][WIDTH + 1]) {
	system("cls");
	for (int i = 0; i < 100; i++)
	{
		for (int j = 0; j < 100; j++)
		{
			if (matrix[i][j] != 0)
				cout << matrix[i][j] << " ";
			else {
				cout << "  ";
			}
		}
		cout << "\n";
	}
	cout << "\n";
}