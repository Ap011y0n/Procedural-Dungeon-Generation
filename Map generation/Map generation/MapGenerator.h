#pragma once
#ifndef _MapGenerator_h
#define _MapGenerator_h

#include <vector>
#include <array>

#define WIDTH 99
#define HEIGHT 99

using namespace std;
enum Direction
{
	
	NONE,
	NORTH,
	SOUTH,
	WEST,
	EAST,
	

};
struct Door
{
	Direction direction = NONE;
	bool used = false;
};
struct Room
{
	vector<Door*> doors;
	int x = 0;
	int y = 0;
	int size = 3;
};
struct Corridor
{
	Direction direction = NONE;
	int posx = 0;
	int posy = 0;
};
void main();
bool CreateRoom(int posx, int posy, Direction direction, int(&matrix)[HEIGHT + 1][WIDTH + 1]);
bool CreateCorridor(int(&array)[HEIGHT + 1][WIDTH + 1], Room*);
bool FillRoom(int (&array)[HEIGHT+1][WIDTH+1], Room*);
void RemoveCorridor(int(&array)[HEIGHT + 1][WIDTH + 1], Room*, int, Direction);
void Print(int(&array)[HEIGHT + 1][WIDTH + 1]);
#endif
