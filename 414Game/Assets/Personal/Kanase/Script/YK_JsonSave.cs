using System.Collections;
using System.Collections.Generic;
using System.IO;  //StreamWriterなどを使うために追加
using UnityEngine;

public class YK_JsonSave : MonoBehaviour
{
    [HideInInspector] public SaveData data;     // json変換するデータのクラス
    string filepath;                            // jsonファイルのパス
    string fileName = "SaveData.json";          // jsonファイル名
    public List<IS_Weapon> WeaponHp;            // 保存するための武器の耐久値
    private int m_nDataLimit;  // データの限界地
    //-------------------------------------------------------------------
    // 開始時にファイルチェック、読み込み
    void Awake()
    {
        // パス名取得
        filepath = Application.dataPath + "/" + fileName;

        m_nDataLimit = WeaponHp.Capacity - 1;//配列のため-１をして調整

        // ファイルがないとき、ファイル作成
        if (!File.Exists(filepath))
        {
            Save(data);
        }

       //ファイルを読み込んでdataに格納
       data = Load(filepath);
    }

    //-------------------------------------------------------------------
    // jsonとしてデータを保存
    public void Save(SaveData data)
    {
        for (int i = 0; i <= m_nDataLimit; i++)
        {
            //data.WeaponHp.Add(WeaponHp[i].GetSetHp);
            data.WeaponHp[i] = WeaponHp[i].GetSetHp;
        }
        data.pos = this.gameObject.transform.position;          // dataの座標保存
        string json = JsonUtility.ToJson(data);                 // jsonとして変換
        StreamWriter wr = new StreamWriter(filepath, false);    // ファイル書き込み指定
        wr.WriteLine(json);                                     // json変換した情報を書き込み
        wr.Close();                                             // ファイル閉じる
    }

    // jsonファイル読み込み
    SaveData Load(string path)
    {
        StreamReader rd = new StreamReader(path);               // ファイル読み込み指定
        string json = rd.ReadToEnd();                           // ファイル内容全て読み込む
        rd.Close();                                             // ファイル閉じる
        Debug.Log(json);
        return JsonUtility.FromJson<SaveData>(json);            // jsonファイルを型に戻して返す
    }

    //プレイヤーとぶつかったら
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Save(data);
    }

}
