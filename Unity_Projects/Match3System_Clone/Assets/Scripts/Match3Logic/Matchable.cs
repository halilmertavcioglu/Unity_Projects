using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

/// <summary>
/// Represents a single item on the grid. Handles its type, power-ups, and clicks.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Matchable : Movable
{
    #region Variables

    private MatchablePool pool;
    private MatchableGrid grid;
    private Cursor cursor;
    private SpriteRenderer spriteRenderer;

    [Header("Item State")]
    private int type;
    private MatchType powerup = MatchType.invalid;

    [Header("Grid Position")]
    public Vector2Int position;

    public int Type
    {
        get
        {
            return type;
        }
    }

    public bool IsGem
    {
        get
        {
            return powerup == MatchType.match5;
        }
    }

    #endregion

    private void Awake()
    {
        cursor = Cursor.Instance;
        pool = (MatchablePool) MatchablePool.Instance;
        grid = (MatchableGrid)MatchableGrid.Instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Updates the visual look and type of the item.
    /// </summary>
    public void SetType(int type, Sprite sprite, Color color)
    {
        this.type = type;
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;
    }

    /// <summary>
    /// Destroys the item or activates its power-up, then returns it to the pool.
    /// </summary>
    public IEnumerator Resolve(Transform collectionPoint)
    {
        if(powerup != MatchType.invalid)
        {
            if(powerup == MatchType.match4)
            {
                grid.MatchAllAdjacent(this);
            }

            if(powerup == MatchType.cross)
            {
                grid.MatchRowAndColumn(this);
            }

            powerup = MatchType.invalid;
        }

        if(collectionPoint == null)
            yield break;

        spriteRenderer.sortingOrder = 2;
        yield return StartCoroutine(MoveToTransform(collectionPoint));
        spriteRenderer.sortingOrder = 1;
        pool.ReturnObjectPool(this);
    }

    /// <summary>
    /// Changes the item into a power-up version.
    /// </summary>
    public Matchable Upgrade(MatchType powerupType, Sprite powerupSprite)
    {
        if (powerupType != MatchType.invalid)
        {
            idle = false;
            StartCoroutine(Resolve(null));
            idle = true;
        }

        if(powerupType == MatchType.match5)
        {
            type = -1;
            spriteRenderer.color = Color.white;
        }

        powerup = powerupType;
        spriteRenderer.sprite = powerupSprite;

        return this;
    }

    /// <summary>
    /// Sets the drawing layer of the sprite.
    /// </summary>
    public int SortingOrder
    {
        set
        {
            spriteRenderer.sortingOrder = value;
        }
    }

    private void OnMouseDown()
    {
        cursor.SelectFirst(this);
    }

    private void OnMouseUp()
    {
        cursor.SelectFirst(null);
    }

    private void OnMouseEnter()
    {
        cursor.SelectSecond(this);
    }
}
