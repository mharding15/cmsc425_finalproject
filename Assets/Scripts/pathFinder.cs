using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathFinder : MonoBehaviour
{
    TileMap map;
    public TileClickable target;

    Vector3 initPos, targetPos;
    Node start;

    public pathFinder(TileMap mapIn, int startX, int startY, TileClickable target)
    {
        this.target = target;
        this.map = mapIn;

        initPos = new Vector3(startX, 0, startY);
        targetPos = new Vector3(target.tileX, 0 , target.tileY);
        start = new Node(this.map, initPos, targetPos, 0, false);
       
    }

    public List<Vector3> solve()
    {
        return new trailBlazer(start).solve();
    }

    class trailBlazer
    {
        SortedList<int, List<Node>> openList;
        List<Vector3> closedList;
        Node StartPoint;
        Vector3 targetPos;
        TileMap map;

        public trailBlazer(Node curr)
        {
            StartPoint = curr;
            openList = new SortedList<int, List<Node>>();
            closedList = new List<Vector3>();
            map = curr.map;
            
        }

        public List<Vector3> solve()
        {
            openList.Add(StartPoint.cost(), new List<Node>());
            openList[StartPoint.cost()].Add(StartPoint);
            return solveAux(StartPoint, 0, new Queue<Vector3>());
        }

        private List<Vector3> solveAux(Node currNode, int gAcc, Queue<Vector3> retVal) //should be FIFO
        {

            if (openList.Count == 0)
            {
                return new List<Vector3>(); 
            } 

            Vector3 currPos = currNode.pos;
            Vector3 shift;

            if (currPos == targetPos)                                       //  | a | b | c |
            {//target found                                                 //  | d | - | e |
                return new List<Vector3>(retVal);                           //  | f | g | h |
            } 
            else
            {

                // take nearest next tile
                //add neighbors to openList
                Vector3 newPos;
                if (currPos.x < Manager.Instance.mapSizeX-1)
                { //if there's a possible right neighbor

                    //e
                    shift = new Vector3(1, 0, 0);
                    newPos = currPos + shift;
                    if (!closedList.Contains(newPos))
                    {
                        peep(newPos, false);
                    }

                    if (currPos.y < Manager.Instance.mapSizeY-1)
                    {
                        //c
                        shift = new Vector3(1, 0, 1);
                        newPos = currPos + shift;
                        if (!closedList.Contains(newPos))
                        {
                            peep(newPos, true);
                        }
                        //b
                        shift = new Vector3(0, 0, 1);
                        newPos = currPos + shift;
                        if (!closedList.Contains(newPos))
                        {
                            peep(newPos, false);
                        }
                    }
                    
                    if (currPos.y > 0)
                    {
                        //h
                        shift = new Vector3(1, 0, -1);
                        newPos = currPos + shift;
                        if (!closedList.Contains(newPos))
                        {
                            peep(newPos, true);
                        }
                        //g
                        shift = new Vector3(0, 0,-1);
                        newPos = currPos + shift;
                        if (!closedList.Contains(newPos))
                        {
                            peep(newPos, false);
                        }
                    }
                       
                }
               
                if (currPos.x > 0)
                { //if there's a possible left neighbor

                    //d
                    shift = new Vector3(-1, 0, 0);
                    newPos = currPos + shift;
                    if (!closedList.Contains(newPos))
                    {
                        peep(newPos, false);
                    }
                    //f
                    shift = new Vector3(-1, 0, -1);
                    newPos = currPos + shift;
                    if (!closedList.Contains(newPos))
                    {
                        peep(newPos, true);
                    }
                    //a
                    shift = new Vector3(-1, 0, 1);
                    newPos = currPos + shift;
                    if (!closedList.Contains(newPos))
                    {
                        peep(newPos, true);
                    }
                }

                //Recurse
                int firstKey = openList.Keys[0];
                List<Node> nextSmallest = openList[firstKey];
                Queue<Vector3> retValBranch = new Queue<Vector3>(retVal);
                retValBranch.Enqueue(nextSmallest[0].pos);
                Node temp = nextSmallest[0];
                nextSmallest.Remove(temp);
                if (nextSmallest.Count == 0) openList.Remove(firstKey);
                return solveAux(temp, gAcc, retVal); 
            }


            //helper
            void peep(Vector3 pos, bool diag)
            {   //adds to open list if not visited already
                Node neighbor;
                neighbor = new Node(this.map, pos, targetPos, currNode.getGCost(), diag);
                if (openList[neighbor.cost()] == null) 
                {
                    openList.Add(neighbor.cost(), new List<Node>());
                }
                openList[neighbor.cost()].Add(neighbor);
                closedList.Add(neighbor.pos);
                
            }
        }
    }



    //Node type
    class Node
    {
        public Vector3 pos, target;
        int costToEnter;

        protected int hCost { get; private set; }
        protected int gCost { get; private set; }
        protected int fCost { get; private set; }

        public int horizScale = 10;
        public int diagScale = 14;

        public TileMap map;  


        public Node(TileMap mapIn, Vector3 currPos, Vector3 end, int gCostAcc, bool wasDiag)
        {
            map = mapIn;

            target = end;
            pos = currPos;
            costToEnter = map.tileTypes[map.tiles[ (int)currPos.x, (int)currPos.y]].cost();

            hCost = hCostCalc(target);
            int scale = (wasDiag) ? diagScale : horizScale;
            gCost = gCostAcc + (costToEnter * scale);
            fCost = hCost + gCost;
        }

        // methods 
        public int hCostCalc(Vector3 target)
        {
            int xDist = Mathf.Abs((int)pos.x - (int)target.x);
            int yDist = Mathf.Abs((int)pos.y - (int)target.y);
            int diff = Mathf.Abs(xDist - yDist);
            return (diagScale * Mathf.Min(xDist, yDist) + horizScale * diff);
        }

        public int cost()
        {
            return fCost;
        } 
        
        public int getGCost()
        {
            return gCost;
        }
    }
  

}
