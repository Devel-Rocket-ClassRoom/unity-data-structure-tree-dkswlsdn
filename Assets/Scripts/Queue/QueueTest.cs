using UnityEngine;

public class QueueTest : MonoBehaviour
{
    PriorityQueue<string, int> queue;

    void Start()
    {
        queue = new PriorityQueue<string, int>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            int rand = Random.Range(0, 100);
            queue.Enqueue(rand.ToString(), rand);
            Debug.Log($"Enqueue : {rand.ToString()}");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log($"Dequeue : {queue.Dequeue()}");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            queue.DebugQueue();
        }
    }
}
