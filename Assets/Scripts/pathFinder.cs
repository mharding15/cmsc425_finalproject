using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class pathFinder
{

    TileMap map;
    TileQ openList;
    List<TileClickable> closedList;
    int thisX, thisY;

    public int horizScale = 10;
    public int diagScale = 14;
    public TQNode target;

    public pathFinder(TileMap map, int startX, int StartY, TileClickable target)
    {
        this.target = target;
        this.thisX = startX;
        this.thisY = StartY;

    }

    public int hCost(TQNode target)
    {
        int xDist = Mathf.Abs(this.x - target.x);
        int yDist = Mathf.Abs(this.y - target.y);
        int diff = Mathf.Abs(xDist - yDist);
        return diagScale * Mathf.Min(xDist, yDist) + horzScale * diff;
    }

    public int gCost()
    {
        //TODO
    }
}

class TileQ : MonoBehaviour
{

    // Implemented a doubly linked list priority queue to handle the A* queue
    protected TQNode first;
    protected TQNode last;
    int size;

    public TileQ()
    {
        size = 0;
    }

    public void Enqueue(TileClickable tileIn)
    {
        TQNode curr;
        TQNode input = new TQNode(tileIn);

        if (this.first == null)
        {
            this.first = input;
            this.last = input;
            return;
        }
        curr = this.first;
        //TODO if input < first

        while (curr != this.last)
        {
            if ( true)//Heck fix node before comparing input < curr
            {
                curr.prev().setNext(input); //put this node after the current's last
                input.setLast(curr.prev()); //link input back to the current's last
                input.setNext(curr);        //link input with current 
                curr.setLast(input);        //link current back to input
                return;
            }
            else
            {
                curr = curr.nextTile();
            }
        }
        // larger than last value
        this.last.setNext(input);
        this.last = input;


    }

    public void Dequeue()
    {
        //TODO
    }

    public int length()
    {
        return this.size;
    }
}


class TQNode
{
    int x;
    int y;
    public TileMap map;

    private int hCost;
    private int gCost;
    private int fCost;

    TQNode next;
    TQNode last;

    public TQNode(TileClickable tile)
    {
        x = tile.tileX;
        y = tile.tileY;
        map = tile.map;
    }

    public TQNode nextTile()
    {
        return next;
    }
    public TQNode prev()
    {
        return last;
    }

    public void setNext(TQNode nodeIn)
    {
        this.next = nodeIn;
    }
    public void setLast(TQNode nodeIn)
    {
        this.last = nodeIn;
    }

}