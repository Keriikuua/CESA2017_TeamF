using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    public GameObject prefab;
    public Vector3 InitPos; // 初期座標
    public int m_nHeight;   // 縦
    public int m_nWidth;    // 横

    private List<List<GameObject>> FieldList = new List<List<GameObject>>();   // フィールドデータ格納の2次元リスト 

    // Use this for initialization
    void  Awake() {
        Create();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // フィールド生成
    void Create()
    {
        for (int nHeight = 0; nHeight < m_nHeight; nHeight++)   // 縦
        {
            List<GameObject> Data = new List<GameObject>();   // 横一列分のデータ
            Vector3 tile_pos;

            for (int nWidth = 0; nWidth < m_nWidth; nWidth++)   // 横
            {
                // フィールドのマス目をそれぞれ生成
                tile_pos = new Vector3( InitPos.x + prefab.transform.localScale.x * nHeight,
                                        InitPos.y,
                                        InitPos.z + prefab.transform.localScale.z * nWidth);


                GameObject FieldData = Instantiate( prefab , tile_pos , Quaternion.identity );
                FieldData.transform.parent = transform;
                Data.Add(FieldData);
            }

            FieldList.Add(Data);
        }
    }

    public Vector3 SetMyCharaPos(){
        GameObject obj = FieldList[m_nHeight - 1][m_nWidth / 2];
        
        return obj.transform.position;
    }//自キャラのスタート位置

    public Vector3 SetMyWaiteCharaPos()
    {
        GameObject obj = FieldList[m_nHeight - 2][m_nWidth / 2];

        return obj.transform.position;
    }

    public Vector3 SetMyRightCharaPos()
    {
        GameObject obj = FieldList[m_nHeight - 2][m_nWidth - 1];
        return obj.transform.position;
    }//右側レーンのスタート位置

    public Vector3 SetMyLeftCharaPos()
    {
        GameObject obj = FieldList[m_nHeight - 2][0];
        return obj.transform.position;
    }//左側レーンのスタート位置

    public Vector3 SetEnemyStartPos()
    {
        GameObject obj = FieldList[0][m_nWidth / 2];
        return obj.transform.position;
    }

    public Vector3 MapDataPullOut(int num,int num2)
    {
        return FieldList[num][num2].transform.position;
    }

}
