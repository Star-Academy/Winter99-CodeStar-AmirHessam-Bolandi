using System.Collections.Generic;
using System;
using Xunit;

namespace library.Test
{
    public class ResultTest
    {
        private Result query, word, empty;

        public ResultTest()
        {
            Entry[] entryList = { new Entry("test1", 11), new Entry("test2", 14) };
            this.query = new Result(new HashSet<Entry>(entryList), true);
            this.word = new Result(new HashSet<Entry>(entryList), false);
            this.empty = new Result(new HashSet<Entry>(), false);
        }

        [Fact]
        public void ToStringTest()
        {
            Assert.True(query.ToString() == " test1 test2" || query.ToString() == " test2 test1");
            Assert.True(word.ToString() == "Document Id: test1, Index: 11, Document Id: test2, Index: 14"
                         || word.ToString() == "[Document Id: test2, Index: 14, Document Id: test1, Index: 11]");
            Assert.True(empty.ToString() == "No result found");
        }
    }
}