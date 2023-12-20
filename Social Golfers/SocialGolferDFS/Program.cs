namespace Social_Golfers_Dfs_Final_Solution
{

    class Program
    {
        public const int G = 8; //Number of groups.
        public const int P = 4; //Number of players per group.
        public const int N = G * P; //Number of Golfers.


        public static void Main(string[] args)
        {
            int[,] playerMatches = new int[N, N];
            List<List<int>> weeks = new();
            int MaxNumberOfWeek = (int)Math.Floor((double)(N - 1) / (P - 1));
            for (int week = 0; week < 5; week++)
            {
                Solution solution = FindSolutionPerWeek(playerMatches);
                if (solution != null)
                {
                    weeks.Add(solution.Week);
                    playerMatches = solution.PlayerMatches;
                }
                else
                {
                    Console.WriteLine("No solution found for week: " + (week + 1));
                    break;
                }

            }

            PrintSolution(weeks);

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

        private static void PrintSolution(List<List<int>> weeks)
        {
            for (int i = 0; i < weeks.Count; i++)
            {
                Console.WriteLine($"Week {i + 1}:");
                for (int j = 0; j < weeks[i].Count; j++)
                {
                    Console.WriteLine($"Player {j + 1} is in group {weeks[i][j]}");
                }
                Console.WriteLine();
            }
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
            public const int G = 8;
            public const int P = 4;
            public const int N = G * P;

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

                return true;
            }

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