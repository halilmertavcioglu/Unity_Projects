using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Orientation
{
    none,
    horizontal,
    vertical,
    both
}

public enum MatchType
{
    invalid,
    match3,
    match4,
    match5,
    cross
}

/// <summary>
/// A collection of matching tiles and their properties.
/// </summary>
public class Match
{
    #region Variables

    [Header("Match Stats")]
    public Orientation orientation = Orientation.none;
    private int unlisted = 0;

    [Header("Match Content")]
    private List<Matchable> matchables;
    private Matchable toBeUpgraded = null;

    public List<Matchable> Matchables
    {
        get
        {
            return matchables; 
        }
    }

    public int Count
    {
        get
        {
            return matchables.Count + unlisted;
        }
    }

    #endregion

    /// <summary>
    /// Checks if an item is part of this match.
    /// </summary>
    public bool Contains(Matchable toCompare)
    {
        return matchables.Contains(toCompare);
    }

    public Match()
    {
        matchables = new List<Matchable>(5);
    }

    /// <summary>
    /// Starts a match and marks the first item for upgrade.
    /// </summary>
    public Match(Matchable original) : this()
    {
        AddMatchable(original);
        toBeUpgraded = original;
    }

    /// <summary>
    /// Decides the type of match based on count and direction.
    /// </summary>
    public MatchType Type
    {
        get
        {
            if (orientation == Orientation.both)
                return MatchType.cross;

            else if (matchables.Count == 3)
                return MatchType.match3;

            else if (matchables.Count == 4)
                return MatchType.match4;

            else if (matchables.Count > 4)
                return MatchType.match5;

            else
                return MatchType.invalid;
        } 
    }

    /// <summary>
    /// Picks which tile will turn into a power-up.
    /// </summary>
    public Matchable ToBeUpgraded
    {
        get
        {
            if(toBeUpgraded != null)
                return toBeUpgraded;

            return matchables[Random.Range(0, matchables.Count)];
        }
    }

    public void AddMatchable(Matchable toAdd)
    {
        matchables.Add(toAdd);
    }

    public void AddUnlisted()
    {
        unlisted++;
    }

    public void RemoveMatchable(Matchable toBeRemoved)
    {
        matchables.Remove(toBeRemoved);
    }

    /// <summary>
    /// Merges two matches and updates the direction.
    /// </summary>
    public void Merge(Match toMerge)
    {
        matchables.AddRange(toMerge.matchables);

        if
        (
               orientation == Orientation.both
            || toMerge.orientation == Orientation.both
            || (orientation == Orientation.horizontal && toMerge.orientation == Orientation.vertical)
            || (orientation == Orientation.vertical && toMerge.orientation == Orientation.horizontal)
        )
            orientation = Orientation.both;

        else if(toMerge.orientation == Orientation.horizontal)
            orientation = Orientation.horizontal;

        else if(toMerge.orientation == Orientation.vertical)
            orientation = Orientation.vertical;
    }

    /// <summary>
    /// Shows the match info as a string.
    /// </summary>
    public override string ToString()
    {
        string s = "Match of type" + matchables[0].Type + " : ";

        foreach (Matchable m in matchables)
            s += "(" + m.position.x + ", " + m.position.y + ") ";

        return s;
    }
}
