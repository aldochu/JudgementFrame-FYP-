using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RadarBehaviour : MonoBehaviour {
    public CompassBehaviour compass;
    public GameObject enemyIconPrefab;
    public RadarDetection radarDetection;
    public float displayMaxRadius; // Change base on the size of the radar display UI

    private List<GameObject> enemiesOnDisplay;

    void Awake()
    {
        enemiesOnDisplay = new List<GameObject>();
        if (!radarDetection)
        {
            Debug.Log("Radar: Can't find radar detection");
        }
    }

    // Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (radarDetection)
        {
            // Clear previous frame's display
            foreach (GameObject gOB in enemiesOnDisplay)
            {
                Destroy(gOB);
            }
            enemiesOnDisplay.Clear();
            foreach (GameObject enemies in radarDetection.nearbyEnemies)
            {
                // Spawn UI elements in this manner: http://docs.unity3d.com/Manual/HOWTO-UICreateFromScripting.html
                GameObject newGuy = GameObject.Instantiate(enemyIconPrefab);
                newGuy.transform.SetParent(gameObject.transform, false);
                
                Vector3 dir =
                    new Vector3(enemies.transform.position.x, 0, enemies.transform.position.z) - new Vector3(radarDetection.transform.position.x, 0, radarDetection.transform.position.z);
                dir.Normalize();
                

                dir *= //displayMaxRadius;
                    (Vector3.Distance(new Vector3(enemies.transform.position.x, 0, enemies.transform.position.z), new Vector3(radarDetection.transform.position.x, 0, radarDetection.transform.position.z))/radarDetection.detectionLength) * displayMaxRadius;
                Debug.Log(dir);
                Vector3 displayPosition = new Vector3(dir.x, dir.z, 0);
                newGuy.GetComponent<RectTransform>().localPosition = displayPosition;
                enemiesOnDisplay.Add(newGuy);
            }

            GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, compass.currentRotation);
        }
	}
}
