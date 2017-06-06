using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class sceneload : MonoBehaviour
{

    public GameObject ImageObj;

    void Update()
    {
        /*
        if (ImageObj.GetComponent<fadeout>().alpha > 1)
        {
                SceneManager.LoadScene("game");
        }
         */


        if (ImageObj.GetComponent<fadeout>().alpha > 1)
        {
            if (SceneManager.GetActiveScene().name == "title")
            {
                SceneManager.LoadScene("Test_Takahashi");
            }

            //    if (SceneManager.GetActiveScene().name == "game")
            //    {
            //        SceneManager.LoadScene("result");
            //    } 

            //   if (SceneManager.GetActiveScene().name == "result")
            //    {
            //        SceneManager.LoadScene("title");
            //    }

        }



        if (Input.GetMouseButtonDown(0))
        {

            ImageObj.GetComponent<fadeout>().go = true;
            //ImageObj.GetComponent<fadeout>().go2 = true;

        }
    }





}