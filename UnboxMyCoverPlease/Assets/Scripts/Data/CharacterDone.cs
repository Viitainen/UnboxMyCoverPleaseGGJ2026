using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterDone
{
    public CharacterData character;

    public CoverOptionData headwearSelection;

    public CoverOptionData itemSelection;

    public CoverOptionData instructionSelection;

    public List<CoverOptionData> GetCoverOptions
    {
        get
        {
            return new List<CoverOptionData>() { headwearSelection, itemSelection, instructionSelection };
        }
    }
}
