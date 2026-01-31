using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class CharacterData : ExpandableScriptableObject
{
    public string characterName;

    [TextArea()]
    public string problem;

    public Sprite baseImage;

    public int impactSuccessThreshold = 5;

    public List<CoverOptionImpact> coverOptionImpacts;


    public CoverOptionsResults GetCoverOptionsResults(List<CoverOptionData> selectedCoverOptions)
    {
        CoverOptionsResults results = new CoverOptionsResults();

        int totalImpact = 0;

        foreach (CoverOptionData selectedOption in selectedCoverOptions)
        {
            // Look for a matching impact entry
            CoverOptionImpact impact = coverOptionImpacts.Find(
                i => i.coverOption == selectedOption
            );

            if (impact != null)
            {
                totalImpact += impact.impactAmount;
            }
        }

        results.totalImpact = totalImpact;
        results.impactThreshold = impactSuccessThreshold;
        results.success = totalImpact >= impactSuccessThreshold;


        return results;
    }

}


