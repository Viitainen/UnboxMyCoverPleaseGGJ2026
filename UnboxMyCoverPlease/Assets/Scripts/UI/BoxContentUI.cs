using System.Collections.Generic;
using UnityEngine;

public abstract class BoxContentUI : MonoBehaviour
{
    [SerializeField]
    private List<CoverOptionUI> coverOptionUIs;

    public void RemoveCoverOption(CoverOptionData coverOptionDataToRemove)
    {
        foreach(CoverOptionUI coverOptionUI in coverOptionUIs)
        {
            if (coverOptionUI.CoverOptionData == coverOptionDataToRemove)
            {
                coverOptionUI.ToggleVisibility(false, true);
            }
        }
    }

    public void ToggleAll(bool isVisible, bool animate)
    {
        foreach(CoverOptionUI coverOptionUI in coverOptionUIs)
        {
            coverOptionUI.ToggleVisibility(isVisible, animate);
        }
    }
}
