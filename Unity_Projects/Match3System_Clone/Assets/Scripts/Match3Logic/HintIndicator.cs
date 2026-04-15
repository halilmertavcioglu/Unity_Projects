using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages visual hints to help the player find moves.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class HintIndicator : Singleton<HintIndicator>
{
    #region Variables

    [Header("Auto Hint Settings")]
    [SerializeField] private float delayBeforeAutoHint;
    private Transform hintLocation;
    private Coroutine autoHintCR;

    [Header("Visuals & UI")]
    [SerializeField] private Button hintButton;
    private SpriteRenderer spriteRenderer;

    #endregion

    /// <summary>
    /// Sets up the initial hidden state of the hint system.
    /// </summary>
    protected override void Init()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        hintButton.interactable = false;
    }

    /// <summary>
    /// Shows a visual indicator at a specific location.
    /// </summary>
    public void IndicateHint(Transform hintLocation)
    {
        CancelHint();
        transform.position = hintLocation.position;
        spriteRenderer.enabled = true;
    }

    /// <summary>
    /// Hides the hint and stops any active timers.
    /// </summary>
    public void CancelHint()
    {
        spriteRenderer.enabled = false;
        hintButton.interactable = false;

        if(autoHintCR != null)
            StopCoroutine(autoHintCR);

        autoHintCR = null;
    }

    /// <summary>
    /// Makes the hint button clickable.
    /// </summary>
    public void EnableHintButton()
    {
        hintButton.interactable = true;
    }

    /// <summary>
    /// Starts a timer to show a hint automatically.
    /// </summary>
    public void StartAutoHint(Transform hintLocation)
    {
        this.hintLocation = hintLocation;
        autoHintCR = StartCoroutine(WaitAndIndicateHint());
    }

    /// <summary>
    /// Wait logic: Waits for a delay then enables the hint button.
    /// </summary>
    private IEnumerator WaitAndIndicateHint()
    {
        yield return new WaitForSeconds(delayBeforeAutoHint);
        EnableHintButton();
    }
}
