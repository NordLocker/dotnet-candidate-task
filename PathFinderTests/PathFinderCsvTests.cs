using PathFinder;

namespace PathFinderTests;


[TestFixture]
public class PathFinderCsvTests
{
    private string _ltPath = @"Klaipeda,Kaunas,2:00
Vilnius,Kaunas,1:00
Kaunas,Ukmerge,1:00
Vilnius,Ukmerge,1:00
Klaipeda,Panevezys,3:00
Vilnius,Plunge,3:15
Panevezys,Vilnius,2:00";

    [TestCase("Vilnius", "Kaunas", 150, "Vilnius,Ukmerge,Kaunas")]
    [TestCase("Kaunas", "Vilnius", 150, "Kaunas,Ukmerge,Vilnius")]
    [TestCase("Vilnius", "Plunge", 195, "Vilnius,Plunge")]
    [TestCase("Vilnius", "Klaipeda", 210, "Vilnius,Kaunas,Klaipeda")]
    [TestCase("Vilnius", "Ukmerge", 570, "Vilnius,Panevezys,Klaipeda,Kaunas,Ukmerge")]
    public void FindFastestPath_LTPathArray_Success(string from, string to, int expectedDurationMinutes, string expectedTrip)
    {
        // Act
        var finder = new PathFinderCsv();

        // Act
        var result = finder.FindFastestPath(from, to, _ltPath);

        // Arrange
        Assert.Multiple(() =>
        {
            Assert.That(result.Item1, Is.EqualTo(TimeSpan.FromMinutes(expectedDurationMinutes)));
            Assert.That(result.Item2, Is.EqualTo(expectedTrip));
        });
    }

    [Test]
    public void FindFastestPath_NotExistingDestination_ReturnEmptyResult()
    {
        // Act
        var finder = new PathFinderCsv();

        // Act
        var result = finder.FindFastestPath("Vilnius", "Siauliai", _ltPath);

        // Arrange
        Assert.Multiple(() =>
        {
            Assert.That(result.Item1, Is.EqualTo(TimeSpan.Zero));
            Assert.That(result.Item2, Is.EqualTo(string.Empty));
        });
    }

    [Test]
    public void FindFastestPath_InvalidCsv_ReturnEmptyResult()
    {
        // Act
        var finder = new PathFinderCsv();

        // Act
        var result = finder.FindFastestPath("Vilnius", "Siauliai", "Not valid csv");

        // Arrange
        Assert.Multiple(() =>
        {
            Assert.That(result.Item1, Is.EqualTo(TimeSpan.Zero));
            Assert.That(result.Item2, Is.EqualTo(string.Empty));
        });
    }
}
