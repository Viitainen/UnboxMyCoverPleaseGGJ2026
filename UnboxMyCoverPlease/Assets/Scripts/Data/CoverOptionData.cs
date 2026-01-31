using UnityEngine;


[CreateAssetMenu(fileName = "New Cover Option", menuName = "Cover Options/Cover Option")]
public class CoverOptionData : ExpandableScriptableObject
{
    public string optionName;

    [TextArea()]
    public string description;

    public Sprite optionIcon;

    public Sprite optionAvatarImage;

}


