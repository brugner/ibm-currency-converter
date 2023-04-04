namespace IBM.Application.DSA;

public class BreadthFirstSearch
{
    /// <summary>
    /// Find the shortest path between to nodes. More info on https://www.koderdojo.com/blog/breadth-first-search-and-shortest-path-in-csharp-and-net-core
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="graph"></param>
    /// <param name="start"></param>
    /// <returns></returns>
    public static Func<T, IEnumerable<T>> ShortestPathFunction<T>(Graph<T> graph, T start) where T : notnull
    {
        var previous = new Dictionary<T, T>();

        var queue = new Queue<T>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var vertex = queue.Dequeue();

            foreach (var neighbor in graph.AdjacencyList[vertex])
            {
                if (previous.ContainsKey(neighbor))
                    continue;

                previous[neighbor] = vertex;
                queue.Enqueue(neighbor);
            }
        }

        IEnumerable<T> shortestPath(T v)
        {
            var path = new List<T> { };
            var current = v;

            while (!current.Equals(start))
            {
                path.Add(current);
                current = previous[current];
            };

            path.Add(start);
            path.Reverse();

            return path;
        }

        return shortestPath;
    }
}