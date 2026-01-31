using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoxContentUI : MonoBehaviour
{
    [SerializeField]
    private List<CoverOptionUI> coverOptionUIs;

    public Action<CoverOptionUI, CoverOptionData> OnOptionSelected;


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
}
