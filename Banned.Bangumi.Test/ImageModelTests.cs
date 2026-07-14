using Banned.Bangumi.Models.Common;
using System.Text.Json;

namespace Banned.Bangumi.Test;

[TestFixture]
public sealed class ImageModelTests
{
    [Test]
    public void Images_DeserializesReturnedSubsetAndLeavesMissingSizesNull()
    {
        const string json = """
            {"large":"large.jpg","medium":"medium.jpg","small":"small.jpg"}
            """;

        var result = JsonSerializer.Deserialize<Images>(json);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result!.Large, Is.EqualTo("large.jpg"));
            Assert.That(result.Medium, Is.EqualTo("medium.jpg"));
            Assert.That(result.Small, Is.EqualTo("small.jpg"));
            Assert.That(result.Grid, Is.Null);
            Assert.That(result.Common, Is.Null);
        });
    }
}
