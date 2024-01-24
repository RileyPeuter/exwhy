using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExWhy
{
    public ExWhy(int xMx, int yMx, string resName)
    {
        xMax = xMx;
        yMax = yMx;

        resourceName = resName;

        gridCells = new ExWhyCell[yMax, yMax];
        spriteSheet = new Sprite[16, 16];

        loadPrefabs();
        loadSpriteMap();
    }



    //To know the dimensions of the grid
    public int xMax;
    public int yMax;

    //These hold the gameObject PRefabs
    public GameObject[] tilePrefabs;
    public GameObject[] iconPrefabs;


    //This variable holds the basic cell or "tile" data for a world. It's stored it chars, so to make it easy to create and edit maps with little to know coding knowledge
    public char[,] worldData;
    //This variable holds events. Each event, such as non random battle, shops or story events have a number. 0 is used to denote no event. 
    //This should be the same dimensions as world data. 
    public int[,] eventMask;

    //The this holds references to the grid cells one they are created.
    public ExWhyCell[,] gridCells;

    //These variables are used to find and load sprites.
    public Sprite[,] spriteSheet;
    public string resourceName;


    //Function to load the sprites of the grid from Unity's "resource" folder into memory
    //Each tile type should have 16 sprites
    public void loadSpriteMap()
    {
        int y = 0;
        foreach (GameObject tp in tilePrefabs)
        {
            for (int x = 0; x < 16; x++)
            {
                //GameObject.Find("globalGameController").GetComponent<globalGameController>().notPrint("OverworldTiles/sprites/" + resourceName + "/" + tp.name + (x + 1));
                //OverworldTiles / sprites / arena / floorSprites1.png
                spriteSheet[y, x] = Resources.Load<Sprite>("MapTiles/Sprites/" + resourceName + "/" + tp.name + (x + 1));//+ (x + 1));                                                                                                             //OverworldTiles/sprites/arena/floorSpritesfloorSprites.png
            }
            y = y + 1;
        }
    }

    public void loadPrefabs()
    {

        tilePrefabs = Resources.LoadAll<GameObject>("MapTiles/Prefabs/" + resourceName);

    }




    //Place Holder. This should be overriden in the implementation
    public virtual void initiateCells()
    {
        for (int x = 0; x < xMax; x = x + 1)
        {
            for (int y = 0; y < yMax; y = y + 1)
            {
                gridCells[y, x] = new ExWhyCell(y, x, worldData[y, x], eventMask[y, x]);
            }
        }
    }


    protected void instantiateCell(ExWhyCell cell, bool walkable, int prefabIndex, int spriteID)
    {

        cell.cellGO = GameObject.Instantiate(tilePrefabs[prefabIndex], new Vector3(cell.xPosition, cell.yPosition, 1) * 4, new Quaternion());
        cell.walkable = walkable;
        cell.spriteID = spriteID;

    }

    public abstract void instantiateCells();
    public abstract void instantiateEvents();



    //This is the script to instantiate the grid in the game world
    //It reads each cell in a grid, then "draws" it in the game world
    public void drawGrid()
    {
        instantiateCells();
        //        instantiateEvents();
        //This section gets the consolidation ID. In other words, checks the cell type around each cell, and gives it a different sprite depending. 
        //For instance, if you have a water cell next to a grass cell, it shows a shore line on the water sprite. 
        foreach (ExWhyCell cell in gridCells)
        {
            cell.colID = getConsolidationID(this, cell.xPosition, cell.yPosition);
            consolidate(cell);
        }




    }



    //This function is used to trigger an event. 
    public abstract void eventTest(int eventIndex);

    public void consolidate(ExWhyCell cell)
    {
        cell.cellGO.GetComponent<SpriteRenderer>().sprite = spriteSheet[cell.spriteID, cell.colID];
    }


    //This bit of code may look complicated, but it's simple. It gets a cell in a grid, then checks the surrounding cells to see whether they're the same type. 
    // If you want some grass next to some water, then it'll show a shoreline, instead of the two sprites next to each other. 
    //0 = middle, 1-4 = North South Left Right, 5-8 = NW, SW, SE, NE, 9-12 = INW, ISW, ISE, INE, 13-14 = NS, WE 15 = Pillar
    public static int getConsolidationID(ExWhy gs, int x, int y)
    {
        char tileChar = gs.worldData[x, y];
        //Likes
        bool up;
        bool right;
        bool down;
        bool left;

        if (x == 0)
        {
            left = true;
        }
        else
        {
            left = (gs.worldData[x - 1, y] == tileChar);
        }

        if (y == 0)
        {
            down = true;
        }
        else
        {
            down = (gs.worldData[x, y - 1] == tileChar);
        }


        if (x + 1 == gs.xMax)
        {
            right = true;
        }
        else
        {
            right = (gs.worldData[x + 1, y] == tileChar);
        }

        if (y + 1 == gs.yMax)
        {
            up = true;
        }
        else
        {
            up = (gs.worldData[x, y + 1] == tileChar);
        }

        //Up Down Left Right
        //Centre
        if (up && down && left && right)
        {
            return 0;
        }

        //Up Edge
        if (!up && down && left && right)
        {
            return 1;
        }

        //Down Edge
        if (up && !down && left && right)
        {
            return 2;
        }

        //Left Edge
        if (up && down && !left && right)
        {
            return 3;
        }

        //Right Edge
        if (up && down && left && !right)
        {
            return 4;
        }

        //Upper Left Edge
        if (!up && down && !left && right)
        {
            return 5;
        }

        //Upper Right Edge
        if (!up && down && left && !right)
        {
            return 6;
        }

        //Down Left Edge
        if (up && !down && !left && right)
        {
            return 7;
        }

        //Down Right Edge
        if (up && !down && left && !right)
        {
            return 8;
        }

        //Down Pocket
        if (!up && down && !left && !right)
        {
            return 9;
        }

        //Down Pocket
        if (up && !down && !left && !right)
        {
            return 10;
        }

        //Left Pocket
        if (!up && !down && !left && right)
        {
            return 11;
        }

        //Right Pocket
        if (!up && !down && left && !right)
        {
            return 12;
        }

        //Up Hallways
        if (!up && !down && left && right)
        {
            return 13;
        }
        //Side Hallways
        if (up && down && !left && !right)
        {
            return 14;
        }
        if (!up && !down && !left && !right)
        {
            return 15;
        }

        return 0;
    }

    //Now, fam, this isn't necessarily the best way. We could use a hypotenuse, but for now, we doing this. 
    public static int getDistanceBetweenCells(ExWhyCell cell1, ExWhyCell cell2){
        return (Math.Abs(cell1.xPosition - cell2.xPosition) + Math.Abs(cell1.yPosition - cell2.yPosition)); 


        }

}
