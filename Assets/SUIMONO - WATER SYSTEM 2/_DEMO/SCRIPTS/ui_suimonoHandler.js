#pragma strict

var uiScale : float = 1.0;

private var loadFlag : boolean = false;
private var lightObject : Transform;
private var suimonoObject : SuimonoObject;
private var uiCanvasScale : UI.CanvasScaler;

private var sliderTOD : UI.Slider;
private var sliderBeaufort : UI.Slider;

private var currentTODVal : float = -1.0;


function Start () {

	//get main object
	lightObject = GameObject.Find("Directional Light").GetComponent(Transform);
	suimonoObject = GameObject.Find("SUIMONO_Surface").GetComponent(SuimonoObject) as SuimonoObject;
	uiCanvasScale = this.transform.GetComponent(UI.CanvasScaler) as UI.CanvasScaler;

	//find UI objects
	sliderTOD = this.gameObject.Find("Slider_TOD").GetComponent(UI.Slider) as UI.Slider;
	sliderBeaufort = this.gameObject.Find("Slider_Beaufort").GetComponent(UI.Slider) as UI.Slider;


}






function LateUpdate(){

	//CANVAS SCALE
	if (uiCanvasScale != null) uiCanvasScale.scaleFactor = uiScale;


	//########################
	// SET TIME OF DAY
	//########################
	if (lightObject != null && sliderTOD != null) lightObject.localEulerAngles.x = Mathf.Lerp(-15.0,90.0,sliderTOD.value);


	//###########################
	// SET SUIMONO SETTINGS
	//###########################
	if (suimonoObject != null) suimonoObject.beaufortScale = sliderBeaufort.value;





}