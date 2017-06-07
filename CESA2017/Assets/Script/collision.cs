using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class collision : MonoBehaviour
{

    //public GameObject ExploadObj; //爆発
   // public GameObject ExploadPos; //位置

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Instantiate(ExploadObj, ExploadPos.transform.position, Quaternion.identity);
        
        //相手のタグがEnemyならば、当たったEnemyを消す
        //Destroy(collision.gameObject); 

        //gameObject.SetActive(false);
        GameObject.Find("Canvas2").transform.Find("title logo2").gameObject.SetActive(true);
        //GameObject.Find("Main Camera").transform.Find("Camera").gameObject.SetActive(true);
       

    }
}