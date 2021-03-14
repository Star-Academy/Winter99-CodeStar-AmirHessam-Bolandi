using System.Collections.Generic;
using Xunit;
using System.Linq;

namespace library.Test
{
    public class FileReaderTest
    {
        private FileReader fileReader;
        const string PATH = "..\\..\\..\\..\\data\\Test-Data";
        public FileReaderTest()
        {
            fileReader = new FileReader(PATH);
        }

        [Fact]
        public void ReadContentTest()
        {
            var contents = new Dictionary<string, string>();
            contents.Add("testData1.txt", "hello world..");
            contents.Add("testData2.txt", "foo? bar!!!");
            contents.Add("testData3.txt", "Freedom is only a hallucination.");
            Assert.True(contents.OrderBy(r => r.Key).SequenceEqual(this.fileReader.ReadContent().OrderBy(r => r.Key)));
        }
    }
}