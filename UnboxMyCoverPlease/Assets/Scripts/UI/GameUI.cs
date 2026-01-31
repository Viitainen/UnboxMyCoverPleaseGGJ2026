using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{

    [SerializeField]
    private GameFlow gameFlow;

    [SerializeField]
    private LeftSideUI leftSideUI;

    [SerializeField]
    private BoxUI boxUI;

    [SerializeField]
    private TMP_Text characterNumberText;

    [SerializeField]
    private Button readyButton;

    private void Awake()
    {
        if (gameFlow)
        {
            gameFlow.OnCharacterChanged += OnCharacterChanged;
            gameFlow.OnCharacterDone += OnCharacterDone;
        }
    }

    private void Start()
    {
        readyButton.onClick.AddListener(OnReadyButtonClicked);
    }

    private void OnReadyButtonClicked()
    {
        gameFlow.AdvanceToNextCharacter(out _);
    }

    private void OnCharacterDone(CharacterDone characterDone)
    {
        leftSideUI.ClearCharacter();
    }

    private void OnCharacterChanged(CharacterData characterData)
    {
        UpdateCharacterNumber(gameFlow.CurrentCharacterIndex + 1, gameFlow.CharacterCount);

        leftSideUI.ShowCharacter(characterData);
    }

    public void UpdateCharacterNumber(int number, int characterCount)
    {
        characterNumberText.text = $"{number}/{characterCount}";
    }
}
