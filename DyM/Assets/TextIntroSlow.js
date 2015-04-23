#pragma strict

function FixedUpdate () {
	transform.localPosition.z += 1 * Time.deltaTime;
	transform.localPosition.y += 10 * Time.deltaTime;
}