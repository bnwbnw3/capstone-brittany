using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AiTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestPicksCorrectPath()
        {
            Graph test = new Graph(11);
            test.addEdge(0,1,1);
            test.addEdge(0,2,1);
            test.addEdge(1,3,1);
            test.addEdge(1,4,1);
            test.addEdge(1,5,1);
            test.addEdge(2,5,1);
            test.addEdge(2,6,1);
            test.addEdge(3,7,1);
            test.addEdge(3,8,1);
            test.addEdge(4,8,1);
            test.addEdge(4,9,1);
            test.addEdge(5,9,1);
            test.addEdge(5,10,1);
            test.addEdge(6,9,1);
            test.addEdge(6,10,1);

            int desired = 9;

            PathWayFinder pf = new PathWayFinder(test,7);
            Node next = new Node() { input = 0, vertex = 0 };
            while (next.vertex != desired)
            {
                next = pf.getNextDesiredInput(next.input, desired);
                
            }
        }

        [TestMethod]
        public void TestPicksRandomPath()
        {
            Graph test = new Graph(11);
            test.addEdge(0,1,1);
            test.addEdge(0,2,1);
            test.addEdge(1,3,1);
            test.addEdge(1,4,1);
            test.addEdge(1,5,1);
            test.addEdge(2,5,1);
            test.addEdge(2,6,1);
            test.addEdge(3,7,1);
            test.addEdge(3,8,1);
            test.addEdge(4,8,1);
            test.addEdge(4,9,1);
            test.addEdge(5,9,1);
            test.addEdge(5,10,1);
            test.addEdge(6,9,1);
            test.addEdge(6,10,1);

            int desired = new Random().Next(7,11);
            desired = 10;

            PathWayFinder pf = new PathWayFinder(test, 7);
            Node next = new Node() { input = 0, vertex = 0 };
            while (pf.isEndOfPath() != true && next.vertex != desired)
            {
                next = pf.getNextDesiredInput(new Random().Next(1,3), desired);
                
            }
        }

        [TestMethod]
        public void TestNotPickCorrect()
        {
            Graph test = new Graph(11);
            test.addEdge(0, 1, 1);
            test.addEdge(0, 2, 1);
            test.addEdge(1, 3, 1);
            test.addEdge(1, 4, 1);
            test.addEdge(1, 5, 1);
            test.addEdge(2, 5, 1);
            test.addEdge(2, 6, 1);
            test.addEdge(3, 7, 1);
            test.addEdge(3, 8, 1);
            test.addEdge(4, 8, 1);
            test.addEdge(4, 9, 1);
            test.addEdge(5, 9, 1);
            test.addEdge(5, 10, 1);
            test.addEdge(6, 9, 1);
            test.addEdge(6, 10, 1);

            int desired = 8;

            PathWayFinder pf = new PathWayFinder(test,7);
            List<int> compSays = new List<int>();
            compSays.Add(pf.getNextDesiredInput(0, desired).vertex);
            compSays.Add(pf.getNextDesiredInput(1, desired).vertex);
            compSays.Add(pf.getNextDesiredInput(3, desired).vertex);
            compSays.Add(pf.getNextDesiredInput(2, desired).vertex);
            
        }
    }
}
