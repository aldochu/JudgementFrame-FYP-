using UnityEngine;
using System.Collections.Generic;

public class RadarDetection : MonoBehaviour {
    public List<GameObject> nearbyEnemies;
    public float detectionLength;
    void Awake()
    {
        nearbyEnemies = new List<GameObject>();
    }

	// Use this for initialization
	void Start () {
        detectionLength = GetComponent<CapsuleCollider>().radius;
	}
	
	// Update is called once per frame
	void Update () {
        if (nearbyEnemies.Count > 0)
        {
            //string output = "";
            //foreach (GameObject e in nearbyEnemies)
            //{
            //    output += e.GetComponent<Enemy>().name + "   ";
            //}
            //Debug.Log(output);
        }
        else
        {
            //Debug.Log("-");
        }

        
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Enemy>())
        {
            // if the entered object has the ENEMY component
            nearbyEnemies.Add(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Enemy>())
        {
            // if the entered object has the ENEMY component
            nearbyEnemies.Remove(other.gameObject);
        }
    }
}

// This class is to be attached to the object with the SPHERE COLLIDER
// that acts as the radar range.