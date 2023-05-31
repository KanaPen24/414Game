using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ON_TextEntry : MonoBehaviour
{
    [SerializeField] private Material _material;
    private static readonly int PROPERTY_RATE = Shader.PropertyToID("_Rate");

    [SerializeField] float test = 0.0f;

    private void Update()
    {
        SetRate(test);
    }

    // 文字登場演出の遷移
    public void SetRate(float num)
    {
        num = num > 0.5f ? 0.5f : num;
        num = num < 0.0f ? 0.0f : num;
        _material.SetFloat(PROPERTY_RATE, num);
    }
}
