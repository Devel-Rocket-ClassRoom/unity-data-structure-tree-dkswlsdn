using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;

public class PriorityQueue<TElement, TPriority>
{
    private List<(TElement e, TPriority p)> queue;
    private Comparer<TPriority> comparer;

    public int Count { get { return queue.Count; } }



    public PriorityQueue()
    {
        queue = new List<(TElement e, TPriority p)>();
        comparer = Comparer<TPriority>.Default;
    }

    public PriorityQueue(Comparer<TPriority> comparer)
    {
        queue = new List<(TElement e, TPriority p)>();
        this.comparer = comparer;
    }



    public void Enqueue(TElement element, TPriority priority)
    {
        queue.Add((element, priority));
        int idx = queue.Count - 1;

        while (idx > 0)
        {
            int compare = comparer.Compare(queue[idx].p, queue[Parent(idx)].p);

            if (compare < 0)
            {
                var temp = queue[idx];
                queue[idx] = queue[Parent(idx)];
                queue[Parent(idx)] = temp;
                idx = (Parent(idx));
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
            throw new InvalidOperationException("큐 비어있음");
        }

        var result = queue[0];

        var temp = queue[0];
        queue[0] = queue[queue.Count - 1];
        queue[queue.Count - 1] = temp;
        
        queue.RemoveAt(queue.Count - 1);


        int idx = 0;

        while (idx < queue.Count - 1)
        {
            int minIdx = idx;

            if (LeftChild(idx) < queue.Count && comparer.Compare(queue[LeftChild(idx)].p, queue[minIdx].p) < 0)
            {
                minIdx = LeftChild(idx);
            }
            if (LeftChild(idx) + 1 < queue.Count && comparer.Compare(queue[LeftChild(idx) + 1].p, queue[minIdx].p) < 0)
            {
                minIdx = LeftChild(idx) + 1;
            }

            if (minIdx == idx) break;

            if (comparer.Compare(queue[idx].p, queue[minIdx].p) > 0)
            {
                var t = queue[idx];
                queue[idx] = queue[minIdx];
                queue[minIdx] = t;
                idx = minIdx;
            }
        }

        return result.e;
    }
    public TElement Peek()
    {
        if (queue.Count == 0)
        {
            throw new InvalidOperationException("큐 비어있음");
        }

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
