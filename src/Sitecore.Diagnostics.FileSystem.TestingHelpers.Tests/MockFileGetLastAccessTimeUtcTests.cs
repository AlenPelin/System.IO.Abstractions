namespace Sitecore.Diagnostics.FileSystem.TestingHelpers.Tests
{
    using System;

    using NUnit.Framework;

    [TestFixture]
    public class MockFileGetLastAccessTimeUtcTests
    {
        [TestCase(" ")]
        [TestCase("   ")]
        public void MockFile_GetLastAccessTimeUtc_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate action = () => fileSystem.Internals.File.GetLastAccessTimeUtc(path);

            // Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.That(exception.ParamName, Is.EqualTo("path"));
        }

        [Test]
        public void MockFile_GetLastAccessTimeUtc_ShouldReturnDefaultTimeIfFileDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var actualLastAccessTime = fileSystem.Internals.File.GetLastAccessTimeUtc(@"c:\does\not\exist.txt");

            // Assert
            Assert.AreEqual(new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc), actualLastAccessTime);
        }
    }
}