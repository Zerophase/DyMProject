using UnityEngine;
using System.Collections;

public class MoveShip : MonoBehaviour {


    public void Update()
    {
		transform.Translate(0, 25 * Time.deltaTime, 0);
		
		if(transform.position.y >= 50f)
			AutoFade.LoadLevel(4, 2, 1, Color.black);
    }

}
