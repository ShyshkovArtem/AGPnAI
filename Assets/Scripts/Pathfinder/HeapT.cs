/// <summary>
/// A generic binary heap implementation (min-heap) for fast retrieval of the smallest item.
/// </summary>
public class Heap<T> where T : IHeapItem<T>
{
    private T[] items;
    private int currentItemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    /// <summary>
    /// Adds an item to the heap.
    /// </summary>
    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    /// <summary>
    /// Removes and returns the first item (smallest) from the heap.
    /// </summary>
    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    /// <summary>
    /// Updates the position of an existing item in the heap (after its value has changed).
    /// </summary>
    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    /// <summary>
    /// Returns the number of items in the heap.
    /// </summary>
    public int Count
    {
        get { return currentItemCount; }
    }

    /// <summary>
    /// Checks whether the heap contains the given item.
    /// </summary>
    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    private void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount && items[childIndexLeft].CompareTo(items[childIndexRight]) > 0)
                {
                    swapIndex = childIndexRight;
                }

                if (item.CompareTo(items[swapIndex]) > 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    private void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) < 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    private void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}