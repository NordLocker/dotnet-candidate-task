namespace PathFinder;

public abstract class PathFinderBase : IPathFinder
{
    private const int AdditionalTravelTimeMinutes = 30;

    public (TimeSpan, string) FindFastestPath(string from, string to, string pathsData)
    {
        var existingPaths = ParseExistingPaths(pathsData);

        if (!existingPaths.ContainsKey(from) || !existingPaths.ContainsKey(to))
        {
            return (TimeSpan.Zero, string.Empty);
        }

        var shortestPath = (TimeSpan.MaxValue, string.Empty);
        FindShortestPath(from, to, new List<string>(), TimeSpan.Zero, existingPaths, ref shortestPath);

        return shortestPath;
    }

    private static void FindShortestPath(string current, string destination, ICollection<string> visited, TimeSpan currentTravelTime, Dictionary<string, Dictionary<string, TimeSpan>> existingPaths, ref (TimeSpan, string) shortestPath)
    {
        visited.Add(current);

        foreach (var nextCity in existingPaths[current])
        {
            if (visited.Contains(nextCity.Key))
            {
                continue;
            }
            var timeToNextCity = existingPaths[current][nextCity.Key];

            var totalDuration = currentTravelTime + timeToNextCity;

            if (visited.Count > 1)
            {
                var fromMinutes = TimeSpan.FromMinutes(AdditionalTravelTimeMinutes);
                totalDuration += fromMinutes;
            }

            if (nextCity.Key == destination)
            {
                if (totalDuration < shortestPath.Item1 || shortestPath.Item1.Minutes == 0)
                {
                    shortestPath = (totalDuration, string.Join(",", visited) + "," + destination);
                }
            }
            else
            {
                FindShortestPath(nextCity.Key, destination, new List<string>(visited), totalDuration, existingPaths, ref shortestPath);
            }
        }
    }

    protected abstract Dictionary<string, Dictionary<string, TimeSpan>> ParseExistingPaths(string pathsData);

    protected static void AddPath(IDictionary<string, Dictionary<string, TimeSpan>> parsedPaths, string from, string to, TimeSpan time)
    {
        if (!parsedPaths.ContainsKey(from))
        {
            parsedPaths[from] = new Dictionary<string, TimeSpan>();
        }

        parsedPaths[from][to] = time;

        if (!parsedPaths.ContainsKey(to))
        {
            parsedPaths[to] = new Dictionary<string, TimeSpan>();
        }

        parsedPaths[to][from] = time;
    }
}