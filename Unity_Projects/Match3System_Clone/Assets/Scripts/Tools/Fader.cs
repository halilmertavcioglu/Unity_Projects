using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls UI transparency for smooth fade-in and fade-out effects.
/// </summary>
[RequireComponent(typeof(Image))]
public class Fader : MonoBehaviour
{
    #region Variables

    [Header("Fade Settings")]
    [SerializeField] private float fadeSpeed = 1;

    [Header("Visual References")]
    private Image toFade;
    private Color faded;

    #endregion

    private void Awake()
    {
        toFade = GetComponent<Image>();
        faded = toFade.color;
    }

    public void Hide(bool hidden)
    {
        toFade.enabled = !hidden;
    }

    /// <summary>
    /// Smoothly changes the image transparency over time.
    /// </summary>
    public IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = faded.a;
        float t = 0;

        do
        {
            t += Time.deltaTime * fadeSpeed;
            if (t > 1)
                t = 1;

            faded.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            toFade.color = faded;
            yield return null;

        } while (t != 1);
    }
}
