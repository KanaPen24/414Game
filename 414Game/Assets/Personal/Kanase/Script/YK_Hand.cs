/**
 * @file   YK_Hand.cs
 * @brief  手のアニメーションのAddEventで使うための関数
 * @author 吉田叶聖
 * @date   2023/03/18
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Rendering;

public class YK_Hand : MonoBehaviour
{
    [SerializeField] private YK_UICatcher UICatcher;
    [SerializeField] private CubismRenderController renderController;
    [SerializeField] private YK_CursolEvent CursolEvent;          //カーソルイベント
    private void Start()
    {
        //レイヤーをUIの後ろにする
        renderController.SortingOrder = -1;
        //透明にする
        renderController.Opacity = 0.0f;
    }

    //アニメーションの開始
    void AnimationStart()
    {
        renderController.Opacity = 1.0f;
    }

    //掴む瞬間
    void HandCatch()
    {
        //掴む瞬間にUIの前に持ってくる
        renderController.SortingOrder = 10;
    }

    //引っ込む時
    public void HandPull()
    {
        renderController.Opacity = 0f;
        //レイヤーをUIの後ろにする
        renderController.SortingOrder = -1;

        // 本来はここでUIを消したい(by:kanase)
    }

    //アニメーションの終わり
    void AnimationEnd()
    {
        UICatcher.ParticleStop();
    }

}
