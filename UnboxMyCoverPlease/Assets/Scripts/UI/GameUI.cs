using System;
using TMPro;
using UnityEngine;

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

    private void Awake()
    {
        if (gameFlow)
        {
            gameFlow.OnCharacterChanged += OnCharacterChanged;
            gameFlow.OnCharacterDone += OnCharacterDone;
        }
    }

    private void OnCharacterDone(CharacterDone characterDone)
    {
        leftSideUI.ClearCharacter();
    }

    private void OnCharacterChanged(CharacterData characterData)
    {
        UpdateCharacterNumber(gameFlow.currentCharacterIndex + 1, gameFlow.CharacterCount);

        leftSideUI.ShowCharacter(characterData);
    }

    public void UpdateCharacterNumber(int number, int characterCount)
    {
        characterNumberText.text = $"{number}/{characterCount}";
    }
}
