using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Convair : PausableCoroutine {

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

    public PausableCoroutine pause;

    bool bSwitch;
    IEnumerator MethodName;


    // 
    IEnumerator MainStart()
    {
        while (true)
        {
            yield return new WaitForSeconds(CreateSpeed);
            Create();
        }
    }
    

    // Use this for initialization
    void Start()
    {
        CreateSpeed = fDefaultCreateSpeed;
        Create();
        bSwitch = true;
        MethodName = MainStart();
        StartCoroutine(MethodName);
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
        if (fever != null && fever.GetFeverMode())
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

    /// <summary>
    /// 素生成のON/OFF切り替え
    /// </summary>
    public void ConvairSwitch()
    {
        if (!bSwitch)
        {
            MethodName = null;
            MethodName = MainStart();
            StartCoroutine(MethodName);
        }
        else
        {
            StopCoroutine(MethodName);
        }
        bSwitch = !bSwitch;
    }
}
