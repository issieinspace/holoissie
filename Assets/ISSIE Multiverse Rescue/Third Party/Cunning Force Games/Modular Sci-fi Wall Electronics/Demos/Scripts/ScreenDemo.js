#pragma strict

var screen1 : GameObject;
var screen2 : GameObject;
var screen3 : GameObject;

private var showMenu : boolean = false;
private var textureScale : float = 1.0;
private var scrollSpeed : float = -0.05;

private var screen1Flicker : Flicker;
private var screen2Flicker : Flicker;
private var screen3Flicker : Flicker;

private var screen1ScrollTexture : ScrollTexture;
private var screen2ScrollTexture : ScrollTexture;
private var screen3ScrollTexture : ScrollTexture;

private var screen1Scanlines : Material;
private var screen2Scanlines : Material;
private var screen3Scanlines : Material;

private var scanlinesAlpha : float = 0.5;

private var minOnDelay : float = 0.5;
private var maxOnDelay : float = 5.0;
private var minOffDelay : float = 0.01;
private var maxOffDelay : float = 0.02;
private var minFlickerAlpha : float = 0.6;
private var maxFlickerAlpha : float = 0.7;

function Start ()
{
	screen1ScrollTexture = screen1.transform.Find("Screen_Scanlines").GetComponent(ScrollTexture);
	screen2ScrollTexture = screen2.transform.Find("Screen_Scanlines").GetComponent(ScrollTexture);
	screen3ScrollTexture = screen3.transform.Find("Screen_Scanlines").GetComponent(ScrollTexture);
	
	screen1Flicker = screen1.GetComponent(Flicker);
	screen2Flicker = screen2.GetComponent(Flicker);
	screen3Flicker = screen3.GetComponent(Flicker);
	
	screen1Scanlines = screen1ScrollTexture.gameObject.GetComponent.<Renderer>().material;
	screen2Scanlines = screen2ScrollTexture.gameObject.GetComponent.<Renderer>().material;
	screen3Scanlines = screen3ScrollTexture.gameObject.GetComponent.<Renderer>().material;
}

function Update ()
{
	if(Input.GetKeyDown(KeyCode.Tab))
	{
		showMenu = !showMenu;
	}
}

function OnGUI()
{
	if(!showMenu)
	{
		GUI.Label(new Rect(10, 20, 250, 30), "Press Tab to Toggle Screen Menu");
	}
	else
	{
		GUI.Label(new Rect(10, 20, 200, 30), "Scan Lines");
		
		GUI.Label(new Rect(10, 50, 200, 30), "Scan Lines Tiling Scale: " + textureScale);
		textureScale = GUI.HorizontalSlider(new Rect(10, 70, 200, 30), textureScale, 0.1, 5.0);
		textureScale = Mathf.Round(textureScale * 100) / 100;
		screen1Scanlines.mainTextureScale = Vector2(textureScale, textureScale);
		screen2Scanlines.mainTextureScale = Vector2(textureScale, textureScale);
		screen3Scanlines.mainTextureScale = Vector2(textureScale, textureScale);

		GUI.Label(new Rect(10, 90, 200, 30), "Scroll Speed: " + scrollSpeed);
		scrollSpeed = GUI.HorizontalSlider(new Rect(10, 110, 200, 30), scrollSpeed, -0.5, 0.5);
		scrollSpeed = Mathf.Round(scrollSpeed * 100) / 100;
		screen1ScrollTexture.scrollYSpeed = scrollSpeed;
		screen2ScrollTexture.scrollYSpeed = scrollSpeed;
		screen3ScrollTexture.scrollYSpeed = scrollSpeed;
		
		GUI.Label(new Rect(10, 130, 200, 30), "Alpha: " + scanlinesAlpha);
		scanlinesAlpha = GUI.HorizontalSlider(new Rect(10, 150, 200, 30), scanlinesAlpha, 0.0, 1.0);
		scanlinesAlpha = Mathf.Round(scanlinesAlpha * 100) / 100;
		screen1Scanlines.color.a = scanlinesAlpha;
		screen2Scanlines.color.a = scanlinesAlpha;
		screen3Scanlines.color.a = scanlinesAlpha;

		GUI.Label(new Rect(10, 180, 200, 30), "Screen Flicker");
		
		GUI.Label(new Rect(10, 210, 200, 30), "Minimum On Delay: " + minOnDelay);
		minOnDelay = GUI.HorizontalSlider(new Rect(10, 230, 200, 30), minOnDelay, 0.01, 5.0);
		minOnDelay = Mathf.Round(minOnDelay * 100) / 100;
		screen1Flicker.minOnDelay = minOnDelay;
		screen2Flicker.minOnDelay = minOnDelay;
		screen3Flicker.minOnDelay = minOnDelay;
		
		GUI.Label(new Rect(10, 250, 200, 30), "Maximum On Delay: " + maxOnDelay);
		maxOnDelay = GUI.HorizontalSlider(new Rect(10, 270, 200, 30), maxOnDelay, 0.01, 5.0);
		maxOnDelay = Mathf.Round(maxOnDelay * 100) / 100;
		screen1Flicker.maxOnDelay = maxOnDelay;
		screen2Flicker.maxOnDelay = maxOnDelay;
		screen3Flicker.maxOnDelay = maxOnDelay;
		
		GUI.Label(new Rect(10, 290, 200, 30), "Minimum Off Delay: " + minOffDelay);
		minOffDelay = GUI.HorizontalSlider(new Rect(10, 310, 200, 30), minOffDelay, 0.01, 1.0);
		minOffDelay = Mathf.Round(minOffDelay * 100) / 100;
		screen1Flicker.minOffDelay = minOffDelay;
		screen2Flicker.minOffDelay = minOffDelay;
		screen3Flicker.minOffDelay = minOffDelay;
		
		GUI.Label(new Rect(10, 330, 200, 30), "Maximum Off Delay: " + maxOffDelay);
		maxOffDelay = GUI.HorizontalSlider(new Rect(10, 350, 200, 30), maxOffDelay, 0.01, 1.0);
		maxOffDelay = Mathf.Round(maxOffDelay * 100) / 100;
		screen1Flicker.maxOffDelay = maxOffDelay;
		screen2Flicker.maxOffDelay = maxOffDelay;
		screen3Flicker.maxOffDelay = maxOffDelay;
		
		GUI.Label(new Rect(10, 370, 200, 30), "Minimum Alpha: " + minFlickerAlpha);
		minFlickerAlpha = GUI.HorizontalSlider(new Rect(10, 390, 200, 30), minFlickerAlpha, 0.1, 1.0);
		minFlickerAlpha = Mathf.Round(minFlickerAlpha * 100) / 100;
		screen1Flicker.minAlpha = minFlickerAlpha;
		screen2Flicker.minAlpha = minFlickerAlpha;
		screen3Flicker.minAlpha = minFlickerAlpha;
		
		GUI.Label(new Rect(10, 410, 200, 30), "Maximum Alpha: " + maxFlickerAlpha);
		maxFlickerAlpha = GUI.HorizontalSlider(new Rect(10, 430, 200, 30), maxFlickerAlpha, 0.1, 1.0);
		maxFlickerAlpha = Mathf.Round(maxFlickerAlpha * 100) / 100;
		screen1Flicker.maxAlpha = maxFlickerAlpha;
		screen2Flicker.maxAlpha = maxFlickerAlpha;
		screen3Flicker.maxAlpha = maxFlickerAlpha;
	}
}