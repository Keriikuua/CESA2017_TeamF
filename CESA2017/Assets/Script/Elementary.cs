using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 素クラス
public class Elementary : MonoBehaviour {

    [SerializeField, Header("素が流れる速さ")]
    Vector3 Speed;

    private bool bStop;

    // Use this for initialization
    void Start () {
        bStop = false;
    }

    // 当たってる間呼ばれ続ける
    void OnCollisionStay(Collision hit)
    {
        if (hit.gameObject.tag == "Convair" && !bStop)    //　コンベアに乗ってて止まってない時
        {
            transform.position += Speed;
        }
    }

    // 当たった瞬間呼ばれる
    void OnCollisionEnter(Collision hit)
    {
        Mold mold;
        if (hit.gameObject.tag == "Mold")
        {
            mold = hit.gameObject.GetComponent<Mold>();
            mold.HitCollider(transform);
            Destroy(transform.gameObject);
        }
    }
}
