using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CompassBehaviour : MonoBehaviour {
    public GameObject mechRef;
    public float smooth;
    public float currentRotation;
    
    private RectTransform imageRTransform;
    private GameObject northObject = null;

    void Awake()
    {
        imageRTransform = GetComponent<RectTransform>();
        northObject = GameObject.FindGameObjectWithTag("NorthPoint");
    }
    
    // Use this for initialization
	void Start () {
        if (!northObject)
        {
            Debug.Log("Compass: Can't find North object");
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (northObject) // If scene has a North object
        {
            Vector3 targetDir =
                new Vector3(northObject.transform.position.x, 0, northObject.transform.position.z) - new Vector3(mechRef.transform.position.x, 0, mechRef.transform.position.z);
            Vector3 forward = mechRef.transform.forward;
            // Get angle between target direction and forward in degrees (0 - 180)
            float angle = Vector3.Angle(targetDir, forward);
            // Decide the sign with this function thanks to this awesome guy
            // on stack overflow: http://answers.unity3d.com/answers/262476/view.html
            // "~the shoulders of giants"
            float sign = Mathf.Sign(Vector3.Dot(targetDir, mechRef.transform.right));
            angle *= -sign;
            currentRotation = angle;
            imageRTransform.localRotation = Quaternion.Euler(0, 0, angle);
        }
	}
}

//Create a "Enemy object". Have it show up on the radar

//Create the "Weapon Management"