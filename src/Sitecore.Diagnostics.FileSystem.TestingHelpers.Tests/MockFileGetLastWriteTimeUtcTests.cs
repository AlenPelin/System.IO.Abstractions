namespace Sitecore.Diagnostics.FileSystem.TestingHelpers.Tests
{
    using System;

    using NUnit.Framework;

    public class MockFileGetLastWriteTimeUtcTests
    {
        [TestCase(" ")]
        [TestCase("   ")]
        public void MockFile_GetLastWriteTimeUtc_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate action = () => fileSystem.Internals.File.GetLastWriteTimeUtc(path);

            // Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.That(exception.ParamName, Is.EqualTo("path"));
        }

        [Test]
        public void MockFile_GetLastWriteTimeUtc_ShouldReturnDefaultTimeIfFileDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var actualLastWriteTime = fileSystem.Internals.File.GetLastWriteTimeUtc(@"c:\does\not\exist.txt");

            // Assert
            Assert.AreEqual(new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc), actualLastWriteTime);
        }
    }
}