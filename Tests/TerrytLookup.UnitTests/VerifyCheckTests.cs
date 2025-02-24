namespace TerrytLookup.UnitTests;

[TestFixture]
public class VerifyCheckTests
{
    [Test]
    public static Task VerifyCheck()
    {
        return VerifyChecks.Run();
    }
}