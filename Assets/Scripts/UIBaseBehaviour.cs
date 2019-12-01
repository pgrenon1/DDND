using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIBaseBehaviour : BaseBehaviour
{
    public float visibilityTransitionDuration;

    private bool _isVisible;
    public bool IsVisible
    {
        get
        {
            return _isVisible;
        }
        private set
        {
            _isVisible = value;

            ApplyVisibility();
        }
    }

    private void ApplyVisibility()
    {
        float canvasAlphaValue = IsVisible ? 1f : 0f;
        var tween = DOTween.To(() => CanvasGroup.alpha, x => CanvasGroup.alpha = x, canvasAlphaValue, visibilityTransitionDuration);
        tween.OnStart(() => CanvasGroup.interactable = false);
        tween.OnComplete(() => CanvasGroup.interactable = IsVisible);

        //CanvasGroup.alpha = IsVisible ? 1f : 0f;
        //CanvasGroup.interactable = IsVisible;
    }

    public void Show()
    {
        IsVisible = true;
    }

    public void Hide()
    {
        IsVisible = false;
    }
}
