using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            //Nothing here yet because I did not make the scenes yet because I will make them when I will start working on them
            //So I will soon be making the battle one
            //if (touch.position.x < aGameObject.position.x + sizeof / 2 &&
            //    touch.position.x > aGameObject.position.x - sizeof / 2 &&
            //    touch.position.y < aGameObject.position.y + sizeof / 2 &&
            //    touch.position.y > aGameObject.position.y - sizeof / 2 &&)
            //{
            //    //Go to the scene of the selected thing
            //}
        }
    }
}
