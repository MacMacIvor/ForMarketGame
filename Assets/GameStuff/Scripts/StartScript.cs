using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScript : MonoBehaviour
{
    private Touch theTouch;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            SceneManager.LoadScene("MainArea");
        }
        //if(Input.GetTouch(0).phase == TouchPhase.Began)
        //{
        //    
        //}
    }
}
