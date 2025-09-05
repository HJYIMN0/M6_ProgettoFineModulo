using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CanvasGroupFader : AbstractFader
{
    private CanvasGroup _canvasGroup;

    protected override void InitializeFadeComponent()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        if (_canvasGroup == null)
        {
            Debug.LogWarning("[CanvasGroupFader] CanvasGroup non trovato. Aggiungendolo automaticamente.");
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public override IEnumerator FadeOut()
    {
        if (_canvasGroup == null) yield break;

        while (_canvasGroup.alpha > 0f)
        {
            _canvasGroup.alpha -= Time.deltaTime * _fadeSpeed;
            yield return null;
        }

        _canvasGroup.alpha = 0f;
    }

    public override IEnumerator FadeIn()
    {
        if (_canvasGroup == null) yield break;

        while (_canvasGroup.alpha < 1f)
        {
            _canvasGroup.alpha += Time.deltaTime * _fadeSpeed;
            yield return null;
        }

        _canvasGroup.alpha = 1f;
    }
}