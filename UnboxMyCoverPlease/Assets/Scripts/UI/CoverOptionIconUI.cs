using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CoverOptionIconUI : CoverOptionUI, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField]
    private Image iconImage;

    [SerializeField]
    private Image borderImage;

    [SerializeField]
    private Sprite normalBorderSprite;

    [SerializeField]
    private Sprite selectedBorderSprite;

    public override void ToggleSelected(bool isSelected)
    {
        borderImage.sprite = isSelected ? selectedBorderSprite : normalBorderSprite;
    }

    public override void UpdateUI()
    {
        iconImage.sprite = coverOptionData ? coverOptionData.optionIcon : null;
        iconImage.enabled = iconImage.sprite != null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }
}
