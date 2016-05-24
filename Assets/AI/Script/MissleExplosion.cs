using UnityEngine;
using System.Collections;

public class MissleExplosion : MonoBehaviour {


	public GameObject Explosion;

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag != "Apache")
			MissleExplode (other.gameObject);
	}



	void MissleExplode(GameObject _Position)
	{
		
		GameObject newExplosion = (GameObject)Instantiate (Explosion, _Position.transform.position, _Position.transform.rotation);
		DestroyObject (newExplosion, 3);
	}

	//Asset Used
	//https://www.assetstore.unity3d.com/en/#!/content/21587
}
