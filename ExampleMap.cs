using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : gridScript
{

    // Start is called before the first frame update
    public arenaMap()
    {

        spriteSheet = new Sprite[2, 16];
        resourceName = "arena";
        gridCells = new gridCell[14, 12];
        yMax = 12;
        xMax = 14;
        ggc = GameObject.Find("globalGameController").GetComponent<globalGameController>();

        tilePrefabs = new GameObject[] {
          //  (GameObject)Resources.Load("OverworldTiles/OverworldTiles1"),
           // (GameObject)Resources.Load("OverworldTiles/OverworldTiles2"),
           // (GameObject)Resources.Load("OverworldTiles/OverworldTiles3") ,
           // (GameObject)Resources.Load("OverworldTiles/OverworldTiles4") ,
           // (GameObject)Resources.Load("OverworldTiles/OverworldTiles5") ,
            (GameObject)Resources.Load("OverworldTiles/wallSprites") ,
            (GameObject)Resources.Load("OverworldTiles/floorSprites") ,
           // (GameObject)Resources.Load("OverworldTiles/OverworldTiles10")
            };

        iconPrefabs = new GameObject[]
        {
            (GameObject)Resources.Load("OverworldTiles/OverworldTiles9") ,
            (GameObject)Resources.Load("OverworldTiles/OverworldTiles10")    
        };


        worldData = new char[15, 13]
{
        {'b','b','b','b','b','b','b','b','b','b','b','b','b'},
        {'b','a','a','a','a','b','b','b','b','b','b','b','b'},
        {'b','a','a','a','a','a','a','a','b','b','b','b','b'},
        {'b','a','a','a','a','b','b','a','b','b','b','b','b'},
        {'b','a','a','a','a','b','b','a','a','a','a','b','b'},
        {'b','a','a','a','a','a','a','a','b','b','a','b','b'},
        {'b','a','a','a','a','b','b','a','b','b','a','b','b'},
        {'b','a','a','a','a','b','b','a','b','b','a','a','b'},
        {'b','a','a','a','a','b','b','a','b','b','a','b','b'},   
        {'b','a','a','a','a','a','a','a','b','b','a','b','b'},
        {'b','a','a','a','a','b','b','a','a','a','a','b','b'},
        {'b','a','a','a','a','b','b','a','b','b','b','b','b'},
        {'b','a','a','a','a','a','a','a','b','b','b','b','b'},
        {'b','a','a','a','a','b','b','b','b','b','b','b','b'},
        {'b','b','b','b','b','b','b','b','b','b','b','b','b'}
};

        eventMask = new int[15, 13]
        {{0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,2,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,6,0,0,0,0},
         {0,0,0,0,0,3,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,8,9},
         {0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,5,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,7,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,4,0,0,0,0,0,0,0},
         {0,10,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0}
        };




        for (int i = 0; i < xMax; i++)
        {
            for (int z = 0; z < yMax; z++)
            {
                gridCells[i, z] = new gridCell(i, z, worldData[i, z]);
                gridCells[i, z].eventIndex = eventMask[i, z];

            }
        }
        loadSpriteMap();

        drawGrid();

    }



    public override void eventTest(int evIndex)
    {
        switch (evIndex)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
                ggc.sceneLoader(1);
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
