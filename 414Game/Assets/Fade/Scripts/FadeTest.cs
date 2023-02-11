using UnityEngine;
using System.Collections;

public class FadeTest : MonoBehaviour
{
    [SerializeField]
    Fade fade = null;

    public void OnClick()
    {
        fade.FadeIn(1, () =>
        {
            fade.FadeOut(1);
        });
    }
}