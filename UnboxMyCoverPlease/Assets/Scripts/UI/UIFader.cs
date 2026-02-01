using UnityEngine;
using DG.Tweening;

public class UIFader : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private float defaultFadeDuration = 0.4f;

    private Tween currentTween;

    public void FadeOut(float duration)
    {
        currentTween?.Kill();

        canvasGroup.blocksRaycasts = true;

        currentTween = canvasGroup
            .DOFade(1f, duration > 0f ? duration : defaultFadeDuration)
            .SetUpdate(true);
    }

    public void FadeIn(float duration)
    {
        currentTween?.Kill();

        currentTween = canvasGroup
            .DOFade(0f, duration > 0f ? duration : defaultFadeDuration)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                canvasGroup.blocksRaycasts = false;
            });
    }

    public void SetIn()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
    }

    public void SetOut()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }
}