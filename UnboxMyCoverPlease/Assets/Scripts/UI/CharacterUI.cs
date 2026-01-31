using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    [SerializeField]
    private Image baseImage;

    [SerializeField]
    private Image headwearImage;

    [SerializeField]
    private Image toolImage;

    public void ChangeBaseImage(Sprite newSprite)
    {
        baseImage.sprite = newSprite;
    }

    public void ChangeHeadwearImage(Sprite newSprite)
    {
        headwearImage.sprite = newSprite;
    }

    public void ChangeToolImage(Sprite newSprite)
    {
        toolImage.sprite = newSprite;
    }

}
