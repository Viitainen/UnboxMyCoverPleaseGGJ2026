using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{

    [SerializeField]
    private GameFlow gameFlow;

    [SerializeField]
    private Image baseImage;

    [SerializeField]
    private Image headwearImage;

    [SerializeField]
    private Image itemImage;

    private void Start()
    {
        gameFlow.OnCurrentHeadwearOptionChanged += OnCurrentHeadwearChanged;
        gameFlow.OnCurrentItemOptionChanged += OnCurrentItemChanged;
        // gameFlow.OnCurrentHeadwearOptionChanged += OnCurrentHeadwearChanged;
    }

    private void OnCurrentHeadwearChanged(CoverOptionData data)
    {
        ChangeHeadwearImage(data ? data.optionAvatarImage : null);
    }

    private void OnCurrentItemChanged(CoverOptionData data)
    {
        ChangeImageImage(data ? data.optionAvatarImage : null);
    }

    public void ChangeBaseImage(Sprite newSprite)
    {
        baseImage.sprite = newSprite;
    }

    public void ChangeHeadwearImage(Sprite newSprite)
    {
        headwearImage.sprite = newSprite;
    }

    public void ChangeImageImage(Sprite newSprite)
    {
        itemImage.sprite = newSprite;
    }

}
