using System;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public int currentCharacterIndex = -1;

    public CharacterData currentCharacter;

    public List<CharacterData> characters;

    public Action<CharacterData> OnCharacterChanged;

    public Action<CharacterDone> OnCharacterDone;

    public int CharacterCount { get { return characters.Count; } }


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

    public CharacterData AdvanceToNextCharacter(out bool hadCharacter)
    {
        SaveCurrentCharacterAsDone();

        currentHeadwearOption = null;
        currentItemOption = null;
        currentInstructionOption = null;

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

            characterDones.Add(characterDone);

            return true;
        }

        return false;
    }
}
