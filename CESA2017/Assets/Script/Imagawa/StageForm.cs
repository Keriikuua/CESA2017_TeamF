using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class StageForm : MonoBehaviour {

    string FileName;            //読み込むファイルの名前
    string StageNum;            //ステージの番号
    TextAsset csvFile;          //csvファイル
    private List<string[]> csvData = new List<string[]>();
    int height = 0;

    private void Awake(){

        FileName = "EnemyForm0";
        StageNum = "1";
        csvFile = Resources.Load("CSV/" + FileName) as TextAsset;
        StringReader reader = new StringReader(csvFile.text);

        while(reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            csvData.Add(line.Split(','));
            height++;
        }
    }

    public List<string[]> StageData_Enemy(){
        return csvData;
    }

}
