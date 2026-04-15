using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages player input for selecting and swapping tiles.
/// Also handles the visual cursor stretching between two selected items.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Cursor : Singleton<Cursor>
{
    #region Variables

    [Header("Internal References")]
    private MatchablePool pool;
    private SpriteRenderer spriteRenderer;
    private Matchable[] selected;
    private MatchableGrid grid;

    [Header("Visual Settings")]
    [SerializeField] private Vector2Int verticalStretch = new Vector2Int(1, 2);
    [SerializeField] private Vector2Int horizontalStretch = new Vector2Int(2, 1);

    [SerializeField]
    private Vector3 halfUp      = Vector3.up / 2,
                    halfDown    = Vector3.down / 2,
                    halfLeft    = Vector3.left / 2,
                    halfRight   = Vector3.right / 2;

    #endregion

    protected override void Init()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        selected = new Matchable[2];
    }

    private void Start()
    {
        pool = (MatchablePool)MatchablePool.Instance;
        grid = (MatchableGrid)MatchableGrid.Instance;
    }

    /// <summary>
    /// Resets the selection state when the level restarts.
    /// </summary>
    public void Reset()
    {
        SelectFirst(null);
        spriteRenderer.enabled = true;
    }

    /// <summary>
    /// Sets the first tile and shows the cursor at its position.
    /// </summary>
    public void SelectFirst(Matchable toSelect)
    {
        selected[0] = toSelect;

        if (!enabled || selected[0] == null)
            return;

        transform.position = toSelect.transform.position;
        spriteRenderer.size = Vector2.one;
        spriteRenderer.enabled = true;
    }

    /// <summary>
    /// Checks if the second tile is valid and tries to perform a swap.
    /// </summary>
    public void SelectSecond(Matchable toSelect)
    {
        selected[1] = toSelect;

        if (!enabled || selected[0] == null || selected[1] == null || !selected[0].Idle || !selected[1].Idle || selected[0] == selected[1])
            return;

        if (SelectedAreAdjacent())
            StartCoroutine(grid.TrySwap(selected));

        SelectFirst(null);
    }

    /// <summary>
    /// Checks if selected tiles are neighbors and stretches the cursor visual accordingly.
    /// </summary>
    private bool SelectedAreAdjacent()
    {
        if (selected[0].position.x == selected[1].position.x)
        {
            if (selected[0].position.y == selected[1].position.y + 1)
            {
                spriteRenderer.size = verticalStretch;
                transform.position += halfDown;
                return true;
            }

            else if(selected[0].position.y == selected[1].position.y - 1)
            {
                spriteRenderer.size = verticalStretch;
                transform.position += halfUp;
                return true;
            }
        }

        else if (selected[0].position.y == selected[1].position.y)
        {
            if(selected[0].position.x == selected[1].position.x + 1)
            {
                spriteRenderer.size = horizontalStretch;
                transform.position += halfLeft;
                return true;
            }

            else if (selected[0].position.x == selected[1].position.x - 1)
            {
                spriteRenderer.size = horizontalStretch;
                transform.position += halfRight;
                return true;
            }
        }
        return false;
    }
}
