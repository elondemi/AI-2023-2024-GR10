namespace Social_Golfers_Bfs_Final_Solution
{
    /* The social golfers problem is a very hard combinatorial optimization problem, 
     * and BFS and backtracking are not very efficient methods for solving it. 
     * The number of nodes in the search tree grows exponentially with the number of weeks, 
     * and the queue can quickly become very large and consume a lot of memory. 
     * Moreover, the backtracking check based on the visited set may not be enough to avoid exploring redundant or infeasible nodes,
     * as there may be a lot of symmetry and constraints in the problem.*/
    class Program
    {
        public const int G = 8; // Number of groups
        public const int P = 4; // Number of players per group
        public const int N = G * P; // Total number of golfers

        public static void Main(string[] args)
        {
            int[,] playerMatches = new int[N, N];
            List<List<int>> weeks = new List<List<int>>();
            int MaxNumberOfWeek = (int)Math.Floor((double)(N - 1) / (P - 1));
            for (int week = 0; week < 5; week++) // Number of weeks can be adjusted
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
            Node startNode = new Node(new List<int>(new int[N]), G, P, playerMatches);
            Queue<Node> queue = new Queue<Node>();
            HashSet<Node> visited = new HashSet<Node>();
            queue.Enqueue(startNode);

            while (queue.Count > 0)
            {
                Node currentNode = queue.Dequeue();
                if (currentNode.IsGoal())
                {
                    return new Solution
                    {
                        Week = currentNode.Week,
                        PlayerMatches = currentNode.PlayerMatches
                    };
                }

                visited.Add(currentNode); // Add to visited to prevent immediate revisits

                foreach (Node child in currentNode.GetChildren())
                {
                    if (!visited.Contains(child)) // Basic backtracking check
                    {
                        queue.Enqueue(child);
                    }
                }
            }

            return null;
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

    }

    public class Node
    {
        public List<int> Week { get; }
        public int[,] PlayerMatches { get; }
        private readonly int G;
        private readonly int P;

        public Node(List<int> week, int G, int P, int[,] playerMatches)
        {
            Week = week;
            this.G = G;
            this.P = P;
            PlayerMatches = playerMatches;
        }

        public bool IsGoal()
        {
            return Week.All(groupNumber => groupNumber != 0);
        }

        public List<Node> GetChildren()
        {
            List<Node> children = new List<Node>();
            for (int i = 0; i < Week.Count; i++)
            {
                if (Week[i] == 0)
                {
                    for (int group = 1; group <= G; group++)
                    {
                        if (IsValidGroup(i, group))
                        {
                            List<int> newWeek = new List<int>(Week);
                            newWeek[i] = group;
                            children.Add(new Node(newWeek, G, P, PlayerMatches));
                        }
                    }
                    break;
                }
            }
            return children;
        }

        private bool IsValidGroup(int player, int group)
        {
            int groupCount = Week.Count(g => g == group);
            if (groupCount >= P)
            {
                return false;
            }

            for (int i = 0; i < Week.Count; i++)
            {
                if (Week[i] == group && PlayerMatches[player, i] == 1)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class Solution
    {
        public List<int> Week { get; set; }
        public int[,] PlayerMatches { get; set; }
    }
}
