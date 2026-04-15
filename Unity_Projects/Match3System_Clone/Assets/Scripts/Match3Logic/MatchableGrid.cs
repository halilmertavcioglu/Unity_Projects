using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

/// <summary>
/// Core grid logic for the Match-3 game. 
/// Handles population, matching algorithms, grid collapsing, and move scanning.
/// </summary>
public class MatchableGrid : GridSystem<Matchable>
{
    #region Variables

    [Header("Systems")]
    private MatchablePool pool;
    private ScoreManager score;
    private HintIndicator hint;
    private AudioManager audioManager;

    [Header("Visual Settings")]
    [SerializeField] private Vector3 offscreenOffset;
    [SerializeField] private List<Matchable> possibleMoves;

    #endregion

    protected override void Init()
    {
        pool = (MatchablePool) MatchablePool.Instance;
        score = ScoreManager.Instance;
        hint = HintIndicator.Instance;
        audioManager = AudioManager.Instance;
    }

    /// <summary>
    /// Clears the entire board and refills it, usually for level restarts.
    /// </summary>
    public IEnumerator Reset()
    {
        for (int y = 0; y != Dimensions.y; y++)
            for (int x = 0; x != Dimensions.x; x++)
                if (!IsEmpty(x, y))
                    pool.ReturnObjectPool(RemoveItemAt(x, y));

        yield return StartCoroutine(PopulatedGrid(false, true));
    }

