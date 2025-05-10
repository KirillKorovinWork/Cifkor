using System;
using System.Collections.Generic;

public static class QueueExtensions
{
    public static void RemoveAll<T>(this Queue<T> queue, Predicate<T> match)
    {
        if (queue.Count == 0) return;
        int n = queue.Count;
        var items = queue.ToArray();
        queue.Clear();
        for (int i = 0; i < n; i++)
        {
            var item = items[i];
            if (!match(item)) queue.Enqueue(item);
        }
    }
}

    

