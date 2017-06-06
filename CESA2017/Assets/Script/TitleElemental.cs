using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleElemental : MonoBehaviour {

    [SerializeField, Header("素が流れる速さ")]
    Vector3 Speed;

    bool bDirect;
    Vector3 InitPos;

    // Use this for initialization
    void Start () {
        bDirect = false;
        InitPos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {

    }

    // 当たってる間呼ばれ続ける
    void OnCollisionStay(Collision hit)
    {
        if (hit.gameObject.tag == "Convair")    //　コンベアに乗ってて止まってない時
        {
            if (!bDirect)
            {
                transform.position += Speed;

                if(transform.position.x < InitPos.x - 0.05f)
                    bDirect = true;
            }
            else 
            {
                transform.position -= Speed;

                if (transform.position.x > InitPos.x + 0.05f)
                    bDirect = false;
            }
        }
    }

    // 当たった瞬間呼ばれる
    void OnCollisionEnter(Collision hit)
    {
        Mold mold;
        if (hit.gameObject.tag == "Mold")
        {
            mold = hit.gameObject.GetComponent<Mold>();
            //mold.HitCollider(transform);
            Destroy(transform.gameObject);
        }
    }
}
