using SnapshotableCollections;

namespace SnapshotableTest
{
    public class BalancedTreeTestWrapper<T> : BalancedTree<T> where T: struct
    {
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
    }
}
