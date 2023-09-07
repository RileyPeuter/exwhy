using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class gridScript{

    //To know the dimensions of the grid
    public int xMax;
    public int yMax;

    //These hold the gameObject PRefabs
    public GameObject[] tilePrefabs;
    public GameObject[] iconPrefabs;


    public globalGameController ggc;

    //This variable holds the basic cell or "tile" data for a world. It's stored it chars, so to make it easy to create and edit maps with little to know coding knowledge
    public char[,] worldData;
    //This variable holds events. Each event, such as non random battle, shops or story events have a number. 0 is used to denote no event. 
    //This should be the same dimensions as world data. 
    public int[,] eventMask;

    //The this holds references to the grid cells one they are created.
    public gridCell[,] gridCells;

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
            for(int x = 0; x < 16; x++)
            {
                GameObject.Find("globalGameController").GetComponent<globalGameController>().notPrint("OverworldTiles/sprites/" + resourceName + "/" + tp.name + (x + 1));
                //OverworldTiles / sprites / arena / floorSprites1.png
                spriteSheet[y, x] = Resources.Load<Sprite>("OverworldTiles/sprites/"+ resourceName + "/" + tp.name + (x + 1));//+ (x + 1));
                                                          //OverworldTiles/sprites/arena/floorSpritesfloorSprites.png
            }
            y = y + 1; 
        }
    }

    //Experimental constructor
    /*
    public gridScript() //int width, int height, char[,] WD, int[,] eventMask Args
    {
        gridCells = new gridCell[width,height];
        xMax = width;
        yMax = height;
        for(int i = 0; i < xMax; i++)
        {
            for(int z = 0; z <yMax; z++)
            {
                gridCells[i, z] = new gridCell(i, z, WD[i,z]);
                gridCells[i, z].eventIndex = eventMask[i, z];
                GameObject.Find("OverworldTiles5").GetComponent<playerOverworldScript>().negroid();
                GameObject.Instantiate();
                
            }
        }
    }
	*/

    //Place Holder. This should be overriden in the implementation
    public virtual void instantiateCells()
    {
        //This instatiates and gives basic properties to each cell, such as whether or not it's able to be walked on
        foreach (gridCell cell in gridCells)
        {
            switch (cell.cellType)
            {

                case 'a':
                    cell.cellGO = GameObject.Instantiate(tilePrefabs[1], new Vector3(cell.xPosition, cell.yPosition, 1) * 2, new Quaternion());
                    cell.spriteID = 1;
                    break;

                case 'g':
                    cell.cellGO = GameObject.Instantiate(tilePrefabs[0], new Vector3(cell.xPosition, cell.yPosition, 1) * 2, new Quaternion());
                    cell.spriteID = 0;
                    break;

                case 'c':
                    cell.cellGO = GameObject.Instantiate(tilePrefabs[1], new Vector3(cell.xPosition, cell.yPosition, 1) * 2, new Quaternion());
                    cell.spriteID = 0;
                    break;


                case 'b':
                    cell.cellGO = GameObject.Instantiate(tilePrefabs[0], new Vector3(cell.xPosition, cell.yPosition, 1) * 2, new Quaternion());
                    cell.walkable = false;
                    cell.spriteID = 0;
                    break;
                    cell.cellGO.GetComponent<SpriteRenderer>().sprite = spriteSheet[cell.spriteID, cell.colID];

            }
            cell.cellGO.GetComponent<SpriteRenderer>().sprite = spriteSheet[cell.spriteID, cell.colID];
        }
    }



    //Place Holder. This should be overriden in the implementation
    public virtual void instantiateEvents()
    {
        //This instatiates and gives basic properties to each cell, such as whether or not it's able to be walked on
        foreach (gridCell cell in gridCells)
        {
            //This adds events to cells. Battles, cutscenes or shops. 
            switch (cell.eventIndex)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    cell.cellGO = GameObject.Instantiate(iconPrefabs[0], new Vector3(cell.xPosition, cell.yPosition, 1) * 2, new Quaternion());
                    break;
                case 9:
                    break;
                case 10:
                    cell.cellGO = GameObject.Instantiate(iconPrefabs[1], new Vector3(cell.xPosition, cell.yPosition, 1) * 2, new Quaternion());
                    break;
            }
        }

        //Sets the cell sprite to the correct cell sprite
    }


    //This is the script to instantiate the grid in the game world
    //It reads each cell in a grid, then "draws" it in the game world
    public void drawGrid()
    {
        //This section gets the consolidation ID. In other words, checks the cell type around each cell, and gives it a different sprite depending. 
        //For instance, if you have a water cell next to a grass cell, it shows a shore line on the water sprite. 
        foreach (gridCell cell in gridCells)
        {
            cell.colID = getConsolidationID(this, cell.xPosition, cell.yPosition);
        }

        instantiateCells();
        instantiateEvents();


        }

   

    //This function is used to trigger an event. 
    public abstract void eventTest(int eventIndex);

    public void consolidate(gridCell gc)
    {
        gc.cellGO.GetComponent<SpriteRenderer>().sprite = spriteSheet[gc.spriteID, gc.colID];
    }


    //This bit of code may look complicated, but it's simple. It gets a cell in a grid, then checks the surrounding cells to see whether they're the same type. 
    // If you want some grass next to some water, then it'll show a shoreline, instead of the two sprites next to each other. 
    //0 = middle, 1-4 = North South Left Right, 5-8 = NW, SW, SE, NE, 9-12 = INW, ISW, ISE, INE, 13-14 = NS, WE 15 = Pillar
    public static int getConsolidationID(gridScript gs, int x, int y)
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
        } else{
            left = (gs.worldData[x - 1, y] == tileChar);
        }

        if(y == 0)
        {
            down = true;
        }
        else
        {
            down = (gs.worldData[x, y-1] == tileChar);
        }


        if (x == gs.xMax)
        {
            right = true;
        }
        else
        {
            right = (gs.worldData[x + 1, y] == tileChar);
        }

        if (y == gs.yMax)
        {
            up = true;
        }
        else
        {
            up = (gs.worldData[x, y + 1] == tileChar);
        }

        //Up Down Left Right
        //Centre
        if(up && down && left && right)
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

}

