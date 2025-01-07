using Microsoft.AspNetCore.Http;
using TerrytLookup.Infrastructure.Models.Dto.Terryt;
using TerrytLookup.Infrastructure.Services.FeedDataService.TerrytReader;

namespace TerrytLookup.Tests.ServiceTests.FeedDataServiceTests;

public class TerrytReaderTests
{
    private const string FileName = "TERC_TestFile.csv";
    private static readonly TerrytReader TerrytReader = new();
    private static readonly string FilePath = Path.Combine(Environment.CurrentDirectory, FileName);

    [SetUp]
    public void Setup()
    {
        using var outputFile = new StreamWriter(FilePath);
        foreach (var line in TercTestFileContent.Content.Split("\r\n"))
            outputFile.WriteLine(line);

        Assume.That(File.Exists(FilePath));
    }

    [TearDown]
    public void TearDown()
    {
        try
        {
            File.Delete(FilePath);
        }
        catch
        {
            // ignored
        }
    }

    [Test]
    public async Task ReadAsync_ShouldReadFile()
    {
        //Arrange
        await using var stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
        var formFile = new FormFile(stream, 0, stream.Length, FileName, FilePath);

        //Act
        var result = await TerrytReader.ReadAsync<TercDto>(formFile);

        //Assert
        Assert.That(result, Has.Count.EqualTo(100));
    }
}