using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A base system to manage any 2D grid of items.
/// </summary>
public abstract class GridSystem<T> : Singleton <GridSystem<T>>
{
    #region Variables

    [Header("Grid Configuration")]
    private Vector2Int dimensions = new Vector2Int(1, 1);
    private bool isReady;

    [Header("Grid Data")]
    private T[,] data;

    public Vector2Int Dimensions
    {
        get
        {
            return dimensions;
        }
    }

    public bool IsReady
    {
        get
        {
            return isReady;
        }
    }

    #endregion

    /// <summary>
    /// Creates the grid with given sizes and prepares the data.
    /// </summary
    public void InitializeGrid(Vector2Int dimensions)
    {
        if (dimensions.x < 1 || dimensions.y < 1)
            Debug.LogError("Grid dimensions must be positive numbers.");

        this.dimensions = dimensions;
        data = new T[dimensions.x, dimensions.y];

        isReady = true;
    }

    /// <summary>
    /// Checks if the given coordinates are inside the grid limits.
    /// </summary>
    public bool CheckBounds(int x, int y)
    {
        if (!isReady)
            Debug.LogError("Grid has not been initialized.");

        return x >= 0 && x < dimensions.x && y >= 0 && y < dimensions.y;
    }

    /// <summary>
    /// Overload: Checks bounds using a Vector2Int position.
    /// </summary>
    public bool CheckBounds(Vector2Int position)
    {
        return CheckBounds(position.x, position.y);
    }

    /// <summary>
    /// Checks if a specific grid slot is empty.
    /// </summary>
    public bool IsEmpty(int x, int y)
    {
        if (!CheckBounds(x, y))
            Debug.LogError("(" + x + ", " + y + ") are not on the grid.");

        return EqualityComparer<T>.Default.Equals(data[x, y], default(T));
    }

    /// <summary>
    /// Overload: Checks if a slot is empty using Vector2Int.
    /// </summary>
    public bool IsEmpty(Vector2Int position)
    {
        return IsEmpty(position.x, position.y);
    }

    /// <summary>
    /// Places an item at the specified coordinates.
    /// </summary>
    public bool PutItemAt(T item, int x, int y, bool allowOwerwrite = false)
    {
        if (!CheckBounds(x, y))
            Debug.LogError("(" + x + ", " + y + ") are not on the grid.");

        if (!allowOwerwrite && !IsEmpty(x, y))
            return false;

        data [x, y] = item;
        return true;
    }

    /// <summary>
    /// Overload: Places an item using Vector2Int.
    /// </summary>
    public bool PutITemAt(T item, Vector2Int position, bool allowOverwrite = false)
    {
        return PutItemAt(item, position.x, position.y, allowOverwrite);
    }

    /// <summary>
    /// Retrieves the item stored at the given coordinates.
    /// </summary>
    public T GetItemAt(int x, int y)
    {
        if (!CheckBounds(x, y))
            Debug.LogError("(" + x + ", " + y + ") are not on the grid.");

        return data[x, y];
    }

    /// <summary>
    /// Overload: Gets an item using Vector2Int.
    /// Aþýrý yükleme: Vector2Int kullanarak bir objeyi getirir.
    /// </summary>
    public T GetItemAt(Vector2Int position)
    {
        return GetItemAt(position.x, position.y);
    }

    /// <summary>
    /// Removes an item from the slot and returns it.
    /// </summary>
    public T RemoveItemAt(int x, int y)
    {
        if (!CheckBounds(x, y))
            Debug.LogError("(" + x + ", " + y + ") are not on the grid.");

        T temp = data[x, y];
        data[x, y] = default(T);
        return temp;
    }

    /// <summary>
    /// Overload: Removes an item using Vector2Int.
    /// </summary>
    public T RemoveItemAt(Vector2Int position)
    {
        return RemoveItemAt(position.x, position.y);
    }

    /// <summary>
    /// Moves an item from one slot to another.
    /// </summary>
    public bool MoveItemTo(int x1, int y1, int x2, int y2, bool allowOwerwrite = false)
    {
        if (!CheckBounds(x1, y1))
            Debug.LogError("(" + x1 + ", " + y1 + ") are not on the grid.");

        if (!CheckBounds(x2, y2))
            Debug.LogError("(" + x2 + ", " + y2 + ") are not on the grid.");

        if (!allowOwerwrite && !IsEmpty(x2, y2))
            return false;

        data[x2, y2] = RemoveItemAt(x1, y1); ;
        return true;
    }

    /// <summary>
    /// Overload: Moves an item using two Vector2Int positions.
    /// </summary>
    public bool MoveItemTo(Vector2Int position1, Vector2Int position2, bool allowOverwrite = false)
    {
        return MoveItemTo(position1.x, position1.y, position2.x, position2.y, allowOverwrite);
    }

    /// <summary>
    /// Swaps the content of two grid slots.
    /// </summary>
    public void SwapItemsAt(int x1, int y1, int x2, int y2)
    {
        if (!CheckBounds(x1, y1))
            Debug.LogError("(" + x1 + ", " + y1 + ") are not on the grid.");

        if (!CheckBounds(x2, y2))
            Debug.LogError("(" + x2 + ", " + y2 + ") are not on the grid.");

        T temp = data[x1, y1];
        data[x1, y1] = data[x2, y2];
        data[x2, y2] = temp;
    }

    /// <summary>
    /// Overload: Swaps items using Vector2Int positions.
    /// </summary>
    public void SwapItemsAt(Vector2Int position1, Vector2Int position2)
    {
        SwapItemsAt(position1.x, position1.y, position2.x, position2.y);
    }

    /// <summary>
    /// Returns a text visualization of the grid.
    /// </summary>
    public override string ToString()
    {
        string s = "";

        for (int y = dimensions.y - 1; y != -1; y--)
        {
            s += "[";
            for (int x = 0; x != dimensions.x; x++)
            {
                if(IsEmpty(x, y))
                    s += " ";
                else
                    s += data[x, y].ToString();

                if(x != dimensions.x - 1)
                    s += ", ";
            }
            s += "]\n";
        }
        return s;
    }
}
