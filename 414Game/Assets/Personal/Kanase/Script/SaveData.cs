using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
//保存するデータクラス
public class SaveData
{
    public Vector3 pos; //座標
    public List<int> WeaponHp;    //武器の耐久値
    public bool RetryFlg;
}
