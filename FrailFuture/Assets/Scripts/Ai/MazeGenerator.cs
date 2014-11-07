using System;
using System.Collections.Generic;
using System.Linq;

class MazeGenerator
{
    private static System.Random rand;
    static public MazeGroup getAProcedurallyGenMaze()
    {
        int rowsToUse = 0;
        rand = new System.Random(System.DateTime.Now.GetHashCode());
        rowsToUse = rand.Next(GameControl.control.getMinRows(), GameControl.control.MaxNumberOfRows + 1);
        return generateRandomGraph(rowsToUse);
    }

    static private MazeGroup generateRandomGraph(int maxRow)
    {
        Graph mazeToReturn;
        int rowReachsEndNodeCount = GameControl.control.getNumMazeEndings() - 1;
        //the row where increaseing nodes in rows stops. From this row to the next row
        //until the max row decrease nodes. Highest Increase Point
        int Hip = rowReachsEndNodeCount + ((maxRow - rowReachsEndNodeCount) / 2);
        //find number of nodes for the graph
        int totalNodes = getTotalNodesFromRowAmount(maxRow, Hip);

        mazeToReturn = new Graph(totalNodes);

        //put in edges
        for (int row = 0; row < maxRow; row++)
        {
            if (row == 0)
            {
                mazeToReturn.addEdge(0, 1);
                mazeToReturn.addEdge(0, 2);
            }
            else
            {
                int[] fromNodes = getNodesInARow(row, Hip, maxRow);
                int[] toNodes = getNodesInARow(row + 1, Hip, maxRow);
                List<NodeAndNumConns> ToNodesGroup = toNodes.ToArray().Select(n => new NodeAndNumConns(n)).ToList();
                foreach(int fromVertex in fromNodes)
                {
                    int connections = rand.Next(GameControl.control.minNumChoices, GameControl.control.maxNumChoices + 1);
                    for (int i = 0; i < connections; i++)
                    {
                        //Grab the indexs from the toNodes not connection yet to the index in the fromNodes
                        List<NodeAndNumConns> potentialConnections = ToNodesGroup.Where(n => !(mazeToReturn.findAllNeighbors(fromVertex).Contains(n.nodeVertex))).ToList();
                        if (potentialConnections.Count() > 0)
                        {
                            int smallestTotalConnectionCount = potentialConnections.Min(n => n.numConnectionsTo);
                            //Grab all values that have the same smallest amount of connections
                            potentialConnections = potentialConnections.Where(n => n.numConnectionsTo == smallestTotalConnectionCount).ToList();
                            int randomIndexToUse = rand.Next(0, potentialConnections.Count());
                            int toNodeConnectionVertex = potentialConnections[randomIndexToUse].nodeVertex;
                            mazeToReturn.addEdge(fromVertex, toNodeConnectionVertex);
                        }
                    }
                }
            }
        }
        //put in endings
        Dictionary<NeutralityTypes, int> endIndexs = new Dictionary<NeutralityTypes, int>();
        endIndexs[NeutralityTypes.Evil] = totalNodes - 5;
        endIndexs[NeutralityTypes.Agitated] = totalNodes - 4;
        endIndexs[NeutralityTypes.Neutral] = totalNodes - 3;
        endIndexs[NeutralityTypes.Lovely] = totalNodes - 2;
        endIndexs[NeutralityTypes.Heavenly] = totalNodes - 1;

        return new MazeGroup() { maze = mazeToReturn, mazeEndIndexs = endIndexs };
    }
    static private int getTotalNodesFromRowAmount(int maxRow, int Hip)
    {
        int totalNodes = 0;
        int increaseAmount = 1;
        int decreaseAmount = 1;
        for (int rowLevel = 0; rowLevel <= maxRow; rowLevel++)
        {
            if (rowLevel <= Hip)
            {
                totalNodes += (rowLevel + increaseAmount);
            }
            else
            {
                int diffFromHip = rowLevel - Hip;
                totalNodes += (rowLevel + 1 - diffFromHip * 2);
                if (rowLevel == maxRow && rowLevel % 2 != 0)
                {
                    //if row is odd at the end the last row will not change from the row before it.
                    //re add in 1 to compensate.
                    totalNodes += 1;
                }
                decreaseAmount++;
            }
        }
        return totalNodes;
    }
    static private int[] getNodesInARow(int rowNum, int Hip, int maxRow)
    {
        int beginOfRow = 0;
        int endOfRow = 0;
        if (rowNum <= Hip)
        {
            beginOfRow = Tools.SummationDownFrom(rowNum);
            endOfRow = beginOfRow + rowNum;
        }
        else
        {
            int diffFromHip = rowNum - Hip;
            beginOfRow = Tools.SummationDownFrom(Hip) + Tools.SummationChangeBy(Hip + 1, diffFromHip, -1);
            endOfRow = beginOfRow + (rowNum - diffFromHip * 2);

            if (rowNum == maxRow && rowNum % 2 != 0)
            {
                //if row is odd at the end the last row will not change from the row before it.
                //re-add in 1 to compensate.
                endOfRow += 1;
            }
        }
        int[] nodesInRow = new int[(endOfRow - beginOfRow) + 1];
        //grab nodes in current row
        int count = 0;
        for (int j = beginOfRow; j <= endOfRow; j++)
        {
            nodesInRow[count++] = j;
        }
        return nodesInRow;
    }

    private class NodeAndNumConns
    {
        public int nodeVertex { get; set; }
        public int numConnectionsTo { get; set; }
        public NodeAndNumConns(int node) { nodeVertex = node; }
    }
}