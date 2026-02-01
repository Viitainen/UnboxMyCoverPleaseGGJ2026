using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{

    [SerializeField]
    private GameFlow gameFlow;

    [SerializeField]
    private AudioManager audioManager;

    [SerializeField]
    private Image baseImage;

    [SerializeField]
    private Image headwearImage;

    [SerializeField]
    private Image itemImage;

    private void OnEnable()
    {
        CheckImagesEnabled();
        if (gameFlow)
        {
            gameFlow.OnCharacterChanged += OnCharacterChanged;
            gameFlow.OnCurrentHeadwearOptionChanged += OnCurrentHeadwearChanged;
            gameFlow.OnCurrentItemOptionChanged += OnCurrentItemChanged;
            // gameFlow.OnCurrentInstructionOptionChanged += OnCurrentInstructionChanged;
        }

    }

    private void OnCharacterChanged(CharacterData data)
    {
        ChangeBaseImage(data ? data.baseImage : null);
        
        if (audioManager && data && data.introductionAudio)
        {
            audioManager.PlayEffect(data.introductionAudio);
        }
    }

    private void OnDisable()
    {
        if (gameFlow)
        {
            gameFlow.OnCharacterChanged -= OnCharacterChanged;
            gameFlow.OnCurrentHeadwearOptionChanged -= OnCurrentHeadwearChanged;
            gameFlow.OnCurrentItemOptionChanged -= OnCurrentItemChanged;
        }
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
        baseImage.enabled = baseImage.sprite != null;
    }

    public void ChangeHeadwearImage(Sprite newSprite)
    {
        headwearImage.sprite = newSprite;
        headwearImage.enabled = headwearImage.sprite != null;

    }

    public void ChangeImageImage(Sprite newSprite)
    {
        itemImage.sprite = newSprite;
        itemImage.enabled = itemImage.sprite != null;
    }

    public void CheckImagesEnabled()
    {
        baseImage.enabled = baseImage.sprite != null;
        headwearImage.enabled = headwearImage.sprite != null;
        itemImage.enabled = itemImage.sprite != null;
    }

}
