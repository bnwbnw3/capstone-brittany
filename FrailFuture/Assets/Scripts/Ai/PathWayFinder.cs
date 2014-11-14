using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PathWayFinder
{
    private Graph g;
    private int _desiredEndingIndex;
    private int _currentGraphVertex;
    private int _lowestEndPathIndex;
    private float _currentNeutralityValue;
    private List<Node> indexInputsDesired;
    private int nextToUse;
    private bool isEndOfPath;
    private int endIndex;
    private int possibleNextNeighbors;

    public PathWayFinder(Graph toUse, int lowestEndPathIndex, int currentGraphVertex)
    {
        g = toUse;
        nextToUse = 0;
        isEndOfPath = false;
        _currentGraphVertex = currentGraphVertex;
        _lowestEndPathIndex = lowestEndPathIndex;
        possibleNextNeighbors = 0;
    }

    public int findVertexAt(int input)
    {
        var choices = g.findAllNeighbors(_currentGraphVertex);
        possibleNextNeighbors = choices.Count();
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
        if (possibleNextNeighbors == 0)
        {
            possibleNextNeighbors = g.findAllNeighbors(_currentGraphVertex).Count();
        }
        return possibleNextNeighbors;
    }

    public Node findNextDesiredInput(int input, int desiredEndingIndex, float currentNeutralityValue, int KnownVertex = 0)
    {
        Node toReturn;
        _desiredEndingIndex  = desiredEndingIndex;
        _currentGraphVertex = KnownVertex == 0 ?  findVertexAt(input): KnownVertex;
        possibleNextNeighbors = g.findAllNeighbors(_currentGraphVertex).Count();
        _currentNeutralityValue = currentNeutralityValue;
        int lastDesiredIndex = nextToUse > 0 ? nextToUse - 1 : 0;

        //needToCreateANewPath == the end of path hasn't been hit 
                                //AND the index pool is null 
                                    //OR players input is not the input that was needed 
                                    //OR the last index this will give does not equal the desired end index
        bool needToCreateANewPath = (!isEndOfPath && (indexInputsDesired == null || indexInputsDesired[lastDesiredIndex].input != input || indexInputsDesired[indexInputsDesired.Count - 1].vertex != desiredEndingIndex));
        if (possibleNextNeighbors < 1)
        {
            endIndex = _currentGraphVertex;
            isEndOfPath = true;
        }
        else if (needToCreateANewPath)
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
        possibleNextNeighbors = 0;
    }

    /// <summary>
    /// Generate the next path to take to get to the end vertex.
    /// </summary>
    /// <param name="currentVertex">vertex to start from</param>
    private void generatePath(int currentVertex)
    {
        bool done = false;
        List<int> desiredTried = new List<int>();
        while (!done)
        {
            List<Node> start = new List<Node>();
           done = generatePathHelper(g.findAllNeighbors(currentVertex), start);

            //if the first gen path doesn't work then the desired end node can't be reached, so find a new one
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
                    //get ranges of indexs of the same neutrality type "going up" or "going down" as the original desired index.
                    List<int> range = Enumerable.Range(_lowestEndPathIndex, g.vCount()).Where(i => neighborsLeft.Contains(i) && !desiredTried.Contains(i) && IsACorrectNextBestIndex(i)).ToList();
                    //if there are none try 
                    if (range.Count <= 0 )
                    {
                        //random
                        if(desiredTried.Contains(_lowestEndPathIndex + 2))
                        {
                            List<int> range2 = Enumerable.Range(_lowestEndPathIndex, g.vCount()).Where(i => neighborsLeft.Contains(i) && !desiredTried.Contains(i)).ToList();
                            var rand = new System.Random();
                            int index = rand.Next(0, range.Count);
                            _desiredEndingIndex = range.ElementAt(index);
                        }
                        //the neutral path
                        else
                        {
                            _desiredEndingIndex = _lowestEndPathIndex + 2;
                        }
                    }
                    else
                    {
                        //it's a good range
                        var rand = new System.Random();
                        int index = rand.Next(0, range.Count);
                        _desiredEndingIndex = range.ElementAt(index);
                    }
                   done = generatePathHelper(g.findAllNeighbors(currentVertex), start);
            }
        }
    }

    private bool IsACorrectNextBestIndex(int newDesiredEndIndex)
    {
         //if the difference between the desired and new index is positive while the current neutrality is negative it's good
        //OR if the difference between the desired and new index is negative while the current neutrality is positive it's good
        //ex: 16 is a "better" neutrality than 15, so going from 15 -> 16 (15- 16) is negative which symbolizes going "Up"
        //ex: 14 is a "worse" neutrality than 15, so going from 15 -> 14 (15- 14) is positive which symbolizes going "Down"
        //if neutrality is neutral it's fine to have a difference of either way.
        bool isCorrect = true;
        if (_currentNeutralityValue != 0)
        {
            bool currentNeutralityIsGoingDown = (_currentNeutralityValue < 0);
            bool newIndexIsGoingPos = (_desiredEndingIndex - newDesiredEndIndex) > 0;
            isCorrect = currentNeutralityIsGoingDown == newIndexIsGoingPos;
        }
       return isCorrect;
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