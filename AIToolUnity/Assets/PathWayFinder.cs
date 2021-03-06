﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PathWayFinder
{
    Graph g;
    int _desiredEndingIndex;
    List<Node> indexInputsDesired;
    int nextToUse;
    bool endPath;
    public int endIndex;
    int currentGraphVertex;
    int _lowestEndPathIndex;
    int possibleNextInputs;

    public PathWayFinder(Graph toUse, int lowestEndPathIndex)
    {
        g = toUse;
        nextToUse = 0;
        endPath = false;
        currentGraphVertex = 0;
        _lowestEndPathIndex = lowestEndPathIndex;
        possibleNextInputs = 0;
    }

    public int findVertexAt(int input)
    {
        var choices = g.findAllNeighbors(currentGraphVertex);
        possibleNextInputs = choices.Count;
        int vertex = currentGraphVertex;
        if (input <= choices.Count && input > 0)
        {
            vertex = choices[input - 1];
        }
        else if (input == 0)
        {
            vertex = 0;
        }
        else
        {
            throw new ArgumentException();
        }
        return vertex;
    }

    public int getNumPossibleInputs(int vertex)
    {
        if (vertex >= 0 && vertex < g.vCount())
        {
            int numInputs = g.findAllNeighbors(vertex).Count;
            return numInputs;
        }
        return -1;
    }
    public int getNumPossibleInputs()
    {
        return possibleNextInputs;
    }


    public Node getNextDesiredInput(int input, int desiredEndingIndex)
    {
        Node toReturn;
        _desiredEndingIndex  = desiredEndingIndex;
        currentGraphVertex = findVertexAt(input);
        possibleNextInputs =  g.findAllNeighbors(currentGraphVertex).Count;
        if (possibleNextInputs < 1)
        {
            endIndex = currentGraphVertex;
            endPath = true;
        }
        if(!endPath && (indexInputsDesired == null || !(indexInputsDesired[nextToUse-1].input == input && indexInputsDesired[indexInputsDesired.Count - 1].vertex == desiredEndingIndex)))
        {
            nextToUse = 0;
            generatePath(currentGraphVertex);
        }

        if (!endPath && nextToUse < indexInputsDesired.Count)
        {
           toReturn = indexInputsDesired[nextToUse++];
        }
        else
        {
            //need to reset inputs. Have reached the Ending
            toReturn = new Node() { input = -1, vertex = -1 };
        }
        
        return toReturn;
    }

    public void reset()
    {
        currentGraphVertex = 0;
        indexInputsDesired = null;
        endPath = false;
        nextToUse = 0;
        possibleNextInputs = 0;
    }

    private void generatePath(int currentVertex)
    {
        bool done = false;
        List<int> desiredTried = new List<int>();
        while (!done)
        {
            List<Node> start = new List<Node>();
            //start.Add(new Node() { input = 0, vertex = currentVertex });
            done = generatePathHelper(g.findAllNeighbors(currentVertex), start);
            if (!done)
            {
                desiredTried.Add(_desiredEndingIndex);
                    List<int> neighborsLeft = g.findAllNeighbors(currentVertex);
                    var range = Enumerable.Range(_lowestEndPathIndex, g.vCount()).Where(i => neighborsLeft.Contains(i) && !desiredTried.Contains(i)).ToList();
                  // var rand = new System.Random();
                   //int index = rand.Next(0, range.Count);
                   _desiredEndingIndex = range.ElementAt(0);

                   done = generatePathHelper(g.findAllNeighbors(currentVertex), start);
            }
        }
    }

    private bool generatePathHelper(List<int> parents, List<Node> set)
    {
        if (parents.Count == 0 && set.Count > 0)
        {
            if (set[set.Count- 1].vertex == _desiredEndingIndex)
            {
                return true;
            }
            else
            {
                set.RemoveAt(set.Count - 1);
                return false;
            }
        }

        for (int i = 0; i < parents.Count; i++)
        {
            List<int> newParents = g.findAllNeighbors(parents[i]);
                set.Add( new Node() { input = i + 1, vertex = parents[i] });
            bool done = generatePathHelper(newParents, set);
            if (done)
            {
                indexInputsDesired = set;
                return done;
            }
        }
        if (set.Count > 0)
        {
            set.RemoveAt(set.Count - 1);
        }
        return false;
    }

    public bool isEndOfPath()
    {
        return endPath;
    }
}


public class Node
    {
        public int vertex;
        public int input;
    }

/*

    public int generateDesiredInput()
    {
      // pathCost = new double[g.vCount()];
       pointer = new int[g.vCount()];
       List<int> topo = new TopologicalSort().sort(g);

       for (int i = 0; i < topo.Count; i++)
       {
           var parents = getParents(i, topo)
           if (parents.Count > 0)
           {
               int desiredParent = (int)parents.Min(n => pathCost[n] + g.getEdge(n, i));
              // pathCost[i] = pathCost[desiredParent] + g.getEdge(desiredParent, i);
               pointer[i] = desiredParent;
           }
       }

        //now switch pointers to point to parent instead of away
       // var correctPaths = pointer.Select(n=>

        //------------------------------------------------------------------------

    }

       

        private List<int> getParents(int index, List<int> source)
        {
            return Enumerable.Range(0, source.Count).Where(n => g.findAllNeighbors(n).Contains(index)).ToList();
        }
        
        //find lowest path
        int lowestYIndexStart = 0;
        int X = width()-1;
        for(int y = 1; y < height(); y++)
        {
            if(pathCost[X][y] < pathCost[X][lowestYIndexStart])
            {
                lowestYIndexStart = y;
            }
        }
        //make and return path
        int[] path = new int[width()];
        path[width()-1] = lowestYIndexStart;
        for(int i = width()-2; i >= 0; i--)
        {
            path[i] = pointer[i][path[i+1]].y;
        }
        return path;
    }

    }
}
*/