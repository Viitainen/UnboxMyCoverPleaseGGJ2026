using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextTypewriterEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textEl;
    [SerializeField] private float TypingSpeed = 0.05f; // Time delay between characters

    private bool useMaskedEffect = false;


    private string fullText;
    private string maskedText;
    private static readonly Dictionary<char, char> maskMapping = new Dictionary<char, char>
    {
        {'A', 'V'}, {'B', 'E'}, {'C', 'O'}, {'D', 'U'}, {'E', 'R'}, {'F', 'B'}, {'G', 'C'}, {'H', 'N'},
        {'I', 'N'}, {'J', 'L'}, {'K', 'X'}, {'L', 'J'}, {'M', 'H'}, {'N', 'I'}, {'O', 'G'}, {'P', 'F'},
        {'Q', 'O'}, {'R', 'B'}, {'S', 'Z'}, {'T', 'J'}, {'U', 'D'}, {'V', 'Y'}, {'W', 'A'}, {'X', 'K'},
        {'Y', 'V'}, {'Z', 'S'}, {'a', 'o'}, {'b', 'k'}, {'c', 'a'}, {'d', 'p'}, {'e', 'a'}, {'f', 't'},
        {'g', 'q'}, {'h', 'k'}, {'i', 'l'}, {'j', 'h'}, {'k', 'b'}, {'l', 'v'}, {'m', 'u'}, {'n', 'm'},
        {'o', 'a'}, {'p', 'b'}, {'q', 'c'}, {'r', 'd'}, {'s', 'e'}, {'t', 'f'}, {'u', 'n'}, {'v', 'x'},
        {'w', 'x'}, {'x', 'w'}, {'y', 'g'}, {'z', 's'}, {'0', '8'}, {'1', '7'}, {'2', '5'}, {'3', '6'},
        {'4', '7'}, {'5', '0'}, {'6', '9'}, {'7', '4'}, {'8', '3'}, {'9', '3'}
    };

    // private static readonly Dictionary<char, char> maskMapping = new Dictionary<char, char>
    // {
    //     {'A', 'M'}, {'B', 'N'}, {'C', 'O'}, {'D', 'P'}, {'E', 'Q'}, {'F', 'R'}, {'G', 'S'}, {'H', 'T'},
    //     {'I', 'U'}, {'J', 'V'}, {'K', 'W'}, {'L', 'X'}, {'M', 'Y'}, {'N', 'Z'}, {'O', 'A'}, {'P', 'B'},
    //     {'Q', 'C'}, {'R', 'D'}, {'S', 'E'}, {'T', 'F'}, {'U', 'G'}, {'V', 'H'}, {'W', 'I'}, {'X', 'J'},
    //     {'Y', 'K'}, {'Z', 'L'}, {'a', 'm'}, {'b', 'n'}, {'c', 'o'}, {'d', 'p'}, {'e', 'q'}, {'f', 'r'},
    //     {'g', 's'}, {'h', 't'}, {'i', 'u'}, {'j', 'v'}, {'k', 'w'}, {'l', 'x'}, {'m', 'y'}, {'n', 'z'},
    //     {'o', 'a'}, {'p', 'b'}, {'q', 'c'}, {'r', 'd'}, {'s', 'e'}, {'t', 'f'}, {'u', 'g'}, {'v', 'h'},
    //     {'w', 'i'}, {'x', 'j'}, {'y', 'k'}, {'z', 'l'}, {'0', '5'}, {'1', '6'}, {'2', '7'}, {'3', '8'},
    //     {'4', '9'}, {'5', '0'}, {'6', '1'}, {'7', '2'}, {'8', '3'}, {'9', '4'}
    // };


    public float AnimationDuration { get { return fullText.Length * TypingSpeed; } }

    void Awake()
    {
        if (textEl == null)
        {
            textEl = GetComponent<TextMeshProUGUI>();
        }

        ResetEffect();
    }

    public void StartTypewriterEffect(float initialDelay = 0f)
    {
        StopAllCoroutines();
        StartCoroutine(useMaskedEffect ? TypeMaskedText(initialDelay) : TypeText(initialDelay));
    }

    public void SetSpeed(float speed)
    {
        TypingSpeed = speed;
    }

    private void SetVisibleCharacters(int charCount)
    {
        textEl.maxVisibleCharacters = charCount;
    }

    private IEnumerator TypeMaskedText(float initialDelay)
    {
        fullText = textEl.text; // Store the full text
        maskedText = GenerateMaskedText(fullText);
        textEl.text = maskedText; // Show masked text first
        SetVisibleCharacters(fullText.Length);

        if (initialDelay > 0)
        {
            yield return new WaitForSeconds(initialDelay);
        }

        char[] revealedText = maskedText.ToCharArray();
        for (int i = 0; i < fullText.Length; i++)
        {
            revealedText[i] = fullText[i]; // Replace masked character with actual character
            textEl.text = new string(revealedText);
            yield return new WaitForSeconds(TypingSpeed);
        }
    }

    private IEnumerator TypeText(float initialDelay)
    {
        fullText = textEl.text; // Store the full text
        SetVisibleCharacters(0);

        if (initialDelay > 0)
        {
            yield return new WaitForSeconds(initialDelay);
        }

        for (int i = 0; i <= fullText.Length; i++)
        {
            SetVisibleCharacters(i);
            yield return new WaitForSeconds(TypingSpeed);
        }
    }

    private string GenerateMaskedText(string text)
    {
        char[] masked = new char[text.Length];
        for (int i = 0; i < text.Length; i++)
        {
            masked[i] = maskMapping.ContainsKey(text[i]) ? maskMapping[text[i]] : text[i];
        }
        return new string(masked);
    }

    public void ResetEffect()
    {
        StopAllCoroutines();
        SetVisibleCharacters(0);
    }
}
