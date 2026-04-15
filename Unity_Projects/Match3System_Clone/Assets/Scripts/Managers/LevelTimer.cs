using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the level timer, updates the UI, and ends the game when time is up.
/// </summary>
public class LevelTimer : MonoBehaviour
{
    #region Variables

    [Header("Settings & State")]
    private float timeRemaining;
    private string timeAsString;

    private GameManager gameManager;
    [SerializeField] private Text timerText;

    #endregion

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    /// <summary>
    /// Starts the countdown with a set time.
    /// </summary>
    public void SetTimer(float t)
    {
        StopAllCoroutines();
        timeRemaining = t;
        UpdateText();
    }

    /// <summary>
    /// Formats the remaining time into a MM:SS string for the UI.
    /// </summary>
    private void UpdateText()
    {
        timeAsString = (int) timeRemaining / 60 + " : ";
        timeAsString += timeRemaining % 60 < 10 ? "0" : "";
        timerText.text = timeAsString + (int) timeRemaining % 60;
    }

    /// <summary>
    /// Coroutine that runs the countdown
    /// </summary>
    public IEnumerator Countdown()
    {
        do
        {
            timeRemaining -= Time.deltaTime;
            UpdateText();
            yield return null;

        } while (timeRemaining > 0);

        gameManager.GameOver();
    }
}