    /// <summary>
    /// Fills empty grid spaces with new items from the pool.
    /// </summary>
    public IEnumerator PopulatedGrid(bool allowMatches = false, bool initialPopulation = false)
    {
        List<Matchable> newMatchables = new List<Matchable>();
        Matchable newMatchable;
        Vector3 onscreenPosition;

        for(int y = 0; y != Dimensions.y; y++)
            for(int x = 0; x != Dimensions.x; x++)
                if(IsEmpty(x, y))
                {
                    newMatchable = pool.GetRandomMatchable();
                    newMatchable.gameObject.SetActive(true);
                    newMatchable.position = new Vector2Int(x, y);
                    PutItemAt(newMatchable, x, y);
                    newMatchables.Add(newMatchable);

                    int type = newMatchable.Type;
                    newMatchable.transform.position = transform.position + new Vector3(x, y) + offscreenOffset;

                    while (!allowMatches && IsPartOfAMatch(newMatchable))
                    {
                        if (pool.NextType(newMatchable) == type)
                        {
                            Debug.LogWarning("Failed to find a matchable type that didnt match at (" + x + ", " + y + ")");
                            break;
                        }
                    }
                }

        for(int i = 0; i != newMatchables.Count; i++)
        {
            onscreenPosition = transform.position + new Vector3(newMatchables[i].position.x, newMatchables[i].position.y);

            StartCoroutine(audioManager.PlayDelayedSound(SoundEffects.land, 1f / newMatchables[i].Speed));

            if (i == newMatchables.Count - 1)
                yield return StartCoroutine(newMatchables[i].MoveToPosition(onscreenPosition));

            else
                StartCoroutine(newMatchables[i].MoveToPosition(onscreenPosition));


            if (initialPopulation)
                yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// Checks if a tile is part of a horizontal or vertical match of 3 or more.
    /// </summary>
    private bool IsPartOfAMatch(Matchable toMatch)
    {
        int horizontalMatches = 0, verticalMatches = 0;

        horizontalMatches += CountMatchesInDirection(toMatch, Vector2Int.left);
        horizontalMatches += CountMatchesInDirection(toMatch, Vector2Int.right);
        if (horizontalMatches > 1) return true;

        verticalMatches += CountMatchesInDirection(toMatch, Vector2Int.up);
        verticalMatches += CountMatchesInDirection(toMatch, Vector2Int.down);
        if (verticalMatches > 1) return true;

        return false;
    }

    /// <summary>
    /// Looks for same-type items in a specific line.
    /// </summary>
    private int CountMatchesInDirection(Matchable toMatch, Vector2Int direction)
    {
        int matches = 0;
        Vector2Int position = toMatch.position + direction;

        while (CheckBounds(position) && !IsEmpty(position) && GetItemAt(position).Type == toMatch.Type)
        {
            matches++;
            position += direction;
        }
        return matches;
    }

    /// <summary>
    /// Swaps two tiles and resolves any matches. Swaps back if no match is found.
    /// </summary>
    public IEnumerator TrySwap(Matchable[] toBeSwapped)
    {
        Matchable[] copies = new Matchable[2];
        copies[0] = toBeSwapped[0];
        copies[1] = toBeSwapped[1];
        hint.CancelHint();

        yield return StartCoroutine(Swap(copies));

        if (copies[0].IsGem && copies[1].IsGem)
        {
            MatchEverything();
            yield break;
        }

        else if (copies[0].IsGem)
        {
            MatchEverythingByType(copies[0], copies[1].Type);
            yield break;
        }

        else if (copies[1].IsGem)
        {
            MatchEverythingByType(copies[1], copies[0].Type);
            yield break;
        }
        
        Match[] matches = new Match[2];
        matches[0] = GetMatch(copies[0]);
        matches[1] = GetMatch(copies[1]);

        if (matches[0] != null)
            StartCoroutine(score.ResolveMatch(matches[0]));

        if (matches[1] != null)
            StartCoroutine(score.ResolveMatch(matches[1]));

        if (matches[0] == null && matches[1] == null)
        {
            yield return StartCoroutine(Swap(copies));
            
            if(ScanForMatches())
                StartCoroutine(FillAndScanGrid());
        }

        else
            StartCoroutine(FillAndScanGrid());
    }

    /// <summary>
    /// Finds extra matches in T or L shapes.
    /// </summary>
    private void GetBranches(Match tree, Match branchToSearch, Orientation perpendicular)
    {
        Match branch;

        foreach (Matchable matchable in branchToSearch.Matchables)
        {
            branch = GetMatchesInDirection(tree, matchable, perpendicular == Orientation.horizontal ? Vector2Int.left : Vector2Int.down);
            branch.Merge(GetMatchesInDirection(tree, matchable, perpendicular == Orientation.horizontal ? Vector2Int.right : Vector2Int.up));
            branch.orientation = perpendicular;

            if(branch.Count > 1)
            {
                tree.Merge(branch);
                GetBranches(tree, branch, perpendicular == Orientation.horizontal ? Orientation.horizontal : Orientation.vertical);
            }
        }
    }

    /// <summary>
    /// Loop: Items fall down, board refills, and looks for new matches.
    /// </summary>
    private IEnumerator FillAndScanGrid()
    {
        CollapseGrid();
        yield return StartCoroutine(PopulatedGrid(true));

        if (ScanForMatches())
            StartCoroutine(FillAndScanGrid());

        else
            CheckPossibleMoves();
    }

    /// <summary>
    /// Checks if the player can still make a move.
    /// </summary>
    public void CheckPossibleMoves()
    {
        if(ScanForMoves() == 0)
            GameManager.Instance.NoMoreMoves();

        else
            hint.StartAutoHint(possibleMoves[Random.Range(0, possibleMoves.Count)].transform);
    }

    /// <summary>
    /// Swaps the position of two items on screen and in data.
    /// </summary>
    private IEnumerator Swap(Matchable[] toBeSwapped)
    {
        SwapItemsAt(toBeSwapped[0].position, toBeSwapped[1].position);

        Vector2Int temp = toBeSwapped[0].position;
        toBeSwapped[0].position = toBeSwapped[1].position;
        toBeSwapped[1].position = temp;

        Vector3[] worldPosition = new Vector3[2];
        worldPosition[0] = toBeSwapped[0].transform.position;
        worldPosition[1] = toBeSwapped[1].transform.position;

        audioManager.PlaySound(SoundEffects.swap);

        StartCoroutine(toBeSwapped[0].MoveToPosition(worldPosition[1]));
        yield return StartCoroutine(toBeSwapped[1].MoveToPosition(worldPosition[0]));
    }

    /// <summary>
    /// Analyzes a single tile to see if it forms a match in any direction.
    /// </summary>
    private Match GetMatch(Matchable toMatch)
    {
        Match match = new Match(toMatch);

        Match horizontalMatch, verticalMatch;

        horizontalMatch = GetMatchesInDirection(match, toMatch, Vector2Int.left);
        horizontalMatch.Merge(GetMatchesInDirection(match, toMatch, Vector2Int.right));

        horizontalMatch.orientation = Orientation.horizontal;

        if (horizontalMatch.Count > 1)
        {
            match.Merge(horizontalMatch);
            GetBranches(match, horizontalMatch, Orientation.vertical);

        }

        verticalMatch = GetMatchesInDirection(match, toMatch, Vector2Int.up);
        verticalMatch.Merge(GetMatchesInDirection(match, toMatch, Vector2Int.down));

        verticalMatch.orientation = Orientation.vertical;

        if (verticalMatch.Count > 1)
        {
            match.Merge(verticalMatch);
            GetBranches(match, verticalMatch, Orientation.horizontal);
        }

        if (match.Count == 1)
            return null;

        return match;
    }

    /// <summary>
    /// Helper: Finds matching tiles in a straight direction.
    /// </summary>
    private Match GetMatchesInDirection(Match tree, Matchable toMatch, Vector2Int direction)
    {
        Match match = new Match();
        Vector2Int position = toMatch.position + direction;
        Matchable next;

        while (CheckBounds(position) && !IsEmpty(position))
        {
            next = GetItemAt(position);

            if (next.Type == toMatch.Type && next.Idle)
            {
                if(!tree.Contains(next))
                    match.AddMatchable(next);

                else
                    match.AddUnlisted();

                position += direction;
            }
            else
                break;
        }
        return match;
    }

    /// <summary>
    /// Gravity: Moves items down to fill empty holes.
    /// </summary>
    private void CollapseGrid()
    {
        for(int x = 0; x != Dimensions.x; x++)
            for(int yEmpty = 0; yEmpty != Dimensions.y - 1; yEmpty++)
                if(IsEmpty(x, yEmpty))
                    for(int yNotEmpty = yEmpty + 1; yNotEmpty != Dimensions.y; yNotEmpty++)
                        if(!IsEmpty(x, yNotEmpty) && GetItemAt(x, yNotEmpty).Idle)
                        {
                            MoveMatchableToPosition(GetItemAt(x, yNotEmpty), x, yEmpty);
                            break;
                        }
    }

    /// <summary>
    /// Moves an item to a new slot and starts the move animation.
    /// </summary>
    private void MoveMatchableToPosition(Matchable toMove, int x, int y)
    {
        MoveItemTo(toMove.position, new Vector2Int(x, y));

        toMove.position = new Vector2Int(x,y);

        StartCoroutine(toMove.MoveToPosition(transform.position + new Vector3(x, y)));

        audioManager.PlayDelayedSound(SoundEffects.land, 1f / toMove.Speed);
    }

    /// <summary>
    /// Scans the whole board to find and clear matches.
    /// </summary>
    private bool ScanForMatches()
    {
        bool madeAMatch = false;
        Matchable toMatch;
        Match match;

        for(int y = 0; y != Dimensions.y; y++)
            for(int x = 0; x != Dimensions.x; x++)
                if(!IsEmpty(x, y))
                {
                    toMatch = GetItemAt(x, y);

                    if (!toMatch.Idle)
                        continue;

                    match = GetMatch(toMatch);

                    if(match != null)
                    {
                        madeAMatch = true;
                        StartCoroutine(score.ResolveMatch(match));
                    }    
                }
        return madeAMatch;
    }

    /// <summary>
    /// Power-up: Clears all 8 neighbors.
    /// </summary>
    public void MatchAllAdjacent(Matchable powerup)
    {
        Match allAdjacent = new Match();
        for(int y = powerup.position.y - 1; y != powerup.position.y + 2; y++)
            for(int x = powerup.position.x - 1; x != powerup.position.x + 2; x++)
                if(CheckBounds(x, y) && !IsEmpty(x, y) && GetItemAt(x, y).Idle)
                    allAdjacent.AddMatchable(GetItemAt(x, y));

        StartCoroutine(score.ResolveMatch(allAdjacent, MatchType.match4));

        audioManager.PlaySound(SoundEffects.powerup);
    }

    /// <summary>
    /// Power-up: Clears the full row and column.
    /// </summary>
    public void MatchRowAndColumn(Matchable powerup)
    {
        Match rowAndColumn = new Match();

        for (int y = 0; y != Dimensions.y; y++)
        {
            Matchable m = GetItemAt(powerup.position.x, y);
            if (CheckBounds(powerup.position.x, y) && !IsEmpty(powerup.position.x, y) && m.Idle)
                rowAndColumn.AddMatchable(m);
        }
            
        for (int x = 0; x != Dimensions.x; x++)
        {
            Matchable m = GetItemAt(x, powerup.position.y);
            if (CheckBounds(x, powerup.position.y) && !IsEmpty(x, powerup.position.y) && m.Idle)
                if(!rowAndColumn.Contains(m))
                    rowAndColumn.AddMatchable(m);
        }
        StartCoroutine(score.ResolveMatch(rowAndColumn, MatchType.cross));

        audioManager.PlaySound(SoundEffects.powerup);
    }

    /// <summary>
    /// Power-up: Clears all items of one color.
    /// </summary>
    public void MatchEverythingByType(Matchable gem, int type)
    {
        Match everythingByType = new Match(gem);

        for (int y = 0; y != Dimensions.y; y++)
            for (int x = 0; x != Dimensions.x; x++)
                if (CheckBounds(x, y) && !IsEmpty(x, y) && GetItemAt(x, y).Idle && GetItemAt(x, y).Type == type)
                    everythingByType.AddMatchable(GetItemAt(x, y));

        StartCoroutine(score.ResolveMatch(everythingByType, MatchType.match5));
        StartCoroutine(FillAndScanGrid());

        audioManager.PlaySound(SoundEffects.powerup);
    }

    /// <summary>
    /// Power-up: Clears every single item currently on the grid.
    /// </summary>
    public void MatchEverything()
    {
        Match everything = new Match();

        for (int y = 0; y != Dimensions.y; y++)
            for (int x = 0; x != Dimensions.x; x++)
                if (CheckBounds(x, y) && !IsEmpty(x, y) && GetItemAt(x, y).Idle)
                    everything.AddMatchable(GetItemAt(x, y));

        StartCoroutine(score.ResolveMatch(everything, MatchType.match5));
        StartCoroutine(FillAndScanGrid());

        audioManager.PlaySound(SoundEffects.powerup);
    }

    /// <summary>
    /// Finds all possible moves for the player.
    /// </summary>
    private int ScanForMoves()
    {
        possibleMoves = new List<Matchable>();

        for (int y = 0; y != Dimensions.y; y++)
            for (int x = 0; x != Dimensions.x; x++)
                if (CheckBounds(x, y) && !IsEmpty(x, y) && CanMove(GetItemAt(x, y)))
                    possibleMoves.Add(GetItemAt(x, y));

        return possibleMoves.Count;
    }

    /// <summary>
    /// Checks if a tile has any valid match move.
    /// </summary>
    private bool CanMove(Matchable toCheck)
    {
        if(CanMove(toCheck, Vector2Int.up) || CanMove(toCheck, Vector2Int.down) || CanMove(toCheck, Vector2Int.right) || CanMove(toCheck, Vector2Int.left))
            return true;

        if (toCheck.IsGem)
            return true;

        return false;
    }

    /// <summary>
    /// Tests if moving a tile in a direction creates a match.
    /// </summary>
    private bool CanMove(Matchable toCheck, Vector2Int direction)
    {
        Vector2Int targetPos = toCheck.position + direction;
        Vector2Int position1 = targetPos + direction;
        Vector2Int position2 = targetPos + direction * 2;

        if(IsAPotentialMatch(toCheck, position1, position2))
            return true;

        Vector2Int cw = new Vector2Int(direction.y, -direction.x);
        Vector2Int ccw = new Vector2Int (-direction.y, direction.x);

        position1 = targetPos + cw;
        position2 = targetPos + cw * 2;

        if (IsAPotentialMatch(toCheck, position1, position2))
            return true;

        position1 = targetPos + ccw;
        position2 = targetPos + ccw * 2;

        if (IsAPotentialMatch(toCheck, position1, position2))
            return true;

        position1 = targetPos + cw;
        position2 = targetPos + ccw;

        if (IsAPotentialMatch(toCheck, position1, position2))
            return true;

        return false;
    }

    /// <summary>
    /// Checks if two positions have the same type as the target.
    /// </summary>
    private bool IsAPotentialMatch(Matchable toCompare, Vector2Int position1, Vector2Int position2)
    {
        if(
            CheckBounds(position1) && CheckBounds(position2)
            && !IsEmpty(position1) && !IsEmpty(position2) &&
            GetItemAt(position1).Idle && GetItemAt(position2).Idle
            && GetItemAt(position1).Type == toCompare.Type && GetItemAt(position2).Type == toCompare.Type
          )
            return true;

        return false;
    }
    public void ShowHint()
    {
        hint.IndicateHint(possibleMoves[Random.Range(0, possibleMoves.Count)].transform);
    }
}
