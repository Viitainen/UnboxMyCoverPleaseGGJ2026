using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameFlow : MonoBehaviour
{

    [SerializeField]
    private AudioManager audioManager;


    public List<CharacterData> characters;

    public Action<CharacterData> OnCharacterChanged;

    public Action<CharacterDone> OnCharacterDone;

    public Action<CharacterDone> OnCharacterResultChanged;

    public Action OnAllCharactersDone;

    public Action OnAllResultsDone;


    private int currentCharacterIndex = -1;

    private CharacterData currentCharacter;

    private CharacterDone currentCharacterResult;

    public int CharacterCount { get { return characters.Count; } }

    public int CurrentCharacterIndex { get { return currentCharacterIndex; } }

    public CharacterData CurrentCharacter { get { return currentCharacter; } }


    public Action<CoverOptionData> OnCurrentHeadwearOptionChanged;
    public Action<CoverOptionData> OnCurrentItemOptionChanged;
    public Action<CoverOptionData> OnCurrentInstructionOptionChanged;


    private CoverOptionData currentHeadwearOption;
    private CoverOptionData currentItemOption;
    private CoverOptionData currentInstructionOption;

    private List<CharacterDone> characterDones;

    public void Start()
    {
        StartGame();
        characterDones = new List<CharacterDone>();
    }

    public void StartGame()
    {
        // Show start place?
        AdvanceToNextCharacter(out _);
    }

    public void ChangeHeadwearOption(CoverOptionData coverOption)
    {
        currentHeadwearOption = coverOption;
        OnCurrentHeadwearOptionChanged?.Invoke(currentHeadwearOption);
    }

    public void ChangeItemOption(CoverOptionData coverOption)
    {
        currentItemOption = coverOption;
        OnCurrentItemOptionChanged?.Invoke(currentItemOption);
    }

    public void ChangeInstructionOption(CoverOptionData coverOption)
    {
        currentInstructionOption = coverOption;
        OnCurrentInstructionOptionChanged?.Invoke(currentInstructionOption);
    }

    public CharacterData AdvanceToNextCharacter(out bool hadCharacter)
    {
        if (currentCharacter)
        {
            if (!currentHeadwearOption || !currentItemOption || !currentInstructionOption)
            {
                hadCharacter = false;
                return null;
            }
        }

        SaveCurrentCharacterAsDone();



        currentCharacterIndex++;
        hadCharacter = currentCharacterIndex < characters.Count;

        if (hadCharacter)
        {
            ChangeHeadwearOption(null);
            ChangeItemOption(null);
            ChangeInstructionOption(null);

            // IS OK
            currentCharacter = characters[currentCharacterIndex];
        }
        else
        {
            InitiateResults();
        }

        OnCharacterChanged?.Invoke(currentCharacter);
        return currentCharacter;
    }

    public void AdvanceToNextResult(out bool hadResult)
    {
        currentCharacterIndex++;
        hadResult = currentCharacterIndex < characterDones.Count;

        if (hadResult)
        {
            // IS OK
            CharacterDone characterDone = characterDones[currentCharacterIndex];
            currentCharacterResult = characterDone;
            currentCharacter = characterDone.character;

            ChangeHeadwearOption(characterDone.headwearSelection);
            ChangeItemOption(characterDone.itemSelection);
            ChangeInstructionOption(characterDone.instructionSelection);

            OnCharacterResultChanged?.Invoke(currentCharacterResult);
            OnCharacterChanged?.Invoke(currentCharacter);
        }
        else
        {
            // All results done
            OnAllResultsDone?.Invoke();
        }
    }

    private bool SaveCurrentCharacterAsDone()
    {
        if (currentCharacterIndex > -1 && currentCharacterIndex < CharacterCount)
        {
            CharacterDone characterDone = new CharacterDone();

            characterDone.character = currentCharacter;
            characterDone.headwearSelection = currentHeadwearOption;
            characterDone.itemSelection = currentItemOption;
            characterDone.instructionSelection = currentInstructionOption;

            CoverOptionsResults results = currentCharacter.GetCoverOptionsResults(characterDone.GetCoverOptions);

            characterDone.results = results;

            OnCharacterDone?.Invoke(characterDone);
            characterDones.Add(characterDone);

            return true;
        }

        return false;
    }


    private void InitiateResults()
    {
        // currentCharacter = null;
        // Go to end results?
        currentCharacterIndex = -1;
        OnAllCharactersDone?.Invoke();

        if (audioManager)
        {
            audioManager.ChangeMusicToResults(1.5f);
        }

        DOVirtual.DelayedCall(1.04f, () =>
        {
            AdvanceToNextResult(out _);
        });
    }
}
