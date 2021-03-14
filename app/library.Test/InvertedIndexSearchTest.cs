using System.Linq;
using System.Collections.Generic;
using Moq;
using Xunit;

namespace library.Test
{
    public class InvertedIndexSearchTest
    {
        private InvertedIndexSearch invertedIndexSearch;
        private static readonly HashSet<Entry> query_test_result = new HashSet<Entry> { new Entry("testData1.txt", 1), new Entry("testData2.txt", 2) };
        private static readonly HashSet<Entry> word_test_result = new HashSet<Entry> { new Entry("testData1.txt", 1) };
        public InvertedIndexSearchTest()
        {
            var contents = new Dictionary<string, string>();
            contents.Add("testData1.txt", "hello hello world..");
            contents.Add("testData2.txt", "foo?  i bars!!! him");
            contents.Add("testData3.txt", "Freedom is only a hallucination.");
            this.invertedIndexSearch = new InvertedIndexSearch(contents);
        }

        [Fact]
        public void AddWordsTest()
        {
            string[] listOfWords = { "hi", "bye", "serious", "hmm" };
            invertedIndexSearch.AddWords("testId", listOfWords);
            foreach (string word in listOfWords)
            {
                Assert.True(this.invertedIndexSearch.IndexMap.ContainsKey(word));
            }
        }

        [Fact]
        public void GetListOfWordsTest()
        {
            Assert.Equal(this.invertedIndexSearch.GetListOfWords("$234 hell*garden*33 why %#REZA"),
                    new string[] { "234", "hell", "garden", "33", "why", "reza" });
        }

        [Fact]
        public void SeperateQueryTest()
        {
            string inputStr = " hello +ali -go +majid +r";
            Assert.Equal(invertedIndexSearch.SeperateQuery(inputStr, @"\+(\w+)"),
                    new List<string> { "ali", "majid", "r" });
            Assert.Equal(invertedIndexSearch.SeperateQuery(inputStr, @"\-(\w+)"), new List<string> { "go" });
            Assert.Equal(invertedIndexSearch.SeperateQuery(inputStr, @"\s(\w+)"), new List<string> { "hello" });
        }

        [Fact]
        public void CombineResutTest()
        {
            var normalSet = new HashSet<Entry> { new Entry("t1", 1), new Entry("t2", 2) };
            var plusSet = new HashSet<Entry> { new Entry("t3", 1), new Entry("t2", 2) };
            var minusSet = new HashSet<Entry> { new Entry("t2", 1), new Entry("t5", 2) };
            var resultSet = new HashSet<Entry> { new Entry("t1", 1), new Entry("t3", 1) };
            Assert.Equal(resultSet, invertedIndexSearch.CombineResults(normalSet, plusSet, minusSet));
        }

        [Fact]
        public void SearchWordTest()
        {
            Assert.Equal(invertedIndexSearch.SearchWord("hello"), word_test_result);
        }

        [Fact]
        public void SearchQueryTest()
        {
            Assert.Equal(invertedIndexSearch.SearchQuery("hello +bar"), query_test_result);
        }

        [Fact]
        public void SearchTest()
        {
            var result1 = new Mock<Result>();  
            var result2 = new Mock<Result>();  
            result1.SetupGet(x => x.ResultSet).Returns(word_test_result);  
            result2.SetupGet(x => x.ResultSet).Returns(query_test_result);  
            Assert.Equal(invertedIndexSearch.Search("hello").ResultSet, (result1.Object.ResultSet));
            Assert.Equal(invertedIndexSearch.Search("hello +bars -doesntExist").ResultSet, (result2.Object.ResultSet));
        }
    }
}