using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static T RandomElement<T>(this List<T> items, bool removeElement = false)
    {
        var randomIndex = Random.Range(0, items.Count - 1);
        var value = items[randomIndex];

        if (removeElement)
            items.RemoveAt(randomIndex);

        return value;
    }

    public static MenuOption ContainsKey(this List<KeyValuePair<MenuOption, SlotElement>> list, MenuOption menuOption)
    {
        foreach (var kvp in list)
        {
            if (kvp.Key == menuOption)
                return kvp.Key;
        }

        return null;
    }

    public static bool IsNullOrEmpty<T>(this List<T> list)
    {
        return list == null || list.Count == 0;
    }
}
