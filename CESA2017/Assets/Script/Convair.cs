using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Convair : MonoBehaviour {

    [SerializeField,Header("素のプレハブ")]
    GameObject prefab;

    [SerializeField, Header("素の生成位置")]
    Vector3 CreatePos;

    [SerializeField, Header("素が生成される速さ")]
    float fDefaultCreateSpeed;

    [SerializeField, Header("フィーバー中に素が生成される速さ")]
    float fFeverCreateSpeed;

    [SerializeField, Header("フィーバー")]
    Fever fever;

    // 生成した素の管理用リスト 
    private List<GameObject> ElementaryList = new List<GameObject>();
    private float CreateSpeed;

    // 
    IEnumerator MainStart()
    {
        while (true)
        {
            Create();
            yield return new WaitForSeconds(CreateSpeed);
        }
    }

    // Use this for initialization
    void Start()
    {
        CreateSpeed = fDefaultCreateSpeed;
        StartCoroutine("MainStart");
    }
	
	// Update is called once per frame
	void Update () {
        for(int nCnt = 0; nCnt < ElementaryList.Count;nCnt++)
        {
            if (ElementaryList[nCnt] == null)
            {
                ElementaryList.RemoveAt(nCnt);
                continue;
            }
        }

        // フィーバー中なら
        if (fever.GetFeverMode())
        {
            CreateSpeed = fFeverCreateSpeed;
        }
        else
        {
            CreateSpeed = fDefaultCreateSpeed;
        }
    }

    // 素生成
    void Create()
    {
        GameObject Element = Instantiate(prefab, CreatePos, Quaternion.identity);   // 素生成
        Element.transform.parent = transform;
        ElementaryList.Add(Element);    // リストに追加
    }
}
