/**
 * @file   IS_UIManager.cs
 * @brief  UIの管理クラス
 * @author IharaShota
 * @date   2023/03/17
 * @Update 2023/03/17 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_UIManager : MonoBehaviour
{
    static public IS_UIManager instance;            // インスタンス
    [SerializeField] private List<YK_UI> m_UIs; // UIクラスの動的配列 

    private void Start()
    {
        // インスタンス化する
        //(他のスクリプトから呼び出すためだが、他のシーンには残さない)
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public YK_UI FindUI(YK_UI ui)
    {
        for (int i = 0, size = m_UIs.Count; i < size; ++i)
        {
            if (ui == m_UIs[i])
            {
                return m_UIs[i];
            }
        }

        return null;
    }
}
