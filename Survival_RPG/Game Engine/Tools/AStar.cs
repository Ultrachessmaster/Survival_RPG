using System;
using System.Collections.Generic;
using System.Linq;
using Priority_Queue;

namespace Engine
{
    class AStar
    {
        private class Node : FastPriorityQueueNode
        {
            public XYZ pos;
            public bool walkable;
            public Node parent;
            public int cost;
            public Node (XYZ pos, bool walkable)
            {
                this.pos = pos;
                this.walkable = walkable;
            }
        }
        Node pos;
        Node dest;
        bool[,] map;

        public AStar(XYZ pos, XYZ dest, bool[,] map)
        {
            this.pos = new Node(pos, true);
            this.pos.cost = 0;
            this.dest = new Node(dest, true);
            this.map = map;
        }

        public List<XYZ> CalculatePath()
        {
            if (pos.pos.Equals(dest.pos))
                return new List<XYZ>();
            FastPriorityQueue<Node> frontier = new FastPriorityQueue<Node>((map.GetUpperBound(0) + 1) * (map.GetUpperBound(1) + 1));
            List<Node> beenthere = new List<Node>();
            frontier.Enqueue(pos, 0);
            bool atend = false;
            int i = 0;
            while (frontier.Count != 0)
            {
                i++;
                var current = frontier.Dequeue();
                var neighbors = Neighbors(map, current);
                for (int j = 0; j < neighbors.Length; j++)
                {
                    var next = neighbors[j];
                    var cost = current.cost + 1;
                    if (next == null)
                        continue;
                    if (next.pos.Equals(dest.pos))
                    {
                        atend = true;
                        dest.parent = current;
                    }

                    if (!next.walkable || atend)
                        continue;

                    bool partofbeenthere = false;
                    for (int bf = 0; bf < beenthere.Count; bf++)
                    {
                        var b = beenthere[bf];
                        if (b.pos.Equals(next.pos))
                        {
                            partofbeenthere = true;
                            break;
                        }
                    }
                    if (!partofbeenthere || cost < current.cost)
                    {
                        next.cost = cost;
                        next.parent = current;
                        var minpriority = cost + ManhattanDistance(next, dest);
                        frontier.Enqueue(next, minpriority);
                        if (!partofbeenthere)
                        {
                            beenthere.Add(next);
                        }
                    }
                }
            }

            Node n = dest;
            List<XYZ> path = new List<XYZ>();
            while (!n.Equals(pos))
            {
                path.Add(n.pos);
                n = n.parent;
            }
            path.Reverse();
            return path;
        }

        private int ManhattanDistance(Node node, Node goal)
        {
            return Math.Abs(goal.pos.X - node.pos.X) + Math.Abs(goal.pos.Y - node.pos.Y);
        }

        private Node[] Neighbors(bool[,] map, Node n)
        {
            Node[] neighbors = new Node[4];
            int[,] offsets = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };
            for(int i = 0; i < 4; i++)
            {
                XYZ p = new XYZ(n.pos.X + offsets[i, 0], n.pos.Y + offsets[i, 1]);
                if (p.X >= 0 && p.Y >= 0 && p.X < map.GetUpperBound(0) + 1 && p.Y < map.GetUpperBound(1) + 1)
                {
                    bool walkable = map[p.X, p.Y];
                    neighbors[i] = new Node(p, walkable);
                }
            }
            return neighbors;
        }
    }

    
}
