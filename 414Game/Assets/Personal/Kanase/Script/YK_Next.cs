/**
 * @file YK_Next.cs
 * @brief NextUIの処理
 * @author 吉田叶聖
 * @date 2023/03/25
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class YK_Next : MonoBehaviour
{
    [SerializeField] private Image Next;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            NextEnableFalse();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            NextEnableTrue();
        }
    }

    //NextUIを表示
    public void NextEnableTrue()
    {
        // 1秒で後X,Y方向を0.5倍に変更
        Next.transform.DOScale(new Vector3(0.5f, 0.3f, 0f), 1f);
        // 1秒でテクスチャをフェードアウト
        Next.DOFade(0f, 1f);
    }
    //NextUIを非表示
    public void NextEnableFalse()
    {
        // 1秒で後X,Y方向を元の大きさに変更
        Next.transform.DOScale(new Vector3(1.5f, 0.7f, 0f), 1f);
        // 1秒でテクスチャをフェードイン
        Next.DOFade(1f, 0f);
    }
}
