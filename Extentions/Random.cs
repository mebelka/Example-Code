using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class RandomElement
{

    /// Shuffles a list so that it is in a random order.
    public static void ShuffleList<T>(this List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /// Removes one element from the list at random.
    public static T RemoveRandomElement<T>(this List<T> list)
    {
        if (list.Count == 0)
        {
            return default;
        }

        int randomIndex = UnityEngine.Random.Range(0, list.Count);
        T removedObject = list[randomIndex];
        list.RemoveAt(randomIndex);
        return removedObject;
    }

    /// Selects a random element from a list.
    public static T RandomListElement<T>(this List<T> list)
    {
        int randomIndex = UnityEngine.Random.Range(0, list.Count);
        return list[randomIndex];
    }

    /// Selects a random element from a list, which is different than the object given
    public static T RandomListElementDifferentThan<T>(this List<T> list, T differentThan)
    {
        if (list.Count > 1)
        {
            list.Remove(differentThan);
        }
        return RandomListElement(list);
    }

    /// Selects a random enum.
    public static Enum RandomEnum(Type type, int? maxRange = null, Enum differentThan = null)
    {
        List<Enum> values = Enum.GetValues(type).OfType<Enum>().ToList(); // Madison if there is slowdown issues check here.
        if (differentThan != null)
        {
            values.Remove(differentThan);
        }

        return RandomListElement(values);
    }
}
