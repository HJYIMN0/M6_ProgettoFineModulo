using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iFadeble
{
    void CallFadeOut();
    void CallFadeIn();
    IEnumerator FadeOut();
    IEnumerator FadeIn();
    void SetFadeSpeed(float speed);
}
