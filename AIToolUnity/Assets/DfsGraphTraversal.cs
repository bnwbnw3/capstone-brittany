using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DfsGraphTraversal
{
    const int VISITED = 1;
    const int UNVISITED = 0;

   // This class performs a traversal of a Graph and emits the visit order of the graph’s vertices.

    // The outer list represents all the connected components.
   public List<List<int>> traverse(Graph g)
   {
       List<List<int>> ret = new List<List<int>>();

       for(int vertex = 0; vertex < g.vCount(); vertex++)
       {
           if(g.getMark(vertex) == UNVISITED)
           {
               List<int> currentTraversal = new List<int>();
               traverseHelper(currentTraversal, g, vertex);
               ret.Add(currentTraversal);
           }
       }
       return ret;
   }

    // The inner List represents a traversal of a connected component in the graph.
    private void traverseHelper(List<int> aConnectedComponent, Graph g, int vertex)
    {
        //PreVisit
        g.setMark(vertex, VISITED);
        for (int w = g.first(vertex); w < g.vCount() ; w = g.next(vertex, w))
        {
            if (g.getMark(w) == UNVISITED)
            {
                traverseHelper(aConnectedComponent, g, w);
            }
        }
        //PostVisit
        aConnectedComponent.Add(vertex);
    }
}