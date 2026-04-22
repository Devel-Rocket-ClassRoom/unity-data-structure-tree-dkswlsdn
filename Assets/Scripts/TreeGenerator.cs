using System.Collections.Generic;
using NUnit.Framework;
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
    private int idx = 0;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        tree = new BinarySerachTree<int, string>();
        TreeGenerate();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("generate");
            TreeGenerate();
        }
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

        positionList = PositionCalculator(tree.root.Height, treeType);
        Debug.Log(tree.root.Height);
        Debug.Log(positionList.Count);
        DrawTree_Pow(tree.root, 0);
    }

    void DrawTree_Pow(TreeNode<int, string> node, int index, NodeType type = NodeType.Mid)
    {
        if (node == null) return;

        int myIdx = index;

        switch (type)
        {
            case NodeType.Left:
                myIdx = 2 * index + 1;
                break;
            case NodeType.Mid:
                myIdx = index;
                break;
            case NodeType.Right:
                myIdx = 2 * index + 2;
                break;
        }

        DrawNode(node, positionList[myIdx], positionList[index]);

        DrawTree_Pow(node.Left, myIdx, NodeType.Left);
        DrawTree_Pow(node.Right, myIdx, NodeType.Right);
    }

    void DrawTree_LevelOrder(TreeNode<int, string> node, Vector3 point, int height, NodeType type = NodeType.Mid)
    {
        if (node == null) return;
        int gap = 5;

    }

    void DrawNode(TreeNode<int, string> node, Vector3 point, Vector3 parentPoint, SortType type = SortType.Pow)
    {
        if (node == null) return;

        var n = Instantiate(nodePrefab);
        n.transform.position = point;
        var nod = n.GetComponent<Node>();
        nod.Key = node.Key;
        nod.Height = node.Height;

        DrawLine(point, parentPoint, n.GetComponent<LineRenderer>());
    }

    void DrawLine(Vector3 a, Vector3 b, LineRenderer renderer)
    {
        renderer.positionCount = 2;
        renderer.SetPosition(0, a);
        renderer.SetPosition(1, b);
    }


    List<Vector3> PositionCalculator(int rootHeight, SortType type)
    {
        List<Vector3> list = new List<Vector3>();

        for (int i = 0; i < rootHeight; i++)
        {
            int allNodeCount = (int)Mathf.Pow(2, i);
            float yPos = (rootHeight - i) * 5;

            float gap = 0;
            float levelWidth = 0;
            float startX = 0;

            switch (type)
            {
                case SortType.Pow:
                    gap = Mathf.Pow(2, rootHeight - i);
                    levelWidth = (allNodeCount - 1) * gap;
                    startX = -levelWidth / 2f;

                    for (int j = 0; j < allNodeCount; j++)
                    {
                        float xPos = startX + (j * gap);
                        list.Add(new Vector3(xPos, yPos, 0));
                    }
                    break;
                case SortType.In_Order:
                    gap = Mathf.Pow(2, rootHeight - i);
                    levelWidth = (allNodeCount - 1) * gap;
                    startX = -levelWidth / 2f;

                    for (int j = 0; j < allNodeCount; j++)
                    {
                        float xPos = startX + (j * gap);
                        list.Add(new Vector3(xPos, yPos, 0));
                    }
                    break;
                case SortType.Level_Order:
                    gap = 5;
                    startX = 0;

                    for (int j = 0; j < allNodeCount; j++)
                    {
                        float xPos = startX + (j * gap);
                        list.Add(new Vector3(xPos, yPos, 0));
                    }
                    break;
            }
        }

        return list;
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
