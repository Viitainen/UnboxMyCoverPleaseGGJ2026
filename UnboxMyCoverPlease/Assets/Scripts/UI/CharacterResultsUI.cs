using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CharacterResultsUI : MonoBehaviour
{
    [SerializeField]
    private GameFlow gameFlow;

    [SerializeField]
    private DialogUI dialogUI;

    [SerializeField]
    private AudioManager audioManager;

    [SerializeField]
    private AudioClip successAudioClip;

    [SerializeField]
    private AudioClip failAudioClip;

    [SerializeField]
    private Button nextResultButton;

    private void OnEnable()
    {
        gameFlow.OnCharacterResultChanged += OnCharacterResultChanged;
        gameFlow.OnAllResultsDone += OnAllResultsDone;
        nextResultButton.onClick.AddListener(OnNextResultButtonClicked);
    }

    private void OnDisable()
    {
        gameFlow.OnCharacterResultChanged -= OnCharacterResultChanged;
        gameFlow.OnAllResultsDone -= OnAllResultsDone;
        nextResultButton.onClick.RemoveListener(OnNextResultButtonClicked);
    }


    private void OnNextResultButtonClicked()
    {
        gameFlow.AdvanceToNextResult(out _);
    }

    private void OnAllResultsDone()
    {
        nextResultButton.gameObject.SetActive(false);
    }

    private void OnCharacterResultChanged(CharacterDone characterDone)
    {
        string endDialog = characterDone != null ? characterDone.GetEndDialog : "";

        dialogUI.ShowDialog(endDialog);

        if (audioManager && characterDone != null)
        {
            DOVirtual.DelayedCall(1.75f, () =>
            {

                AudioClip audioClip = characterDone.results.success ? successAudioClip : failAudioClip;

                if (audioClip)
                {
                    audioManager.PlayEffect(audioClip, 0.4f);
                }
            });
        }

    }
}
