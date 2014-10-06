using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TopologicalSort
{
    DfsGraphTraversal DFS;
    public TopologicalSort()
    {
        DFS = new DfsGraphTraversal();
    }
   public List<int> sort(Graph g)
    {
        return sortWithDFS(g);
    }

    List<int> sortWithDFS(Graph g)
    {
        List<List<int>> backwardsSort = DFS.traverse(g);
        List<int> sorted = new List<int>();

        for(int i = backwardsSort.Count; i >= 0 ; i--)
        {
            for( int j = backwardsSort[i].Count-1; j >= 0  ; j--)
            {
                sorted.Add(backwardsSort[i][j]);
            }
        }
        return sorted;
    }

    List<int> sortWithSourceRemoval(Graph g)
    {
        return null;
    }

    List<int> sourceRemovalHelper( Graph g, int[] vertsLeft)
    {
       return null;
    }
    }