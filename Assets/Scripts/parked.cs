using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parked : MonoBehaviour
{

    public float necessaryTime = 2f;
    float elapsed;
    bool isParked;
    GameObject pnt;
    // Start is called before the first frame update
    void Start()
    {
        isParked = false;
       pnt = gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay(Collider other)
    {
        if (!isParked)
        {
            if (other.gameObject.CompareTag("Player"))
            {

                elapsed += Time.fixedDeltaTime;
                if (elapsed > necessaryTime)
                {
                    // PathCreator.instance.CarParked(other.gameObject.name);
                    // isParked = true;
                    pnt.GetComponent<BoxCollider>().enabled = false;
                    PathCreator.instance.Score();
                  //  Debug.Log("LEVEL COMPleted");
                    isParked = true;
                }
            }
        }
    }

}
