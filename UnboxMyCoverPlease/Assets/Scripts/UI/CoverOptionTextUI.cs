using TMPro;
using UnityEngine;

public class CoverOptionTextUI : CoverOptionUI
{
    [SerializeField]
    private TMP_Text optionText;

    public override void UpdateUI()
    {
        optionText.text = coverOptionData.description;
    }
}
