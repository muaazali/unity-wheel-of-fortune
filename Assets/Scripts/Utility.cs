using UnityEngine;
using System.Collections.Generic;

public static class Utility
{
    public static List<T> ShuffleItems<T>(List<T> items)
    {
        List<T> shuffledItems = new List<T>(items);

        for (int i = 0; i < shuffledItems.Count; i++)
        {
            T temp = shuffledItems[i];
            int randomIndex = Random.Range(i, shuffledItems.Count);
            shuffledItems[i] = shuffledItems[randomIndex];
            shuffledItems[randomIndex] = temp;
        }

        return shuffledItems;
    }
}