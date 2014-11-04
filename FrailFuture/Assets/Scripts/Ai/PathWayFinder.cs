using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PathWayFinder
{
    private Graph g;
    private int _desiredEndingIndex;
    private List<Node> indexInputsDesired;
    private int nextToUse;
    private bool isEndOfPath;
    private int endIndex;
    private int _currentGraphVertex;
    private int _lowestEndPathIndex;
    private int possibleNextInputs;

    public PathWayFinder(Graph toUse, int lowestEndPathIndex, int currentGraphVertex)
    {
        g = toUse;
        nextToUse = 0;
        isEndOfPath = false;
        _currentGraphVertex = currentGraphVertex;
        _lowestEndPathIndex = lowestEndPathIndex;
        possibleNextInputs = 0;
    }

    public int findVertexAt(int input)
    {
        var choices = g.findAllNeighbors(_currentGraphVertex);
        possibleNextInputs = choices.Count;
        int vertex = _currentGraphVertex;
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

    public int findNumPossibleInputs(int vertex)
    {
        if (vertex >= 0 && vertex < g.vCount())
        {
            int numInputs = g.findAllNeighbors(vertex).Count;
            return numInputs;
        }
        return -1;
    }
    public int findNumPossibleInputs()
    {
        if (possibleNextInputs == 0)
        {
            possibleNextInputs = g.findAllNeighbors(_currentGraphVertex).Count;
        }
        return possibleNextInputs;
    }

    public Node findNextDesiredInput(int input, int desiredEndingIndex, int KnownVertex = 0)
    {
        Node toReturn;
        _desiredEndingIndex  = desiredEndingIndex;
        if (KnownVertex == 0)
        {
            _currentGraphVertex = findVertexAt(input);
        }
        else
        {
            _currentGraphVertex = KnownVertex;
        }
        possibleNextInputs =  g.findAllNeighbors(_currentGraphVertex).Count;
        if (possibleNextInputs < 1)
        {
            endIndex = _currentGraphVertex;
            isEndOfPath = true;
        }
        int nextDesiredIndex = nextToUse > 0 ? nextToUse - 1 : 0;
        if (!isEndOfPath && (indexInputsDesired == null || !(indexInputsDesired[nextDesiredIndex].input == input && indexInputsDesired[indexInputsDesired.Count - 1].vertex == desiredEndingIndex)))
        {
            nextToUse = 0;
            generatePath(_currentGraphVertex);
        }

        if (!isEndOfPath && nextToUse < indexInputsDesired.Count)
        {
           toReturn = indexInputsDesired[nextToUse++];
        }
        else
        {
            //need to resetPath inputs. Have reached the Ending
            toReturn = new Node() { input = -1, vertex = -1 };
        }
        
        return toReturn;
    }

    public void reset()
    {
        _currentGraphVertex = 0;
        indexInputsDesired = null;
        isEndOfPath = false;
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
            done = generatePathHelper(g.findAllNeighbors(currentVertex), start);
            if (!done)
            {
                desiredTried.Add(_desiredEndingIndex);
                    List<int> neighborsLeft = g.findAllNeighbors(currentVertex);
                //Make sure we have a list of end index nodes
                    bool foundEndIndex = false;
                    while (!foundEndIndex)
                    {
                        foundEndIndex = true;
                        for(int i = 0; i < neighborsLeft.Count; i++)
                        {
                            List<int> nextNeighbors = g.findAllNeighbors(neighborsLeft[i]);
                            if (nextNeighbors.Count > 0)
                            {
                                foundEndIndex = false;
                                neighborsLeft.RemoveAt(i);
                                neighborsLeft.AddRange(nextNeighbors);
                            }
                        }
                    }
                    var range = Enumerable.Range(_lowestEndPathIndex, g.vCount()).Where(i => neighborsLeft.Contains(i) && !desiredTried.Contains(i)).ToList();
                    var rand = new System.Random();
                    int index = rand.Next(0, range.Count);
                   _desiredEndingIndex = range.ElementAt(index);

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

    //Get-ers
    public bool getIsEndOfPath()
    {
        return isEndOfPath;
    }
    public int getCurrentGraphIndex()
    {
        return _currentGraphVertex;
    }
    public int getAvalibleDesiredIndex()
    {
        return _desiredEndingIndex;
    }
    public int getEndIndex()
    {
        return endIndex;
    }
}