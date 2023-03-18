/**
 * @file   YK_UICacher.cs
 * @brief  UIをとるための関数たち
 * @author 吉田叶聖
 * @date   2023/03/18
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YK_UICatcher : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;   //エフェクトオブジェクト

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // 1. 再生
    private void ParticlePlay()
    {
        particle.Play();
    }

    // 2. 一時停止
    private void ParticlePause()
    {
        particle.Pause();
    }

    // 3. 停止
    private void ParticleStop()
    {
        particle.Stop();
    }

    //アニメーション動かす
    public void AnimationPlay()
    {
        // アニメーターで動かす
        //GetComponent<Animator>().SetTrigger("Hand");
    }
}
