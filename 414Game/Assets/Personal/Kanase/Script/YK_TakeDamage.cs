using UnityEngine;
using System.Collections;

public class YK_TakeDamage : MonoBehaviour
{

    //　DamageUIプレハブ
    [SerializeField] private GameObject damageUI;
    private GameObject Canvas;
    private void Start()
    {
        Canvas= GameObject.Find("YK_Canvas");
    }

    public void Damage(Collider col, int damage)
    {
        damageUI.GetComponent<YK_DamageUI>().SetDamage(damage);
        //　DamageUIをインスタンス化。登場位置は接触したコライダの中心からカメラの方向に少し寄せた位置
        var parent = Canvas.transform;
        var obj = Instantiate<GameObject>(damageUI, col.bounds.center - Camera.main.transform.forward * 0.2f, Quaternion.identity, parent);
    }
}