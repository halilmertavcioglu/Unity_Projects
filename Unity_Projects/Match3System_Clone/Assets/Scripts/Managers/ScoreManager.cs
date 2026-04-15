using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Manages player score, combo multipliers, and the logic for clearing matches.
/// </summary>
public class ScoreManager : Singleton<ScoreManager>
{
    #region Variables

    [Header("Internal Systems (Auto-Assigned)")]
    [SerializeField] private MatchablePool pool;
    [SerializeField] private MatchableGrid grid;
    private AudioManager audioManager;

    [Header("UI & Animation References")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text comboText;
    [SerializeField] private Image ComboSlider;
    [SerializeField] private Transform collectionPoint;

    [Header("Data Monitoring")]
    [SerializeField] private int score;
    [SerializeField] private int comboMultiplier;

    [Header("Combo Timer Settings")]
    [SerializeField] private float timeSinceLastScore;
    [SerializeField] private float maxComboTime;
    [SerializeField] private float currentComboTime;
    [SerializeField] private bool timerIsActive;

    public int Score
    {
        get
        {
            return score;
        }
    }

    #endregion

    private void Start()
    {
        pool = (MatchablePool)MatchablePool.Instance;
        grid = (MatchableGrid)MatchableGrid.Instance;
        audioManager = AudioManager.Instance;

        comboText.enabled = false;
        ComboSlider.gameObject.SetActive(false);
    }
    /// <summary>
    /// Resets score and combo when starting a new game or retrying.
    /// </summary>
    public void Reset()
    {
        score = 0;
        scoreText.text = score.ToString();
        timeSinceLastScore = maxComboTime;
    }

    /// <summary>
    /// Increases score based on the current combo and resets the combo timer.
    /// </summary>
    public void AddScore(int amount)
    {
        score += amount * IncreaseCombo();
        scoreText.text = score.ToString();
        timeSinceLastScore = 0;

        if (!timerIsActive)
            StartCoroutine(ComboTimer());

        audioManager.PlaySound(SoundEffects.score);
    }

    /// <summary>
    /// Counts down the combo bar and resets the multiplier if time runs out.
    /// </summary>
    private IEnumerator ComboTimer()
    {
        timerIsActive = true;
        comboText.enabled = true;
        ComboSlider.gameObject.SetActive(true);

        do
        {
            timeSinceLastScore += Time.deltaTime;
            ComboSlider.fillAmount = 1 - timeSinceLastScore / currentComboTime;
            yield return null;
        }
        while (timeSinceLastScore < currentComboTime);

        comboMultiplier = 0;
        comboText.enabled = false;
        ComboSlider.gameObject.SetActive(false);
        timerIsActive = false;
    }

    /// <summary>
    /// Increases multiplier and makes the next combo harder by reducing the time window.
    /// </summary>
    private int IncreaseCombo()
    {
        comboText.text = "Combo x" + comboMultiplier++;
        currentComboTime = maxComboTime - Mathf.Log(comboMultiplier) / 2;
        return comboMultiplier;
    }

    /// <summary>
    /// Handles the removal of matched items and creates power-ups for large matches. 
    /// </summary>
    public IEnumerator ResolveMatch(Match toResolve, MatchType powerupUsed = MatchType.invalid)
    {
        Matchable powerupFormed = null;
        Matchable matchable;
        Transform target = collectionPoint;

        if (powerupUsed == MatchType.invalid && toResolve.Count > 3)
        {
            powerupFormed = pool.UpgradeMatchable(toResolve.ToBeUpgraded, toResolve.Type);
            toResolve.RemoveMatchable(powerupFormed);
            target = powerupFormed.transform;
            powerupFormed.SortingOrder = 3;

            audioManager.PlaySound(SoundEffects.upgrade);
        }
        else
            audioManager.PlaySound(SoundEffects.resolve);

        for (int i = 0; i != toResolve.Count; i++)
        {
            matchable = toResolve.Matchables[i];

            if (powerupUsed == MatchType.invalid && matchable.IsGem)
                continue;

            grid.RemoveItemAt(matchable.position);

            if (i == toResolve.Count - 1)
                yield return StartCoroutine(matchable.Resolve(target));

            else
                StartCoroutine(matchable.Resolve(target));
        }
        AddScore(toResolve.Count * toResolve.Count);

        if(powerupFormed != null)
            powerupFormed.SortingOrder = 1;
    }
}
