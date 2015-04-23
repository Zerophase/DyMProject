#pragma strict

function FixedUpdate () {
	transform.localPosition.y += 10 * Time.deltaTime;
}