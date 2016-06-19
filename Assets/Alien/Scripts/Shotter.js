#pragma strict
var fireprefab : Rigidbody;
var shotcontrol : Transform;
var alien : GameObject;
var shotsound : AudioClip; 
function Start () {

}

function Update () {
if(Input.GetButtonDown("Fire1")){
alien.GetComponent.<Animation>().Play("alien_attack");
MakeShot();
}
}



function MakeShot(){
yield WaitForSeconds(0.5);
GetComponent.<AudioSource>().PlayOneShot(shotsound);
var fireInstance : Rigidbody;
fireInstance= Instantiate(fireprefab, shotcontrol.position, shotcontrol.rotation);
fireInstance.AddForce(shotcontrol.forward * 5000);
}

