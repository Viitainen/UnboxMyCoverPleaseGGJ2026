using System;
using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameFlow gameFlow;

    [SerializeField]
    private GameObject gameUI;

    [SerializeField]
    private GameObject endResultsUI;

    [SerializeField]
    private UIFader fader;

    [SerializeField]
    private float fadeOutDuration = 1f;

    [SerializeField]
    private float fadeInDuration = 2f;

    private void OnEnable()
    {
        gameFlow.OnAllCharactersDone += OnAllCharactersDone;
        gameFlow.OnAllResultsDone += OnAllResultsDone;
    }

    private void OnDisable()
    {
        gameFlow.OnAllCharactersDone -= OnAllCharactersDone;
        gameFlow.OnAllResultsDone -= OnAllResultsDone;

    }

    private void Start()
    {
        fader.SetOut();
        fader.FadeIn(fadeInDuration);
    }

    private void OnAllCharactersDone()
    {
        StartCoroutine(EndResultsSequence());
    }


    private void OnAllResultsDone()
    {
        StartCoroutine(EndOfEndResultsSequence());
    }

    private IEnumerator EndResultsSequence()
    {
        // Fade to black
        fader.FadeOut(fadeOutDuration);
        yield return new WaitForSecondsRealtime(fadeOutDuration);

        // Switch UI
        gameUI.SetActive(false);
        endResultsUI.SetActive(true);

        // Fade back in
        fader.FadeIn(fadeInDuration);
    }

    private IEnumerator EndOfEndResultsSequence()
    {
        // Fade to black
        fader.FadeOut(fadeOutDuration);
        yield return new WaitForSecondsRealtime(fadeOutDuration);
        
        yield return new WaitForSecondsRealtime(2f);

        // End screen?
        CustomSceneManager.Instance.LoadMainMenuScene();
    }
}