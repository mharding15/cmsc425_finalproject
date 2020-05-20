﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathFinder
{
    TileMap map;

    Vector3 initPos, targetPos;
    Node start;

    public pathFinder(TileMap mapIn, int startX, int startY, int destX, int destY)
    {
        this.map = mapIn;

        initPos = new Vector3(startX, 0, startY);
        targetPos = new Vector3(destX, 0 , destY);
        start = new Node(this.map, initPos, null, 0, false, true);
       
    }

    public List<Vector3> solve()
    {
        return new trailBlazer(start, targetPos).solve();
    }

    class trailBlazer
    {
        SortedList<int, List<Node>> openList;
        List<Vector3> closedList;
        Node StartPoint;
        Vector3 targetPos;
        TileMap map;

        public trailBlazer(Node curr, Vector3 target)
        {
            StartPoint = curr;
            openList = new SortedList<int, List<Node>>();
            closedList = new List<Vector3>();
            map = curr.map;
            targetPos = target;
            
        }

        public List<Vector3> solve()
        {
            openList.Add(StartPoint.cost(), new List<Node>());
            openList[StartPoint.cost()].Add(StartPoint);
            return solveAux(new Queue<Vector3>());
        }

        private List<Vector3> tracePath(Node node)
        {
            List<Vector3> path = new List<Vector3>();
            while (node != StartPoint)
            {
                path.Insert(0, node.pos);
                node = node.parentNode;
            }
            path.Insert(0, StartPoint.pos);
            return path;
        }

        private List<Vector3> solveAux(Queue<Vector3> retVal) //should be FIFO
        {
            int ap = 0;
            Node currNode;
            while (openList.Count > 0)
            {
                //Recurse
                int firstKey = openList.Keys[0];
                List<Node> nextSmallest = openList[firstKey];
                Queue<Vector3> retValBranch = new Queue<Vector3>(retVal);
                retValBranch.Enqueue(nextSmallest[0].pos);
                currNode = nextSmallest[0];
                nextSmallest.Remove(currNode);
                if (nextSmallest.Count == 0) openList.Remove(firstKey);

                ap++;
                Vector3 currPos = currNode.pos;
                Vector3 shift;
                closedList.Add(currPos);

                if (currPos == targetPos)                                       //  | a | b | c |
                {//target found                                                 //  | d | - | e |
                    return tracePath(currNode);                                 //  | f | g | h |
                }
                else
                {

                    // take nearest next tile
                    //add neighbors to openList
                    Vector3 newPos;

                    if (currPos.z < Manager.Instance.mapSizeY - 1)
                    {
                        //b
                        shift = new Vector3(0, 0, 1);
                        newPos = currPos + shift;
                        if (!closedList.Contains(newPos))
                        {
                            peep(newPos, currNode, false);
                            if (newPos == targetPos)
                            {//target found      
                                return tracePath(openListFind(newPos));
                            }
                        }
                    }


                    if (currPos.z > 0)
                    {
                        //g
                        shift = new Vector3(0, 0, -1);
                        newPos = currPos + shift;
                        if (!closedList.Contains(newPos))
                        {
                            peep(newPos, currNode, false);
                            if (newPos == targetPos)
                            {//target found      
                                return tracePath(openListFind(newPos));
                            }
                        }
                    }

                    if (currPos.x < Manager.Instance.mapSizeX - 1)
                    { //if there's a possible right neighbor

                        //e
                        shift = new Vector3(1, 0, 0);
                        newPos = currPos + shift;
                        
                        if (!closedList.Contains(newPos))
                        {
                            peep(newPos, currNode, false);
                            if (newPos == targetPos)
                            {//target found      
                                return tracePath(openListFind(newPos));
                            }
                        }

                        

                        if (currPos.z < Manager.Instance.mapSizeY - 1)
                        {
                            //c
                            shift = new Vector3(1, 0, 1);
                            newPos = currPos + shift;
                            if (!closedList.Contains(newPos))
                            {
                                peep(newPos, currNode, true);
                                if (newPos == targetPos)
                                {//target found      
                                    return tracePath(openListFind(newPos));
                                }
                            }
                        }

                        if (currPos.z > 0)
                        {
                            //h
                            shift = new Vector3(1, 0, -1);
                            newPos = currPos + shift;
                            if (!closedList.Contains(newPos))
                            {
                                peep(newPos, currNode, true);
                                if (newPos == targetPos)
                                {//target found      
                                    return tracePath(openListFind(newPos));
                                }
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
                            peep(newPos, currNode, false);
                            if (newPos == targetPos)
                            {//target found      
                                return tracePath(openListFind(newPos));
                            }
                        }

                        if (currPos.z > 0)
                        {
                            //f
                            shift = new Vector3(-1, 0, -1);
                            newPos = currPos + shift;
                            if (!closedList.Contains(newPos))
                            {
                                peep(newPos, currNode, true);
                                if (newPos == targetPos)
                                {//target found      
                                    return tracePath(openListFind(newPos));
                                }
                            }
                        }

                        if (currPos.z < Manager.Instance.mapSizeY - 1)
                        {
                            //a
                            shift = new Vector3(-1, 0, 1);
                            newPos = currPos + shift;
                            if (!closedList.Contains(newPos))
                            {
                                peep(newPos, currNode, true);
                                if (newPos == targetPos)
                                {//target found      
                                    return tracePath(openListFind(newPos));
                                }
                            }
                        }
                    }

                   
                    
                    //return solveAux(temp, gAcc, retVal);
                }

            }


            return new List<Vector3>(retVal);


        }

        //helper
        void peep(Vector3 pos, Node sourceNode, bool diag)
        {   //adds to open list if not visited already
            Node neighbor;
            neighbor = new Node(this.map, pos, sourceNode, sourceNode.getGCost(), diag, false);

            

            Node foundInstance = openListFind(pos);
            if (foundInstance != null)
            {
                if (foundInstance.cost() > neighbor.cost())
                {
                    openListRemove(foundInstance);
                    try
                    {
                        List<Node> lst = openList[neighbor.cost()];
                    }
                    catch (KeyNotFoundException e)
                    {
                        openList.Add(neighbor.cost(), new List<Node>());
                    }
                    openList[neighbor.cost()].Add(neighbor);
                }
            }
            else
            {
                try
                {
                    List<Node> lst = openList[neighbor.cost()];
                }
                catch (KeyNotFoundException e)
                {
                    openList.Add(neighbor.cost(), new List<Node>());
                }
                openList[neighbor.cost()].Add(neighbor);
            }
            //closedList.Add(neighbor.pos);

        }

        Node openListFind(Vector3 pos)
        {
            Node foundInstance = null;
            foreach (KeyValuePair<int, List<Node>> kvp in openList)
            {
                foundInstance = kvp.Value.Find(x => x.pos.Equals(pos));
                if (foundInstance != null)
                {
                    return foundInstance;
                }
            }
            return foundInstance;
        }

        void openListRemove(Node node)
        {
            List<int> emptyKeys = new List<int>();
            foreach (KeyValuePair<int, List<Node>> kvp in openList)
            {
                kvp.Value.Remove(node);
                if (kvp.Value.Count == 0)
                {
                    emptyKeys.Add(kvp.Key);
                }
            }

            foreach (int i in emptyKeys)
            {
                openList.Remove(i);
            }
        }
    }



    //Node type
    class Node
    {
        public Vector3 pos;

        public Node parentNode;
        int costToEnter;

        protected int hCost { get; private set; }
        protected int gCost { get; private set; }
        protected int fCost { get; private set; }

        public int horizScale = 10;
        public int diagScale = 14;

        public TileMap map;




        public Node(TileMap mapIn, Vector3 currPos, Node source, int gCostAcc, bool wasDiag, bool isStart)
        {
            map = mapIn;

            parentNode = source;
            pos = currPos;
            costToEnter = map.tileTypes[map.tiles[ (int)currPos.x, (int)currPos.z]].cost();
            //costToEnter = 1;
            if (!isStart) {
                /*if (costToEnter < 0) {
                    hCost = hCostCalc(parent);
                } else
                {
                    hCost = int.MaxValue;
                }
                */
                hCost = 0;
                int scale = (wasDiag) ? diagScale : horizScale;
                gCost = gCostAcc + (costToEnter * scale);
                fCost = hCost + gCost;
            } else
            {
                hCost = 0;
                gCost = 0;
                fCost = 0;
            }
            
        }


        // methods 
        public int hCostCalc(Vector3 target)
        {
            int xDist = Mathf.Abs((int)pos.x - (int)target.x);
            int yDist = Mathf.Abs((int)pos.z - (int)target.z);
            int diff = Mathf.Abs(xDist - yDist);
            return (diagScale * Mathf.Min(xDist, yDist) + horizScale * diff);
        }

        public int cost()
        {
            return gCost;
        } 
        
        public int getGCost()
        {
            return gCost;
        }

        public bool Equals(Node other)
        {
            if(other.pos.Equals(pos))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Equals(Vector3 other)
        {
            if (other.Equals(pos))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
  

}
