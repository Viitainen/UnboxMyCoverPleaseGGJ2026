using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoverOptionTextUI : CoverOptionUI
{
    [SerializeField]
    private TMP_Text optionText;

    [SerializeField]
    private Image borderImage;

    [SerializeField]
    private Color normalBorderColor;

    [SerializeField]
    private Color selectedBorderColor;

    [SerializeField]
    private GameObject selectedObject;

    public override void UpdateUI()
    {
        optionText.text = coverOptionData.description;
    }

    public override void ToggleSelected(bool isSelected)
    {
        if (selectedObject)
        {
            selectedObject.SetActive(isSelected);
        }

        if (borderImage)
        {
            borderImage.DOColor(isSelected ? selectedBorderColor : normalBorderColor, 0.15f);
        }
    }
}
