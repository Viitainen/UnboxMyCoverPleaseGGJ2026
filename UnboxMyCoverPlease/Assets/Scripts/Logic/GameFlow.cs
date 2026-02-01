using System;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public List<CharacterData> characters;

    public Action<CharacterData> OnCharacterChanged;

    public Action<CharacterDone> OnCharacterDone;

    public Action OnAllCharactersDone;

    private int currentCharacterIndex = -1;

    private CharacterData currentCharacter;

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

        ChangeHeadwearOption(null);
        ChangeItemOption(null);
        ChangeInstructionOption(null);

        currentCharacterIndex++;
        hadCharacter = currentCharacterIndex < characters.Count;

        if (hadCharacter)
        {
            // IS OK
            currentCharacter = characters[currentCharacterIndex];
        }
        else
        {
            currentCharacter = null;
            // Go to end results?
            OnAllCharactersDone?.Invoke();
        }

        OnCharacterChanged?.Invoke(currentCharacter);
        return currentCharacter;
    }

    private bool SaveCurrentCharacterAsDone()
    {
        if (currentCharacterIndex > -1)
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
}
