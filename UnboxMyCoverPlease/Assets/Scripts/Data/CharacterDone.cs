using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterDone
{
    public CharacterData character;

    public CoverOptionData headwearSelection;

    public CoverOptionData itemSelection;

    public CoverOptionData instructionSelection;

    public CoverOptionsResults results;

    public List<CoverOptionData> GetCoverOptions
    {
        get
        {
            return new List<CoverOptionData>() { headwearSelection, itemSelection, instructionSelection };
        }
    }

    public string GetEndDialog
    {
        get
        {
            return results.success ? character.successDialog : character.failDialog;
        }
    }

}
