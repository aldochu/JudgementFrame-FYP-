
using UnityEngine;
using System.Collections;

public class ApacheFSM : FSM {
	
		public enum FSMState
		{
			//None, //default
			Chase, //chase or go towards player
			HitChase, //chase or go towards player
			Attack, //attack
			Dead, // explosion
		}
		

		/// <summary>
		/// Weapon system
		/// </summary>
		public Transform missleStartPosition;
		public Transform BulletStartPosition;
		public GameObject Missle;
		public GameObject Bullet;
		public float MissleInterval; //the interval between each missle attack
		public float BulletInterval; //the interval between each missle attack
		private float _MissleInterval;
		private float _BulletInterval;
		//Range limit before engaging player
		public float range;
		
		
		//Current state that the NPC is reaching
		public FSMState curState;
		
		private bool reach = false;
	
		
		//Delay variable
		protected int delay;
		public float minDistToAvoid = 5.0f;
		
	/// <summary>
	/// This is for the movement of the apache, float i,b,f is used to do the swaying motion. FirstMovement is use to help check whether it's the 1st movement or not,
	/// because 1st movement rotation starts from default which is 0 to rotate 40, subsequently rotation would be from -40 to 40 and 40 to -40, hence this step is
	/// neceesary
	/// </summary>

		public GameObject Apachebody; //this is to rotate the body when it's swaying so that it will look more realistic
		private float i = 0;
		private float b = 0;
		private float f = 0;
		private bool FirstMovement = false;


	/// <summary>
	/// The range to attack is the minimum range it needs to be before engage to open fire
	/// </summary>
		public float TheRangeToShootRocket,TheRangeToAttack; //the range it require before it start attacking
		

		//Health of enemy
		public int Health = 100;

		
		
		//Initialize the Finite state machine for the NPC tank
		protected override void Initialize()
		{
			nav = GetComponent<NavMeshAgent> ();

			playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
			
			_MissleInterval = MissleInterval;
			_BulletInterval = BulletInterval;
			curState = FSMState.Chase;
		}
		


		//Update each frame
		protected override void FSMUpdate()
		{
			switch (curState) {
			case FSMState.Chase: UpdateChaseState(); break;
			case FSMState.Attack: UpdateAttackState(); break;
			case FSMState.Dead: UpdateDeadState(); break;
			case FSMState.HitChase: UpdateHitChaseState(); break;
			}
			
		}
		
		
		
		/// Chase state
		protected virtual void UpdateChaseState()
		{
			//Do what chasing does
			//nav.Resume ();
			//nav.speed = 4.5f; //nav mesh speed increase when chasing player

			nav.SetDestination (playerTransform.position); //make the enemy chase after the player
			//Move towards player
			//Quaternion targetRotation = Quaternion.LookRotation(
			//	playerTransform.position - transform.position);
			//Method 1: Vector Transformation
			//transform.rotation = Quaternion.Slerp(transform.rotation,
			    //                                  targetRotation, Time.deltaTime * 4 );
			//Method 2: Turning Animation
			//Vector3 rot = targetRotation.eulerAngles;
			//animator.SetFloat("Turn",rot.x/180.0);



			if (Vector3.Distance (transform.position, playerTransform.position) < TheRangeToShootRocket)
			{
				curState = FSMState.HitChase;
			}
			

		}


		protected virtual void UpdateHitChaseState()
		{

			nav.SetDestination (playerTransform.position); //make the enemy chase after the player

		//To make apache always facing player so that they can shoot missle at the correct direction.
			Vector3 lookDir = playerTransform.position - Apachebody.transform.position;
			lookDir.y = 0;
			Apachebody.transform.rotation = Quaternion.Slerp(Apachebody.transform.rotation,Quaternion.LookRotation(lookDir), Time.deltaTime * 4 );
			
		if ((_MissleInterval -= Time.deltaTime) < 0) {
			GameObject NewMissle = (GameObject)Instantiate (Missle, missleStartPosition.position, missleStartPosition.rotation);
			NewMissle.GetComponent<Rigidbody> ().velocity = missleStartPosition.forward * 100;
			_MissleInterval = MissleInterval;
			DestroyObject (NewMissle, 4);
		}

			if (Vector3.Distance (transform.position, playerTransform.position) < TheRangeToAttack)
			{
				curState = FSMState.Attack;
			}

		}
		
		/// Attack state
		protected virtual void UpdateAttackState()
		{
			nav.Stop ();

		///////////////////////////////////////////////////Movement///////////////////////////////////////////////////////////////
		transform.RotateAround (playerTransform.position, Vector3.down, (Mathf.Sin(Time.time))/3);
        //Orbit around player


        //To make apache always facing player so that they can shoot missle at the correct direction.
        Vector3 lookDir = playerTransform.position - transform.position;
        lookDir.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 4);



        Vector3 temp = transform.rotation.eulerAngles;
		//to edit transform.rotation use eularAngles


		if ((Mathf.Sin(Time.time))/3 < -0.01f && FirstMovement == false) 
		{
			temp.z = Mathf.Lerp (0f, 40f, f+= Time.deltaTime);

			Apachebody.transform.rotation = Quaternion.Euler (temp);

			if((Mathf.Sin(Time.time))/3 > -0.01f)
			{
				i = b = 0;
				FirstMovement = true;
			}



		}

		else if((Mathf.Sin(Time.time))/3 < -0.01f && FirstMovement == true)
		{
			temp.z = Mathf.Lerp (-40f, 40f, i+= Time.deltaTime/2);

			Apachebody.transform.rotation = Quaternion.Euler (temp);

			b = 0;
		}

		else
		{
			i = 0;

			temp.z = Mathf.Lerp (40f, -40f, b+= Time.deltaTime/2);

			Apachebody.transform.rotation = Quaternion.Euler (temp);

			FirstMovement = true;
		}
		/////////////////////////////////////////////////// End Of Movement///////////////////////////////////////////////////////////////


		/////////////////////////////////////////////////// Attack System///////////////////////////////////////////////////////////////

	
		if ((_BulletInterval -= Time.deltaTime) < 0) {
			GameObject NewBullet = (GameObject)Instantiate (Bullet, BulletStartPosition.position, BulletStartPosition.rotation);
			NewBullet.GetComponent<Rigidbody> ().velocity = BulletStartPosition.forward * 600;
			_BulletInterval = BulletInterval;
			DestroyObject (NewBullet, 2);
		}



		////////////////////////////////////////////////////////// Chase Again ////////////////////////////////////////////////////////////
			if (Vector3.Distance (transform.position, playerTransform.position) > TheRangeToAttack + 20)
			{
				nav.Resume ();
				curState = FSMState.HitChase;
				FirstMovement = false;
			}

		}




		/// Dead state
		protected virtual void UpdateDeadState()
		{
			nav.Stop ();
			nav.speed = 0.0f;
			
		}
		

	}
	
	
	
	
	
	
