namespace PathFinder;

public class PathFinderCsv : PathFinderBase
{

    private const string Separator = ",";

    protected override Dictionary<string, Dictionary<string, TimeSpan>> ParseExistingPaths(string pathsData)
    {
        var parsedPaths = new Dictionary<string, Dictionary<string, TimeSpan>>();

        var reader = new StringReader(pathsData);
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            var parts = line.Split(Separator);

            if (parts.Length != 3 || !TimeSpan.TryParse(parts[2], out var time))
            {
                continue;
            }

            var from = parts[0];
            var to = parts[1];
            AddPath(parsedPaths, from, to, time);
        }

        return parsedPaths;
    }
}