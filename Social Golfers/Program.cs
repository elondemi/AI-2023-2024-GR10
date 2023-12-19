namespace Social.Golfers.Dfs
{

    class Program
    {
        public const int G = 8; //Number of groups.
        public const int P = 4; //Number of players per group.
        public const int N = G * P; //Number of Golfers.


        public static void Main(string[] args)
        {

            Node n1 = new(new List<int>() { 1, 2, 3, 4 });
            Node n2 = new(new List<int>() { 5, 6, 7, 8 });
            Node n3 = new(new List<int>() { 1, 2, 4, 5 });

            List<Node> nodes = new() { n1, n2, n3 };
            Node n4 = new(new List<int>() { 1, 2, 3, 4 });

            bool x = ListContains(n4, nodes);

            int[,] playerMatches = new int[N, N];
            List<List<int>> weeks = new();
            int MaxNumberOfWeek = (int)Math.Floor((double)(N - 1) / (P - 1));
            for (int i = 0; i < 5; i++)
            {
                Solution sol = FindSolutionPerWeek(playerMatches);
                weeks.Add(sol.Week);
                playerMatches = sol.PlayerMatches;

            }

            for (int i = 0; i < weeks.Count; i++)
            {
                Console.WriteLine($"Week {i + 1}");
                for (int j = 0; j < weeks[i].Count; j++)
                {
                    Console.WriteLine($"Player {j} is in group {weeks[i][j]}");
                }
                Console.WriteLine();
                //for (int j = 0; j < weeks[i].Count; j++)
                //{
                //    Console.WriteLine($"Player {weeks[i][j].Id} is in group {weeks[i][j].Group}");
                //}
                //Console.WriteLine();
            }

        }

        public static Solution FindSolutionPerWeek(int[,] playerMatches)
        {

            List<int> week = new();
            for (int i = 0; i < N; i++)
            {
                week.Add(0);
            }


            Node node = new Node(week);

            Stack<Node> OpenList = new();
            OpenList.Push(node);
            List<Node> ClosedList = new();
            Node Solution = new();
            while (OpenList.Count > 0)
            {
                Node currentNode = OpenList.Pop();
                ClosedList.Add(currentNode);
                if (currentNode.isGoal())
                {
                    Solution = currentNode;
                    break;
                }

                List<Node> children = currentNode.GetChildren(playerMatches);
                foreach (var child in children)
                {
                    if (!ListContains(child, OpenList.ToList()) && !ListContains(child, ClosedList))
                        OpenList.Push(child);
                }

            }

            List<int> weekSolution = Solution.Week;

            for (int i = 0; i < weekSolution.Count; i++)
            {
                for (int j = 0; j < weekSolution.Count; j++)
                {
                    if (i != j && weekSolution[i] == weekSolution[j])
                    {
                        playerMatches[i, j] = 1;
                    }
                }
            }

            return new Solution()
            {
                PlayerMatches = playerMatches,
                Week = Solution.Week
            };



        }

        public static bool ListContains(Node node, List<Node> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Equals(node))
                    return true;
            }

            return false;
        }

        public class Solution
        {
            public List<int> Week { get; set; }
            public int[,] PlayerMatches { get; set; }
        }

        public class Node
        {
            public const int G = 8; //Number of groups.
            public const int P = 4; //Number of players per group.
            public const int N = G * P; //Number of Golfers.

            public Node()
            {

            }
            public Node(List<int> newWeek)
            {
                Week = newWeek;
            }

            public List<int> Week { get; set; }

            public bool isGoal() => !Week.Any(x => x == 0);

            public List<Node> GetChildren(int[,] playerMatches)
            {
                List<Node> children = new();
                for (int i = 0; i < Week.Count; i++)
                {
                    if (!(Week[i] == 0))
                        continue;

                    for (int g = G; g >= 1; g--)
                    {
                        //if (playerMatches[i, g] == 1)
                        //    continue;

                        List<int> newWeek = new(Week)
                        {
                            [i] = g
                        };
                        if (newWeek.Where(x => x == g).Count() > P)
                            continue;

                        if (!ValidGroup(i, playerMatches, newWeek))
                            continue;

                        children.Add(new Node(newWeek));
                    }

                    return children;
                }

                return children;
            }

            public bool ValidGroup(int player, int[,] playerMatches, List<int> newWeek)
            {
                int group = newWeek[player];
                List<int> otherPlayersInGroup = new();
                for (int i = 0; i < newWeek.Count; i++)
                {
                    if (i != player && newWeek[i] == group && newWeek[i] != 0)
                    {
                        otherPlayersInGroup.Add(i);
                    }
                }


                for (int j = 0; j < otherPlayersInGroup.Count; j++)
                {
                    if (playerMatches[player, otherPlayersInGroup[j]] == 1)
                        return false;
                }
                //{
                //    if (playerMatches[player, j] == 1)
                //        return false;
                //}

                //if (playerMatches[i, player] == 1)
                //{
                //    if (newWeek[i] == 0)
                //        return false;
                //}


                return true;
            }
            //Override the Equals
            public bool Equals(object obj)
            {
                Node objSud = (Node)obj;
                int counter = 0;
                for (int i = 0; i < Week.Count; i++)
                {
                    if (Week[i] == objSud.Week[i])
                        counter++;
                }

                return counter == Week.Count;
            }


        }
    }

}