using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoxContentUI : MonoBehaviour
{
    [SerializeField]
    private List<CoverOptionUI> coverOptionUIs;

    public Action<CoverOptionUI, CoverOptionData> OnOptionSelected;

    private CoverOptionUI selectedCoverOptionUI;

    public void Start()
    {
        AddOptionSelectedListeners();
    }

    private void AddOptionSelectedListeners()
    {
        foreach (CoverOptionUI option in coverOptionUIs)
        {
            option.OnOptionSelected += OnOptionSelectedHandler;
        }
    }

    private void OnOptionSelectedHandler(CoverOptionUI coverOptionUI, CoverOptionData coverOptionData)
    {
        if (selectedCoverOptionUI != coverOptionUI)
        {
            if (selectedCoverOptionUI)
            {
                selectedCoverOptionUI.ToggleSelected(false);
            }
        }

        selectedCoverOptionUI = coverOptionUI;
        selectedCoverOptionUI.ToggleSelected(true);

        OnOptionSelected?.Invoke(coverOptionUI, coverOptionData);
    }

    public void RemoveCoverOption(CoverOptionData coverOptionDataToRemove)
    {
        foreach (CoverOptionUI coverOptionUI in coverOptionUIs)
        {
            if (coverOptionUI.CoverOptionData == coverOptionDataToRemove)
            {
                coverOptionUI.ToggleUsable(false, true);
            }
        }
    }

    public void ToggleAll(bool isVisible, bool animate)
    {
        foreach (CoverOptionUI coverOptionUI in coverOptionUIs)
        {
            coverOptionUI.ToggleUsable(isVisible, animate);
        }
    }

    public void DeselectSelected()
    {
        if (selectedCoverOptionUI)
        {
            selectedCoverOptionUI.ToggleSelected(false);
        }
    }
}
