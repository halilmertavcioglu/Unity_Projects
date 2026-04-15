using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles button clicks and changes scenes with a fade effect.
/// </summary>
public class MenuManager : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private Fader loadingScreen;

    private void Start()
    {
        loadingScreen.Hide(false);
        StartCoroutine(loadingScreen.Fade(0));
    }

    #region Scene Logic

    //These methods wait for the fade animation to finish before doing the action.
    private IEnumerator Quit()
    {
        yield return StartCoroutine(loadingScreen.Fade(1));
        Application.Quit();
    }
    private IEnumerator StartSurvivalGame()
    {
        yield return StartCoroutine(loadingScreen.Fade(1));
        SceneManager.LoadScene("Survival");
    }
    private IEnumerator StartTimeRushGame()
    {
        yield return StartCoroutine(loadingScreen.Fade(1));
        SceneManager.LoadScene("TimeRush");
    }
    public void QuitButtonPressed()
    {
        StartCoroutine(Quit());
    }
    public void SurvivalButtonPressed()
    {
        StartCoroutine(StartSurvivalGame());
    }
    public void TimeRushButtonPressed()
    {
        StartCoroutine(StartTimeRushGame());
    }

    #endregion
}
