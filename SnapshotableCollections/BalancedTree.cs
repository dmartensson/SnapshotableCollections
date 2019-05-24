#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SnapshotableCollections
{
    
    public class BalancedTree<T> : IEnumerable<KeyValuePair<string, T>> where T: struct
    {
        private TreeNode? _root;
        private readonly StringComparison _comparer;

        public BalancedTree(StringComparison? comparer = null)
        {
            _comparer = comparer ?? StringComparison.OrdinalIgnoreCase;
        }

        ///If inserting a key that already exists, we overwrite the previous key
        public void Insert(string key, T value)
        {
            if (_root == null)
            {
                _root = new TreeNode(key, value);
                return;
            }

            var (parent, current, found) = InternalFind(key);
            if (found)
                current!.Value = value;
            else
            {
                Debug.Assert(parent != null);
                current = new TreeNode(key, value) {Parent = parent};
                if (string.Compare(key, parent.Key, _comparer) < 0)
                {
                    parent!.Left = current;
                }
                else
                {
                    parent!.Right = current;
                }
            }
            //TODO balance tree
        }

        public void Remove(string key)
        {
            var (parent, node, found) = InternalFind(key);
            if (!found || node == null)
                return;
            if (parent == null)
            {
                _root = null;
            }
            else if (parent.Left == node)
                parent.Left = null;
            else if (parent.Right == node)
                parent.Right = null;
            if(node.Left != null)
                foreach (var n in Walk(node.Left))
                    Insert(n.Key, n.Value);
            if (node.Right != null)
                foreach (var n in Walk(node.Right))
                    Insert(n.Key, n.Value);
        }
        public T Get(string key)
        {
            var (_, current, found) = InternalFind(key);
            if (!found || current == null)
                throw new KeyNotFoundException($"Key '{key}' does not exist!");
            return current.Value;
        }

        public T Get(string key, T defaultValue)
        {
            var (_, current, _) = InternalFind(key);
            return current?.Value ?? defaultValue;
        }
        public bool TryGet(string key, out T value)
        {
            var (_, current, found) = InternalFind(key);
            value = current?.Value ?? default;
            return found;
        }

        public List<string> Keys()
        {
            return _root == null 
                ? new List<string>() 
                : Walk(_root).Select(n => n.Key).ToList();
        }

        public List<T> Values()
        {
            return _root == null
                ? new List<T>()
                : Walk(_root).Select(n => n.Value).ToList();
        }

        public string Tree()
        {
            return InternalTree(_root);
        }

        private string InternalTree(TreeNode? node)
        {
            return node == null 
                ? "-" 
                : $"{node.Key}:{node.Value}(Left: {InternalTree(node.Left)}, Right: {InternalTree(node.Right)})";
        }

        private IEnumerable<TreeNode> Walk(TreeNode node)
        {
            if (node.Left != null)
                foreach (var n in Walk(node.Left))
                    yield return n;
            yield return node;
            if (node.Right != null)
                foreach (var n in Walk(node.Right))
                    yield return n;
        }
        ///Finds the closest match, if the key exists, returns the Keynode, if not returns the parent that is closest to the key
        private (TreeNode? parent, TreeNode? current, bool found) InternalFind(string key)
        {
            if (_root == null)
                return (null, null, false);
            var walk = _root;
            while (walk != null)
            {
                if (walk.Key.Equals(key, _comparer))
                    return (walk.Parent, walk, true);
                if (string.Compare(key, walk.Key, _comparer) < 0)
                {
                    if (walk.Left != null)
                        walk = walk.Left;
                    else
                        return (walk, null, false);
                }
                else if (walk.Right != null)
                    walk = walk.Right;
                else
                {
                    return (walk, null, false);
                }
            }
            throw new InvalidOperationException("Walker is null!");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (_root != null)
                foreach (var node in Walk(_root))
                    yield return node;
        }

        IEnumerator<KeyValuePair<string, T>> IEnumerable<KeyValuePair<string, T>>.GetEnumerator()
        {
            if (_root != null)
                foreach (var node in Walk(_root))
                    yield return new KeyValuePair<string, T>(node.Key, node.Value);
        }
        private class TreeNode
        {
            public string Key { get; }
            public T Value { get; set; }
            public TreeNode? Left { get; set; }
            public TreeNode? Right { get; set; }
            public TreeNode? Parent { get; set; }
            public int Height { get; set; } = 0;
            public TreeNode(string key, T value)
            {
                Key = key;
                Value = value;
            }

        }
    }
}
