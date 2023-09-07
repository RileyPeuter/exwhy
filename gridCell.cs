using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridCell{
   public int xPosition;
    public int yPosition;
    public GameObject cellGO;
    public char cellType;
    public int eventIndex;
    public int colID;
    public int spriteID;
    public string resourceName;
        /* 
    g = grass
    w= water
    c = crevace
    p = path

    */
    public bool walkable;

    public gridCell(int x, int y, char tipe)
    {
        cellType = tipe;
        eventIndex = 0;
        walkable = true;
        xPosition = x;
        yPosition = y;
    }   

    public void consolidate()
    {

    }   

}
