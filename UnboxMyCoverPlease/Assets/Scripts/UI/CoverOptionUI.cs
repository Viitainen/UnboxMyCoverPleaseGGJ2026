using UnityEngine;

public class CoverOptionUI : MonoBehaviour
{
    [SerializeField]
    private CoverOptionData coverOptionData;

    [SerializeField]
    private GameObject optionParent;

    public CoverOptionData CoverOptionData
    {
        get
        {
            return coverOptionData;
        }
    }

    public void ToggleVisibility(bool isVisible, bool animate)
    {
        optionParent.SetActive(isVisible);
    }
}
