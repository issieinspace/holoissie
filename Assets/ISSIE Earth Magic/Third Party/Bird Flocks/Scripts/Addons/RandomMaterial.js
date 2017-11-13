#pragma strict
var targetRenderer:Renderer;
var materials:Material[];

function Start () {
	ChangeMaterial();
}

function ChangeMaterial(){
	targetRenderer.sharedMaterial = materials[Random.Range(0, materials.length)];
}