﻿/**
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
    [SerializeField] private IS_Player Player;
    private bool m_bOpacity = false;
    private Vector2 Scale;

    //定数定義
    const float MAX_SCALE = 120.0f;
    const float MIN_SCALE = 70.0f;

    private void Start()
    {
        //レイヤーをUIの後ろにする
        renderController.SortingOrder = -1;
        //透明にする
        renderController.Opacity = 0.0f;
        //現在のサイズ取得
        Scale = this.transform.localScale;
        this.transform.localScale= new Vector3(MIN_SCALE, MIN_SCALE);
    }

    private void Update()
    {
        //α値を増やす場合のフラグが真なら
        if (m_bOpacity)
        {
            if (renderController.Opacity <= 1.0f)
            {
                renderController.Opacity += 0.04f;
                if (this.transform.localScale.x <= MAX_SCALE)
                this.transform.localScale += new Vector3(1.0f, 1.0f);
            }
        }
        //α値を増やす場合のフラグが偽なら
        if (!m_bOpacity)
        {
            if (renderController.Opacity > 0.0f)
            {
                renderController.Opacity -= 0.02f;
                if (this.transform.localScale.x > MIN_SCALE)
                    this.transform.localScale -= new Vector3(2.0f, 2.0f);
            }
        }
    }

    //アニメーションの開始
    void AnimationStart()
    {
        m_bOpacity = true;
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
        m_bOpacity = false;
        //レイヤーをUIの後ろにする
        renderController.SortingOrder = -1;

        // 武器を装備する(武器を表示する)
        Player.GetWeapons((int)Player.GetSetEquipWeaponState).GetSetVisible = true;

        // 本来はここでUIを消したい(by:kanase)
    }

    //アニメーションの終わり
    void AnimationEnd()
    {
        UICatcher.ParticleStop();
    }

}
