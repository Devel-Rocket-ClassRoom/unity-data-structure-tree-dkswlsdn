using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class PriorityQueue<TElement, TPriority>
{
    private List<(TElement e, TPriority p)> queue = new List<(TElement e, TPriority p)>();

    public int Count { get { return queue.Count; } }

    public void Enqueue(TElement element, TPriority priority)
    {
        queue.Add((element, priority));
        int idx = queue.Count - 1;

        while (idx > 0)
        {
            int compare = Comparer<TPriority>.Default.Compare(queue[idx].p, queue[Parent(idx)].p);

            if (compare < 0)
            {
                var temp = queue[idx];
                queue[idx] = queue[Parent(idx)];
                queue[Parent(idx)] = temp;
                idx = (Parent(idx));
            }
            else if (compare == 0)
            {
                queue[Parent(idx)] = queue[idx];
                queue.RemoveAt(idx);
                return;
            }
            else
            {
                return;
            }
        }
    }
    public TElement Dequeue()
    {
        if (queue.Count == 0)
        {
            Debug.Log("아무것도 없음");
            
        }

        var result = queue[0];

        var temp = queue[0];
        queue[0] = queue[queue.Count - 1];
        queue[queue.Count - 1] = temp;
        
        queue.RemoveAt(queue.Count - 1);


        int idx = 0;

        while (idx < queue.Count - 1)
        {
            int childIdx = 0;

            if (LeftChild(idx) < queue.Count && LeftChild(idx) + 1 < queue.Count)
            {
                if (Comparer<TPriority>.Default.Compare(queue[LeftChild(idx)].p, queue[LeftChild(idx) + 1].p) > 0)
                {
                    childIdx = LeftChild(idx) + 1;
                }
                else
                {
                    childIdx = LeftChild(idx);
                }
            }
            else if (LeftChild(idx) < queue.Count && LeftChild(idx) + 1 >= queue.Count)
            {
                childIdx = LeftChild(idx);
            }
            else if (LeftChild(idx) >= queue.Count && LeftChild(idx) + 1 < queue.Count)
            {
                childIdx = LeftChild(idx) + 1;
            }
            else
            {
                break;
            }

            if (Comparer<TPriority>.Default.Compare(queue[idx].p, queue[childIdx].p) > 0)
            {
                var t = queue[idx];
                queue[idx] = queue[childIdx];
                queue[childIdx] = t;
                idx = childIdx;
            }
            else
            {
                break;
            }
        }

        return result.e;
    }
    public TElement Peek()
    {
        return queue[0].e;
    }
    public void Clear()
    {
        queue.Clear();
    }


    int Parent(int i)
    {
        return (i - 1) / 2;
    }
    int LeftChild(int i)
    {
        return i * 2 + 1;
    }

    public void DebugQueue()
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < queue.Count; i++)
        {
           sb.Append($"[{queue[i].e}] ");
        }

        Debug.Log(sb.ToString());
    }
}
