using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BfsGraphTraversal
{
    const int VISITED = 1;
    const int UNVISITED = 0;
    // The inner List represents a traversal of a connected component in the graph.
    // The outer list represents all the connected components.
   public List<List<int>> traverse(Graph g)
   {
       List<List<int>> ret = new List<List<int>>();

       for(int vertex = 0; vertex < g.vCount(); vertex++)
       {
           if(g.getMark(vertex) == UNVISITED)
           {
               List<int> currentTraversal = new List<int>();

               //Visit
               Queue<int> queue = new Queue<int>();
               //add vertex
               queue.Enqueue(vertex);

               while(queue.Count > 0)
               {
                   int currentVert = queue.Dequeue();
                   if(g.getMark(currentVert) == UNVISITED)
                   {
                       //pre visit
                       g.setMark(currentVert, VISITED);

                        //add neighbors
                       for (int w = g.first(currentVert); w < g.vCount() ; w = g.next(currentVert, w))
                       {
                           if (g.getMark(w) == UNVISITED)
                           {
                               queue.Enqueue(w);
                           }
                       }
                       //post visit
                       currentTraversal.Add(currentVert);
                   }
               }

               ret.Add(currentTraversal);
           }
       }
       return ret;
   }
}