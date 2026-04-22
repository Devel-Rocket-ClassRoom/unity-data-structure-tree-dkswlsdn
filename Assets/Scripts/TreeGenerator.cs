using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class TreeGenerator : MonoBehaviour
{
    public SortType treeType;
    public GameObject nodePrefab;
    private LineRenderer lineRenderer;
    private SpriteRenderer spriteRenderer;
    public int nodeCount;
    private BinarySerachTree<int, string> tree;

    private List<Vector3> positionList = new List<Vector3>();
    readonly int gap = 5;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        tree = new BinarySerachTree<int, string>();
        TreeGenerate();
    }

    void TreeGenerate()
    {
        tree.Clear();

        List<int> numList = new List<int>();
        for (int i = 0; i < nodeCount; i++)
        {
            numList.Add(i);
        }

        for (int i = 0; i < nodeCount; i++)
        {
            int rand = Random.Range(0, numList.Count);
            int num = numList[rand];
            numList.Remove(numList[rand]);

            tree.Add(num, num.ToString());
        }


        switch (treeType)
        {
            case SortType.Pow:
                DrawTree_Pow(tree.root, new Vector3(0, gap * tree.root.Height, 0));
                break;
            case SortType.In_Order:
                DrawTree_InOrder(tree.root);
                DrawTree_InOrder2(tree.root, null, tree.root.Height);
                
                break;
            case SortType.Level_Order:
                DrawTree_LevelOrder(tree.root, tree.root.Height);
                DrawTree_LevelOrder2(tree.root);
                DrawTree_LevelOrder3(tree.root, null, tree.root.Height);
                break;
        }
        dict.Clear();
        idx = 0;
    }

    void DrawTree_Pow(TreeNode<int, string> node, Vector3 parentPoint, NodeType type = NodeType.Mid)
    {
        if (node == null) return;

        int height = (int)parentPoint.y / gap - 1;


        var currentNodePoint = parentPoint;

        switch (type)
        {
            case NodeType.Left:
                currentNodePoint = parentPoint + new Vector3(-gap * Mathf.Pow(2, height - 2), -gap, 0);
                break;
            case NodeType.Mid:
                currentNodePoint = parentPoint;
                break;
            case NodeType.Right:
                currentNodePoint = parentPoint + new Vector3(gap * Mathf.Pow(2, height - 2), -gap, 0);
                break;
        }

        DrawNode(node, currentNodePoint, parentPoint);

        DrawTree_Pow(node.Left, currentNodePoint, NodeType.Left);
        DrawTree_Pow(node.Right, currentNodePoint, NodeType.Right);
    }

    Dictionary<TreeNode<int, string>, int> dict = new Dictionary<TreeNode<int, string>, int>();
    private int idx = 0;
    void DrawTree_InOrder(TreeNode<int, string> node)
    {
        if (node == null) return;


        DrawTree_InOrder(node.Left);

        dict.Add(node, idx++);

        DrawTree_InOrder(node.Right);
    }

    TreeNode<int, string> DrawTree_InOrder2(TreeNode<int, string> node, TreeNode<int, string> parent, int level)
    {
        if (node == null) return null;

        if (node.Left != null)
        {
            DrawTree_InOrder2(node.Left, node, level - 1);
        }

        Vector3 nodePoint = new Vector3(dict[node] * gap, level * gap, 0);
        Vector3 parentNodePoint = parent != null ? new Vector3(dict[parent] * gap, (level + 1) * gap, 0) : nodePoint;
        DrawNode(node, nodePoint, parentNodePoint);

        if (node.Right != null)
        {
            DrawTree_InOrder2(node.Right, node, level - 1);
        }

        
        return node;
    }



    void DrawTree_LevelOrder(TreeNode<int, string> node, int level)
    {
        if (node == null) return;

        dict.Add(node, level);

        if (node.Left != null)
        {
            DrawTree_LevelOrder(node.Left, level - 1);
        }
        if (node.Right != null)
        {
            DrawTree_LevelOrder(node.Right, level - 1);
        }
    }
    void DrawTree_LevelOrder2(TreeNode<int, string> node)
    {
        Dictionary<TreeNode<int, string>, int> d = new Dictionary<TreeNode<int, string>, int>(dict);
        int height = node.Height;
        while (height > 0)
        {
            List<TreeNode<int, string>> list = new List<TreeNode<int, string>>();

            foreach (var n in d)
            {
                if (d[n.Key] == height)
                {
                    list.Add(n.Key);
                }
            }

            list.Sort((x, y) => x.Key.CompareTo(y.Key));

            int x = 0;
            for (int i = 0; i < list.Count; i++)
            {
                dict[list[i]] = x++;
            }

            height--;
        }
    }
    TreeNode<int, string> DrawTree_LevelOrder3(TreeNode<int, string> node, TreeNode<int, string> parent, int level)
    {
        if (node == null) return null;

        Vector3 nodePoint = new Vector3(dict[node] * gap, level * gap, 0);
        Vector3 parentNodePoint = parent != null ? new Vector3(dict[parent] * gap, (level + 1) * gap, 0) : nodePoint;
        DrawNode(node, nodePoint, parentNodePoint);

        if (node.Left != null)
        {
            DrawTree_LevelOrder3(node.Left, node, level - 1);
        }

        if (node.Right != null)
        {
            DrawTree_LevelOrder3(node.Right, node, level - 1);
        }

        return node;
    }




    void DrawNode(TreeNode<int, string> node, Vector3 point, Vector3 drawLinePoint)
    {
        if (node == null) return;
        if (node.Equals(drawLinePoint)) return;

        var n = Instantiate(nodePrefab);
        n.transform.position = point;
        var nod = n.GetComponent<Node>();
        nod.Key = node.Key;
        nod.Height = node.Height;

        DrawLine(point, drawLinePoint, n.GetComponent<LineRenderer>());
    }

    void DrawLine(Vector3 a, Vector3 b, LineRenderer renderer)
    {
        renderer.positionCount = 2;
        renderer.SetPosition(0, a);
        renderer.SetPosition(1, b);
    }


    

    public enum SortType
    {
        Pow, In_Order, Level_Order
    }

    public enum NodeType
    {
        Left, Mid, Right
    }
}
