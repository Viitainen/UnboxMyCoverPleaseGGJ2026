using UnityEngine;

public class LeftSideUI : MonoBehaviour
{
    [SerializeField]
    private CharacterUI characterUI;
    
    [SerializeField]
    private DialogUI dialogUI;

    public void ShowCharacter(CharacterData characterData)
    {
        characterUI.ChangeBaseImage(characterData.baseImage);
        dialogUI.ShowDialog(characterData.problem);
    }

    public void ClearCharacter()
    {
        characterUI.ChangeBaseImage(null);
        dialogUI.ClearDialog();
    }
}
