using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractFader : MonoBehaviour, iFadeble
{
    [Header("Fade Settings")]
    [SerializeField] protected float _fadeSpeed = 1f;

    protected virtual void Awake()
    {
        InitializeFadeComponent();
    }

    protected abstract void InitializeFadeComponent();

    public virtual void CallFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    public virtual void CallFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    public abstract IEnumerator FadeOut();
    public abstract IEnumerator FadeIn();

    public virtual void SetFadeSpeed(float speed)
    {
        _fadeSpeed = Mathf.Max(0.1f, speed);
    }

    public virtual void OnTimerFadeOutRequested() => CallFadeOut();
    public virtual void OnTimerFadeInRequested() => CallFadeIn();
}
