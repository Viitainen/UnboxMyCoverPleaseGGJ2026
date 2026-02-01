using System.Collections;
using UnityEngine;

public class MainMenuFlow : MonoBehaviour
{
    [SerializeField]
    private UIFader fader;

    [SerializeField]
    private float fadeOutDuration = 0.8f;

    [SerializeField]
    private float waitDuration = 2.0f;

    [SerializeField]
    private float fadeInDuration = 0.8f;

    private void Start()
    {
        StartCoroutine(Sequence());
    }

    private IEnumerator Sequence()
    {
        // Scene starts black → fade out to reveal menu
        fader.FadeIn(fadeOutDuration);
        yield return new WaitForSecondsRealtime(fadeOutDuration);

        // Wait on main menu
        yield return new WaitForSecondsRealtime(waitDuration);

        // Fade back to black
        fader.FadeOut(fadeInDuration);

        yield return new WaitForSecondsRealtime(fadeInDuration);

        CustomSceneManager.Instance.LoadGameScene();
    }
}
