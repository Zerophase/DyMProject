#pragma strict

function OnTriggerEnter (collision : Collider)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.Find("MainCamera").GetComponent("SplineController").enabled=true;
        }
    }