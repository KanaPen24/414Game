/**
 * @file YK_Controller.cs
 * @brief コントローラーの処理
 * 
 * コントローラーの入力とバイブレーション制御を行うスクリプト
 * ゲームパッドの接続状態を確認し、バイブレーションの開始と停止
 * 
 * @author 吉田叶聖
 * @date 2023/06/03
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class YK_Controller : MonoBehaviour
{
    public static YK_Controller instance;
    private Gamepad gamepad = Gamepad.current;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /**
     * @brief コントローラーのバイブレーションを開始する
     * 
     * 指定された時間だけコントローラーをバイブレーション
     * ゲームパッドが接続されていない場合はログを出力して処理を終了
     * バイブレーションの強さは左右で同じ値を設定
     * 指定時間後にStopVibration()関数を呼び出してバイブレーションを停止
     * 
     * @param time バイブレーションの継続時間（秒）
     */
    public void ControllerVibration(float time)
    {
        // ゲームパッドが接続されているかどうかを確認
        if (gamepad == null)
        {
            Debug.Log("ゲームパッド未接続");
            return;
        }

        // バイブレーションの強さを設定（左右で同じ値）
        gamepad.SetMotorSpeeds(1.0f, 1.0f);

        // 指定時間後にバイブレーションを停止する
        Invoke(nameof(StopVibration), time);
    }

    /**
     * @brief コントローラーのバイブレーションを停止
     * 両方のモーターの速度を0に設定してバイブレーションを停止
     */
    public void StopVibration()
    {
        gamepad.SetMotorSpeeds(0.0f, 0.0f);
    }
}
