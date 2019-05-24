using System.Collections.Generic;
using SnapshotableCollections;
using Xunit;
using Xunit.Abstractions;

namespace SnapshotableTest
{
    public class BalancedTreeTest
    {
        private readonly ITestOutputHelper _output;

        public BalancedTreeTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Insert1Item()
        {
            var t = new BalancedTree<int>();
            t.Insert("1", 1);
            _output.WriteLine(t.Tree());
        }

        [Fact]
        public void Insert2Items()
        {
            var t = new BalancedTree<int>();
            t.Insert("1", 1);
            _output.WriteLine(t.Tree());
            t.Insert("2", 2);
            _output.WriteLine(t.Tree());
        }

        [Fact]
        public void Insert3Items()
        {
            var t = new BalancedTree<int>();
            t.Insert("1", 1);
            _output.WriteLine(t.Tree());
            t.Insert("2", 2);
            _output.WriteLine(t.Tree());
            t.Insert("3", 3);
            _output.WriteLine(t.Tree());
        }

        [Fact]
        public void Insert3ItemsMixedOrder()
        {
            var t = new BalancedTree<int>();
            t.Insert("2", 2);
            _output.WriteLine(t.Tree());
            t.Insert("3", 3);
            _output.WriteLine(t.Tree());
            t.Insert("1", 1);
            _output.WriteLine(t.Tree());
        }

        [Fact]
        public void InsertReplaceItem()
        {
            var t = new BalancedTree<int>();
            t.Insert("1", 1);
            t.Insert("1", 2);
        }

        [Fact]
        public void Get()
        {
            var t = new BalancedTree<int>();
            t.Insert("1", 1);
            _output.WriteLine(t.Tree());
            var actual = t.Get("1");
            const int expected = 1;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetReplaced()
        {
            var t = new BalancedTree<int>();
            t.Insert("1", 1);
            _output.WriteLine(t.Tree());
            t.Insert("1", 2);
            _output.WriteLine(t.Tree());
            var actual = t.Get("1");
            const int expected = 2;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Get2Items()
        {
            var t = new BalancedTree<int>();
            t.Insert("1", 1);
            _output.WriteLine(t.Tree());
            t.Insert("2", 2);
            _output.WriteLine(t.Tree());
            var actual1 = t.Get("1");
            const int expected1 = 1;
            Assert.Equal(expected1, actual1);
            var actual2 = t.Get("2");
            const int expected2 = 2;
            Assert.Equal(expected2, actual2);
        }

        [Fact]
        public void Get3Items()
        {
            var t = new BalancedTree<int>();
            t.Insert("1", 1);
            _output.WriteLine(t.Tree());
            t.Insert("2", 2);
            _output.WriteLine(t.Tree());
            t.Insert("3", 3);
            _output.WriteLine(t.Tree());
            var actual1 = t.Get("1");
            const int expected1 = 1;
            Assert.Equal(expected1, actual1);
            var actual2 = t.Get("2");
            const int expected2 = 2;
            Assert.Equal(expected2, actual2);
            var actual3 = t.Get("3");
            const int expected3 = 3;
            Assert.Equal(expected3, actual3);
        }

        [Fact]
        public void GetWithDefault()
        {
            var t = new BalancedTree<int>();
            _output.WriteLine(t.Tree());
            var actual = t.Get("1", 2);
            const int expected = 2;
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void GetWithTry()
        {
            var t = new BalancedTree<int>();
            t.Insert("1", 1);
            _output.WriteLine(t.Tree());
            var actual = t.TryGet("1", out var result);
            const bool expected1 = true;
            Assert.Equal(expected1, actual);
            const int expected2 = 1;
            Assert.Equal(expected2, result);
        }

        [Fact]
        public void GetNotExistsWithTry()
        {
            var t = new BalancedTree<int>();
            _output.WriteLine(t.Tree());
            var actual = t.TryGet("1", out _);
            const bool expected = false;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Keys1Item()
        {
            var t = new BalancedTree<int>();
            t.Insert("1", 1);
            _output.WriteLine(t.Tree());
            var actual = t.Keys();
            Assert.Single(actual);
            Assert.Collection(actual, n => Assert.Equal("1", n));
        }

        [Fact]
        public void Keys1ItemAndReplace()
        {
            var t = new BalancedTree<int>();
            t.Insert("1", 1);
            _output.WriteLine(t.Tree());
            t.Insert("1", 2);
            _output.WriteLine(t.Tree());
            var actual = t.Keys();
            Assert.Single(actual);
            Assert.Collection(actual, n => Assert.Equal("1", n));
        }

        [Fact]
        public void Keys2Items()
        {
            var t = new BalancedTree<int>();
            t.Insert("1", 1);
            _output.WriteLine(t.Tree());
            t.Insert("2", 2);
            _output.WriteLine(t.Tree());
            var actual = t.Keys();
            Assert.Collection(actual, n => Assert.Equal("1", n), n => Assert.Equal("2", n));
        }

        [Fact]
        public void Keys3Items()
        {
            var t = new BalancedTree<int>();
            t.Insert("1", 1);
            _output.WriteLine(t.Tree());
            t.Insert("2", 2);
            _output.WriteLine(t.Tree());
            t.Insert("3", 3);
            _output.WriteLine(t.Tree());
            var actual = t.Keys();
            Assert.Collection(actual, 
                              n => Assert.Equal("1", n), 
                              n => Assert.Equal("2", n),
                              n => Assert.Equal("3", n)
                              );
        }


        [Fact]
        public void Keys3ItemsUnsorted()
        {
            var t = new BalancedTree<int>();
            t.Insert("2", 2);
            _output.WriteLine(t.Tree());
            t.Insert("3", 3);
            _output.WriteLine(t.Tree());
            t.Insert("1", 1);
            _output.WriteLine(t.Tree());
            var actual = t.Keys();
            Assert.Collection(actual,
                              n => Assert.Equal("1", n),
                              n => Assert.Equal("2", n),
                              n => Assert.Equal("3", n)
                             );
        }

        [Fact]
        public void Remove()
        {
            var t = new BalancedTree<int>();
            t.Insert("2", 2);
            _output.WriteLine(t.Tree());
            t.Remove("2");
            _output.WriteLine(t.Tree());
            Assert.Throws<KeyNotFoundException>(() => t.Get("2"));
        }

        [Fact]
        public void RemoveInMiddleOfTree()
        {
            var t = new BalancedTree<int>();
            t.Insert("1", 1);
            t.Insert("2", 2);
            t.Insert("3", 3);
            _output.WriteLine(t.Tree());
            t.Remove("2");
            _output.WriteLine(t.Tree());
            Assert.Throws<KeyNotFoundException>(() => t.Get("2"));
            var result1 = t.Get("1");
            var result3 = t.Get("3");
            Assert.Equal(1, result1);
            Assert.Equal(3, result3);
        }
    }
}
