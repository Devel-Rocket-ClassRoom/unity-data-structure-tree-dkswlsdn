using TMPro;
using UnityEngine;

public class Node : MonoBehaviour
{
    public TextMeshProUGUI text;

    public int Height;
    public int Key;

    private void Update()
    {
        text.text = Key.ToString();
    }
}
