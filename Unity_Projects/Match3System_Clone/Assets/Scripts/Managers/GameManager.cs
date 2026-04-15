using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net.Sockets;

public class GameManager : Singleton<GameManager>
{
    #region Variables

    private MatchablePool pool;
    private MatchableGrid grid;
    private Cursor cursor;
    private AudioManager audioManager;
    private ScoreManager score;

    [Header("UI & Visual Effects")]
    [SerializeField] private Fader loadingScreen;
    [SerializeField] private Fader darkener;
    [SerializeField] private Text finalScoreText;
    [SerializeField] private Movable resultPage;

    [Header("Level Settings")]
    [SerializeField] private bool levelIsTimed;
    [SerializeField] private LevelTimer timer;
    [SerializeField] private float timeLimit;

    [Header("Grid Configuration")]
    [SerializeField] private Vector2Int dimensions = Vector2Int.one;

    #endregion

    private void Start()
    {
        pool = (MatchablePool) MatchablePool.Instance;
        grid = (MatchableGrid) MatchableGrid.Instance;
        cursor = Cursor.Instance;
        audioManager = AudioManager.Instance;
        score = ScoreManager.Instance;

        StartCoroutine(Setup());
    }

    /// <summary>
    /// Prepares the level, pools objects, and starts the gameplay.
    /// </summary>
    private IEnumerator Setup()
    {
        cursor.enabled = false;
        loadingScreen.Hide(false);

        if(levelIsTimed)
            timer.SetTimer(timeLimit);

        pool.PoolObjects(dimensions.x * dimensions.y * 2);
        grid.InitializeGrid(dimensions);

        StartCoroutine(loadingScreen.Fade(0));
        audioManager.PlayMusic();

        yield return StartCoroutine(grid.PopulatedGrid(false, true));

        grid.CheckPossibleMoves();
        cursor.enabled = true;

        if (levelIsTimed)
            StartCoroutine(timer.Countdown());
    }

    /// <summary>
    /// Decides what happens when no matches are possible on the board.
    /// </summary>
    public void NoMoreMoves()
    {
        if (levelIsTimed)
            grid.MatchEverything();

        else
            GameOver();
    }

    /// <summary>
    /// Shows the result screen and stops the game.
    /// </summary>
    public void GameOver()
    {
        finalScoreText.text = score.Score.ToString();
        cursor.enabled = false;

        darkener.Hide(false);
        StartCoroutine(darkener.Fade(0.75f));

        StartCoroutine(resultPage.MoveToPosition(new Vector2(Screen.width / 2, Screen.height / 2)));
    }

    #region Scene Transitions

    private IEnumerator Quit()
    {
        yield return StartCoroutine(loadingScreen.Fade(1));
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitButtonPressed()
    {
        StartCoroutine(Quit());
    }

    /// <summary>
    /// Cleans up the current board and restarts the level without reloading the scene.
    /// </summary>
    private IEnumerator Retry()
    {
        Vector2 offScreenPosition = new Vector2(Screen.width / 2f, Screen.height / 2f) + Vector2.down * 1000f;
        StartCoroutine(resultPage.MoveToPosition(offScreenPosition));

        yield return StartCoroutine(darkener.Fade(0));
        darkener.Hide(true);

        if (levelIsTimed)
            timer.SetTimer(timeLimit);

        cursor.Reset();
        score.Reset();
        yield return StartCoroutine(grid.Reset());

        cursor.enabled = true;

        if (levelIsTimed)
            StartCoroutine(timer.Countdown());
    }
    public void RetryButtonPressed()
    {
        StartCoroutine(Retry());
    }

    #endregion
}
