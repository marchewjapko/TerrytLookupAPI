namespace TerrytLookup.Tests;

public class VerifyCheckTests
{
    [Test]
    public Task VerifyCheck()
    {
        return VerifyChecks.Run();
    }
}