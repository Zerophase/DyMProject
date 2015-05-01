using UnityEngine;
using System.Collections;

public class MoveShip : MonoBehaviour {


    private GameObject Trigger;

    void Start(){
        Trigger = GameObject.FindGameObjectWithTag("Trigger");
    }

    public void OnTriggerEnter(Collider Trigger)
    {
           transform.Translate(0, 50 * 2, 0);

    }

}
