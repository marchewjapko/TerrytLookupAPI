using DotNet.Testcontainers.Builders;

namespace TerrytLookup.IntegrationTests;

public class Tests
{
    private const string ContainerName = "terryt-lookup-api-web-api-integration-test";

    [Test]
    [Order(1)]
    public void ShouldBuildDockerImage()
    {
        //Arrange
        var image = new ImageFromDockerfileBuilder()
            .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), string.Empty)
            .WithCleanUp(true)
            .WithName(ContainerName)
            .Build();

        //Act
        image
            .CreateAsync()
            .Wait();

        //Arrange
        //Check if image has been created, how can I do that?
    }
}