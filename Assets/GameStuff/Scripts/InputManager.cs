using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager n_InputManager;
    public GameObject theParticles;
    public GameObject thePoint;
    private bool letGo = true;
    //Use this input manager for anything that doesn't have input integrated into it yet
    private void Awake()
    {
        if (n_InputManager == null)
        {
            n_InputManager = this;
            return;
        }
        Destroy(this);
    }
    public List<Vector3> getInput()
    {
        List<Vector3> list = new List<Vector3>(); //Returns a number for position and one for input





        return list;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject effectBlow = Instantiate(theParticles, new Vector3(200, 300, 0), Quaternion.identity) as GameObject;
        //var main = effectBlow.gameObject.GetComponent<ParticleSystem>().main;
        Destroy(effectBlow, 3);
        //main.startColor = player.GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            
            Vector3 point = new Vector3();
            Touch touch = Input.GetTouch(0);
            if (letGo == true)
            {
                GameObject effectBlow = Instantiate(theParticles, Camera.main.ScreenToWorldPoint(touch.position), Quaternion.identity);
                effectBlow.transform.GetComponent<ParticleSystem>().Play();
                Destroy(effectBlow, 3);

            }
            letGo = false;
        }
        else
        {
            letGo = true;
        }
    }
}
