using UnityEngine;

public class TreeTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var bst = new BinarySerachTree<string, string>();
        bst["5"] = "A";
        bst["4"] = "B";
        bst["6"] = "C";
        bst["7"] = "D";
        bst["8"] = "E";
        bst["1"] = "F";
        bst["2"] = "G";

        foreach (var pair in bst.LevelOrderTraversal())
        {
            Debug.Log(pair);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
