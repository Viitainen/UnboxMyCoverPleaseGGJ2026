using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CoverOptionUI : MonoBehaviour
{
    [SerializeField]
    protected CoverOptionData coverOptionData;

    [SerializeField]
    protected GameObject optionParent;

    [SerializeField]
    private Button optionButton;

    public Action<CoverOptionUI, CoverOptionData> OnOptionSelected;

    public CoverOptionData CoverOptionData
    {
        get
        {
            return coverOptionData;
        }
    }

    public virtual void Start()
    {
        optionButton.onClick.AddListener(OnButtonClicked);
        UpdateUI();
    }

    private void OnButtonClicked()
    {
        OnOptionSelected?.Invoke(this, coverOptionData);
    }

    public virtual void ToggleUsable(bool isUsable, bool animate)
    {
        optionButton.interactable = isUsable;
    }

    public virtual void ToggleSelected(bool isSelected)
    {
        
    }

    public virtual void UpdateUI()
    {
        
    }
}
