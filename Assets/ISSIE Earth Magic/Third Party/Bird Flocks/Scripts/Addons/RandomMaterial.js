#pragma strict
var targetRenderer:Renderer;
var materials:Material[];


function Start () {
	targetRenderer.sharedMaterial = materials[Random.Range(0, materials.length)];
}