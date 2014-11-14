using UnityEngine;
using System.Collections;

// Require these components when using this script
[RequireComponent(typeof (Animator))]
//[RequireComponent(typeof (CapsuleCollider))]
[RequireComponent(typeof (Rigidbody))]


public class BotControlScript : MonoBehaviour
{
	

	
	public float animSpeed = 1.5f;				// a public setting for overall animator animation speed

	public bool useCurves;						// a setting for teaching purposes to show use of curves

	private Animator anim;							// a reference to the animator on the character
	private AnimatorStateInfo currentBaseState;			// a reference to the current state of the animator, used for base layer

	private CapsuleCollider col;					// a reference to the capsule collider of the character


	static int idleState = Animator.StringToHash("Base Layer.Idle");	
	static int runForwardState = Animator.StringToHash("Base Layer.RunForward");// these integers are references to our animator's states


	void Start ()
	{

		// initialising reference variables
		anim = GetComponent<Animator>();					  
		col = GetComponent<CapsuleCollider>();				
		anim.SetLayerWeight (1,1);

		
	}

	void Update()
	{

	}
	
	
	public float movementSpeed;
	public float rotationSmooth;

	void FixedUpdate ()
	{
				
		
				Vector3 temp = transform.position;
				temp.x = 0.0f;
				transform.position = temp;
				float h = Input.GetAxis ("Horizontal");				// setup h variable as our horizontal input axis
				float v = Input.GetAxis ("Vertical");				// setup v variables as our vertical input axis
				// set our animator's float parameter 'Speed' equal to the vertical input axis				
				anim.SetFloat ("Direction", h); 						// set our animator's float parameter 'Direction' equal to the horizontal input axis		
				anim.speed = animSpeed;								// set the speed of our animator to the public variable 'animSpeed'
					// set the Look At Weight - amount to use look at IK vs using the head's animation
				currentBaseState = anim.GetCurrentAnimatorStateInfo (0);	// set our currentState variable to the current state of the Base Layer (0) of animation
		


				if (v > 0) {
						anim.SetFloat ("AimDir", 1);
				}
				if (v < 0) {
						anim.SetFloat ("AimDir", -1);
		
				}

				if (h > 0) 
				{
						anim.SetFloat ("Direction", 1);
						transform.Translate (Vector3.forward * movementSpeed * Time.deltaTime);
						transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (Vector3.forward), Time.deltaTime * rotationSmooth);
		
				} 

				if (h < 0) 
				{
			
						anim.SetFloat ("Direction", -1);
						transform.Translate (Vector3.forward * movementSpeed * Time.deltaTime);
						transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (Vector3.back), Time.deltaTime * rotationSmooth);
				}	
				
		}

}





	

