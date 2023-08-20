namespace PathFinder;
public interface IPathFinder
{
    (TimeSpan, string) FindFastestPath(string from, string to, string pathsData);
}
