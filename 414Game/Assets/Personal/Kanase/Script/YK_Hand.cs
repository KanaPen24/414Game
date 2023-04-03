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
using Live2D.Cubism.Framework;

public class YK_Hand : MonoBehaviour
{
    [SerializeField] private YK_UICatcher UICatcher;
    [SerializeField] private YK_HPBarVisible HPBarVisible;
    [SerializeField] private YK_SkillIcon SkillIconVisible;
    [SerializeField] private IS_Player Player;
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
    void HandPull()
    {
        //for (int i = 0; i < 100; i++)
        //{
        //    renderController.Opacity -= 0.1f;
        //}
        renderController.Opacity = 0f;
        //レイヤーをUIの後ろにする
        renderController.SortingOrder = -1;
        //どのUIを選んでるかで引っ張ってくる座標を変える
        //switch (CursolEvent.GetUINumber())
        //{
        //    case 0:
        //        HPBarVisible.GetSetVisible = false;
        //        break;
        //    case 1:
        //        SkillIconVisible.VisibleSkill(0);
        //        break;
        //    case 2:
        //        SkillIconVisible.VisibleSkill(1);
        //        break;
        //    case 3:
        //        SkillIconVisible.VisibleSkill(2);
        //        break;
        //    case 4:
        //        SkillIconVisible.VisibleSkill(3);
        //        break;
        //    case 5:
        //        SkillIconVisible.VisibleSkill(4);
        //        break;
        //}
    }

    //アニメーションの終わり
    void AnimationEnd()
    {
        UICatcher.ParticleStop();
        Player.GetSetEquip = true;
    }

}
