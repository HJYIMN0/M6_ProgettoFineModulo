using System.Collections;
using UnityEngine;

public abstract class AbstractFader : MonoBehaviour
{
    [SerializeField] private float _fadeTimer = 1f;
    [SerializeField] protected bool _isVisibleOnStart = false;
    public bool _isVisible { get; private set; }
    public bool _isFading { get; private set; }

    public float FadeTimer => _fadeTimer;

    public virtual void Awake()
    {
        float alpha = GetCurrentAlpha();
        _fadeTimer = _fadeTimer <= 0.5f ? 1f : _fadeTimer;
        _isVisible = _isVisibleOnStart;
    }

    public void SetVisibilityState(bool value)
    {
        if (_isVisible != value)
            _isVisible = value;
    }

    public virtual void CallFadeOut(float timer)
    {
        if (_isFading) return;
        StartCoroutine(FadeOut(timer));
    }

    public virtual void CallFadeIn(float timer)
    {
        if (_isFading) return;
        StartCoroutine(FadeIn(timer));
    }

    // Coroutine generica
    protected virtual IEnumerator FadeOut(float timer)
    {
        Debug.Log($"FadeIn chiamato - _isVisible: {_isVisible}");

        if (!_isVisible) yield break;
        _isFading = true;
        float elapsed = 0f;

        Debug.Log($"Iniziando fade da {GetVisibleAlpha()} a {GetInvisibleAlpha()}");

        while (elapsed < timer)
        {
            float alpha = Mathf.Lerp(GetVisibleAlpha(), GetInvisibleAlpha(), elapsed / timer);
            ApplyAlpha(alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        ApplyAlpha(GetInvisibleAlpha());
        Debug.Log($"Fade completato - Alpha finale: {GetInvisibleAlpha()}");
        _isVisible = false;
        _isFading = false;
    }

    protected virtual IEnumerator FadeIn(float timer)
    {
        Debug.Log($"FadeIn chiamato - _isVisible: {_isVisible}");

        if (_isVisible) yield break;
        _isFading = true;
        float elapsed = 0f;

        Debug.Log($"Iniziando fade da {GetInvisibleAlpha()} a {GetVisibleAlpha()}");

        while (elapsed < timer)
        {
            float alpha = Mathf.Lerp(GetInvisibleAlpha(), GetVisibleAlpha(), elapsed / timer);
            ApplyAlpha(alpha);
            
            elapsed += Time.deltaTime;
            yield return null;
        }

        ApplyAlpha(GetVisibleAlpha());
        _isVisible = true;
        _isFading = false;
    }

    // La figlia definisce quali alpha usare
    protected abstract float GetVisibleAlpha();
    protected abstract float GetInvisibleAlpha();

    public abstract float GetCurrentAlpha();
    // La figlia applica l'alpha
    protected abstract void ApplyAlpha(float alpha);

}
