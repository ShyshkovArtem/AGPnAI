using System;
using System.Collections.Generic;

/// <summary>
/// Interface for items stored in the heap, providing comparison and index tracking.
/// </summary>
public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex { get; set; }
}