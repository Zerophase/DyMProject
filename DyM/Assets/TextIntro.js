#pragma strict



function FixedUpdate () {
	transform.localPosition.z += 1 * Time.deltaTime;
	transform.localPosition.y += 0.5 * Time.deltaTime;
}