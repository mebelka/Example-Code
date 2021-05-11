using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// A list that can be looped around and can always get the previous and next values.
public class LoopedList<T>: IEnumerable<T> {

    private List<T> values = new List<T>();
    private int currentIndex = 0;
    public T CurrentValue => values[currentIndex];

    public event Action CompletedLoop;

    public LoopedList(List<T> list) {
        values = list;
    }

    public LoopedList(T[] array) {
        values.AddRange(array);
    }

    /// Moves the index of the looped list to the matching item
    public void GoTo(T item) {
        for(int i =0; i < values.Count; i++) {
            if(values[i].Equals(item)) {
                currentIndex = i;
                return;
            }
        }
    }

    public T GetNext() {
        currentIndex++;
        if(currentIndex >= values.Count) {
            currentIndex = 0;
            CompletedLoop?.Invoke();
        }

        return values[currentIndex];
    }

    public T GetPrevious() {
        currentIndex--;
        if(currentIndex < 0) {
            currentIndex = values.Count - 1;
        }

        return values[currentIndex];
    }

    public IEnumerator<T> GetEnumerator() {
        return values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return values.GetEnumerator();
    }
}
