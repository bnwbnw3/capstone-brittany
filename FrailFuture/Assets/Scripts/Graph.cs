using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Graph
{
    //using Adjacency Matrix

    private int[,] matrix;
    private int[] marks; // per vertex not per edge

    public Graph(int numVerts)
    {
        matrix  = new int[numVerts , numVerts];
        marks = new int[numVerts];
    }

    public int vCount()
    {
        return marks.Length;
    }

    public int first(int vertex)
    {
        return next(vertex, -1);
    }

    //linear Theta( v);  for each verticies
    // to do BFS or DFS it would be Theta(v^2)
    public int next(int vertex, int lastVisitedNeighbor)
    {
        for (int i = lastVisitedNeighbor + 1; i < vCount(); i++)
        {
            if (matrix[vertex,i] != 0)
            {
                return i;
            }
        }
        //didnt find something returning size of verts
        return vCount();
    }

    public List<int> findAllNeighbors(int vertex)
    {
        bool foundAllNeighbors = false;
        int lastNeighbor = -1;
        List<int> neighbors = new List<int>();

        while (!foundAllNeighbors)
        {
            lastNeighbor = next(vertex, lastNeighbor);
            if (lastNeighbor != vCount())
            {
                neighbors.Add(lastNeighbor);
            }
            else
            {
                foundAllNeighbors = true;
            }
        }
        return neighbors;
    }

    public void addEdge(int vertex, int neighbor, int weight = 1)
    {
        //if adding weight 0, complain
        if ((vertex >= 0 && vertex < matrix.Length) && (neighbor >= 0 && neighbor < matrix.Length))
        {
            matrix[vertex,neighbor] = weight;
        }
    }

    public int getEdge(int vertex, int neighbor)
    {
        return matrix[vertex, neighbor];
    }

    public void removeEdge(int vertex, int neighbor)
    {
        matrix[vertex,neighbor] = 0;
    }

    public bool isEdge(int vertex, int neighbor)
    {
        return matrix[vertex,neighbor] != 0;
    }

    public int getMark(int vertex)
    {
        return marks[vertex];
    }

    public void setMark(int vertex, int mark)
    {
        marks[vertex] = mark;
    }

    // returns how many edges depart from vertex v
    int degree(int v)
    {
        return findAllNeighbors(v).Count;
    }

}
