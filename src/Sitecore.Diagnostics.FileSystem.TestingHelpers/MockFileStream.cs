namespace Sitecore.Diagnostics.FileSystem.TestingHelpers
{
    using System;
    using System.IO;

    [Serializable]
    public class MockFileStream : MemoryStream
    {
        private readonly IMockFileDataAccessor mockFileDataAccessor;
        private readonly string path;

        public enum StreamType
        {
            READ,
            WRITE,
            APPEND
        }

        public MockFileStream(IMockFileDataAccessor mockFileDataAccessor, string path, StreamType streamType)
        {
            if (mockFileDataAccessor == null)
            {
                throw new ArgumentNullException("mockFileDataAccessor");
            }

            this.mockFileDataAccessor = mockFileDataAccessor;
            this.path = path;

            if (mockFileDataAccessor.FileExists(path))
            {
                /* only way to make an expandable MemoryStream that starts with a particular content */
                var data = mockFileDataAccessor.GetFile(path).Contents;
                if (data != null && data.Length > 0)
                {
                    Write(data, 0, data.Length);
                    Seek(0, StreamType.APPEND.Equals(streamType)
                        ? SeekOrigin.End
                        : SeekOrigin.Begin);
                }
            }
            else
            {
                if (StreamType.READ.Equals(streamType))
                {
                    throw new FileNotFoundException("File not found.", path);
                }
                mockFileDataAccessor.AddFile(path, new MockFileData(new byte[] { }));
            }
        }

        public override void Close()
        {
            InternalFlush();
        }

        public override void Flush()
        {
            InternalFlush();
        }

        private void InternalFlush()
        {
            if (mockFileDataAccessor.FileExists(path))
            {
                var mockFileData = mockFileDataAccessor.GetFile(path);
                /* reset back to the beginning .. */
                Seek(0, SeekOrigin.Begin);
                /* .. read everything out */
                var data = new byte[Length];
                Read(data, 0, (int) Length);
                /* .. put it in the mock system */
                mockFileData.Contents = data;
            }
        }
    }
}