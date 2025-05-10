using System.Collections.Generic;
using System;

// Removes and returns a random element from the list.
public static class ListExtensions
{
   
    public static T PopRandom<T>(this List<T> list)
    {
        if (list == null || list.Count == 0)
            throw new InvalidOperationException("Cannot PopRandom from an empty list");
        int index = UnityEngine.Random.Range(0, list.Count);
        T item = list[index];
        list.RemoveAt(index);
        return item;
    }
}
