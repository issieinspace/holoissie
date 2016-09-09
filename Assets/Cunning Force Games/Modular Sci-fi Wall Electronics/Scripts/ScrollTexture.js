#pragma strict
/* This script controls the scrolling scanlines on the holographic screens */

var scrollXSpeed : float = 0.0;		// Scrolling speed on X axis
var scrollYSpeed : float = 0.0;		// Scrolling speed on Y axis

private var offsetX : float;
private var offsetY : float;

function Update()
{
    offsetX = Time.time * scrollXSpeed;
    offsetY = Time.time * scrollYSpeed;
    
    GetComponent.<Renderer>().material.SetTextureOffset ("_MainTex", Vector2(offsetX, offsetY));		// Offset main texture
}