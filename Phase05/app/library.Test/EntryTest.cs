using Xunit;
using System;

namespace library.Test
{
    public class EntryTest
    {
        private Entry entry;

        public EntryTest()
        {
            this.entry = new Entry("test", 11);
        }

        [Fact]
        public void GetIndexTest()
        {
            Assert.Equal(11, entry.Index);
        }

        [Fact]
        public void ToStringTest()
        {
            Assert.Equal("Document Id: test, Index: 11", entry.ToString());
        }

        [Theory]
        [InlineData("test", 11)]
        [InlineData("test", 12)]
        public void EqualsTestTrue(string docName, int index)
        {
            Assert.True(this.entry.Equals(new Entry(docName, index)));
        }

        [Theory]
        [InlineData("wrong test", 11)]
        [InlineData("wrong test", 12)]
        public void EqualsTestFalse(string docName, int index)
        {
            Assert.False(this.entry.Equals(new Entry(docName, index)));
        }

        [Fact]
        public void EqualsTestFalseDifferentType()
        {
            Assert.False(this.entry.Equals(new object()));
            Assert.False(this.entry.Equals("test"));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            Assert.Equal( "test".GetHashCode(), entry.GetHashCode());
        }
    }
}
