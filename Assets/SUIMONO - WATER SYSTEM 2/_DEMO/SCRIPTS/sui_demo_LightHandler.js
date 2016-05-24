#pragma strict

var dayIntensity : float = 1.0;
var nightIntensity : float = 0.01;
var sunsetDegrees : float = 20.0;
var lightDegrees : float = 10.0;
var dayColor: Color = Color(1.0,0.968,0.933,1.0);
var sunsetColor: Color = Color(0.77,0.33,0.0,1.0);

private var lightObject : Light;
private var lightFac : float;
private var sunsetFac : float;

function Start () {
	lightObject = gameObject.GetComponent(Light) as Light;
}


function LateUpdate () {

	if (lightObject != null){

		//clamp values
		sunsetDegrees = Mathf.Clamp(sunsetDegrees,0.0,90.0);

		//find the light factor based on the rotation of the light
		lightFac = transform.eulerAngles.x;
		if (lightFac > 90.0) lightFac = 0.0;
		sunsetFac = Mathf.Clamp01(lightFac / sunsetDegrees);
		lightFac = Mathf.Clamp01(lightFac / lightDegrees);

		//set the light intensity
		lightObject.intensity = Mathf.Lerp(nightIntensity,dayIntensity,lightFac);

		//clamp the intensity just in case (having a 0.0 intensity can cause un-anticipated lighting problems in Unity)
		if (lightObject.intensity < 0.01) lightObject.intensity = 0.01;

		//modulate the light color
		lightObject.color = Color.Lerp(sunsetColor,dayColor,sunsetFac);

	}
}