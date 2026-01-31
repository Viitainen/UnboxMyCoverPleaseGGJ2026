using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text dialogText;

    [SerializeField]
    private TextTypewriterEffect typewriterEffect;

    public void ShowDialog(string dialog)
    {
        dialogText.text = dialog;

        if (typewriterEffect)
        {
            typewriterEffect.StartTypewriterEffect();
        }
    }

    public void ClearDialog()
    {
        dialogText.text = "";
    }
}
