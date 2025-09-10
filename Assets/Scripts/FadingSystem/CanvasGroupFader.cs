using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGroupFader : AbstractFader
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float invisibleAlpha;
    [SerializeField] private float visibleAlpha;

    protected override void ApplyAlpha(float alpha) => canvasGroup.alpha = alpha;
    protected override float GetVisibleAlpha() => visibleAlpha;
    protected override float GetInvisibleAlpha() => invisibleAlpha;

    public override float GetCurrentAlpha()
    {
        if (canvasGroup == null)
        {
            Debug.LogWarning("CanvasGroup component is missing. Cannot get current alpha.");
            return 0f; // or some default value
        }
        else
            return canvasGroup.alpha;
    }
    public override void Awake()
    {        
        if (canvasGroup == null)
        {
            Debug.LogWarning($"CanvasGroup component is missing on {gameObject.name}. Searching...");
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                Debug.LogError($"CanvasGroup component not found on {gameObject.name}. Please add one.");
                return;
            }
            else if (canvasGroup != null && visibleAlpha <= 0.01f)
            {
                Debug.Log($"{gameObject.name} is assigning CanvasGroup Alpha to VisibleAlphaVar");
                Debug.Log($"CanvasGroup component found and assigned on {gameObject.name}.");
                visibleAlpha = canvasGroup.alpha;
            }
        }

        base.Awake();
    }

    protected override IEnumerator FadeIn(float timer)
    {
        yield return StartCoroutine(base.FadeIn(timer));
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    protected override IEnumerator FadeOut(float timer)
    {
        yield return StartCoroutine(base.FadeOut(timer));
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
