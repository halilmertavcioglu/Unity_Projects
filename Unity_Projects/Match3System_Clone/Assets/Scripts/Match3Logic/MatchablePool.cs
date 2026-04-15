using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the creation and recycling of match items.
/// </summary>
public class MatchablePool : ObjectPool <Matchable>
{
    #region Variables

    [Header("Pool Configuration")]
    [SerializeField] private int howManyTypes;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Color[] colors;

    [Header("Power-up Sprites")]
    [SerializeField] private Sprite crossPowerup;
    [SerializeField] private Sprite Match4Powerup;
    [SerializeField] private Sprite Match5Powerup;

    #endregion

    /// <summary>
    /// Assigns a random color and sprite to an item.
    /// </summary>
    public void RandomizeType(Matchable toRandomize)
    {
        int random = Random.Range(0, howManyTypes);
        toRandomize.SetType(random, sprites[random], colors[random]);
    }

    /// <summary>
    /// Gets an item from the pool and gives it a random type.
    /// </summary>
    public Matchable GetRandomMatchable()
    {
        Matchable randomMatchable = GetPooledObject();
        RandomizeType(randomMatchable);
        return randomMatchable;
    }

    /// <summary>
    /// Changes an item to the next available type in the list.
    /// </summary>
    public int NextType(Matchable matchable)
    {
        int nextType = (matchable.Type + 1) % howManyTypes;
        matchable.SetType(nextType, sprites[nextType], colors[nextType]);
        return nextType;
    }

    /// <summary>
    /// Turns a normal item into a special power-up item.
    /// </summary>
    public Matchable UpgradeMatchable(Matchable toBeUpgraded, MatchType type)
    {
        if(type == MatchType.cross)
            return toBeUpgraded.Upgrade(MatchType.cross, crossPowerup);

        if (type == MatchType.match4)
            return toBeUpgraded.Upgrade(MatchType.match4, Match4Powerup);

        if (type == MatchType.match5)
            return toBeUpgraded.Upgrade(MatchType.match5, Match5Powerup);

        Debug.LogWarning("Tried to upgrade a matchable with an invalid match type.");
        return toBeUpgraded;
    }

    /// <summary>
    /// Forces an item to be a specific type.
    /// </summary>
    public void ChangeType(Matchable toChange, int type)
    {
        toChange.SetType(type, sprites[type], colors[type]);
    }
}
