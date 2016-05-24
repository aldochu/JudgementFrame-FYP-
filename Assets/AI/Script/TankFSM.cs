using UnityEngine;
using System.Collections;

public class TankFSM : FSM {

	
	public enum FSMState
	{
		//None, //default
		Chase, //chase or go towards player
		Attack, //attack
		Dead, // explosion
	}
	
	
	/// <summary>
	/// Weapon system
	/// </summary>
	public Transform BulletStartPosition;
	private Transform HitPosition;
	public GameObject Bullet;
	public float BulletInterval; //the interval between each missle attack
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
	
	public GameObject Turret; //this is to rotate the body so that it will look more realistic
	
	
	/// <summary>
	/// The range to attack is the minimum range it needs to be before engage to open fire
	/// </summary>
	public float TheRangeToAttack; //the range it require before it start attacking
	
	
	//Health of enemy
	public int Health = 100;
	
	
	
	//Initialize the Finite state machine for the NPC tank
	protected override void Initialize()
	{
		nav = GetComponent<NavMeshAgent> ();
		
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		HitPosition = GameObject.FindGameObjectWithTag("HitLocation").transform;

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
		//To make apache always facing player so that they can shoot missle at the correct direction.
		Vector3 lookDir = playerTransform.position - Turret.transform.position;
		Turret.transform.rotation = Quaternion.Slerp(Turret.transform.rotation,Quaternion.LookRotation(lookDir), Time.deltaTime);
		//Orbit around player
		
		

		/////////////////////////////////////////////////// End Of Movement///////////////////////////////////////////////////////////////
		
		
		/////////////////////////////////////////////////// Attack System///////////////////////////////////////////////////////////////
		


		if ((_BulletInterval -= Time.deltaTime) < 0) {


			Vector3 lookat = HitPosition.position - BulletStartPosition.position;
			BulletStartPosition.transform.rotation = Quaternion.LookRotation (lookat);

			GameObject NewBullet = (GameObject)Instantiate (Bullet, BulletStartPosition.position, BulletStartPosition.rotation);
			NewBullet.GetComponent<Rigidbody> ().velocity = BulletStartPosition.forward * 60;
			_BulletInterval = BulletInterval;
			DestroyObject (NewBullet, 5);
		}
		
		
		
		////////////////////////////////////////////////////////// Chase Again ////////////////////////////////////////////////////////////
		if (Vector3.Distance (transform.position, playerTransform.position) > TheRangeToAttack + 20)
		{
			nav.Resume ();
			curState = FSMState.Chase;

		}


	}
	
	
	
	
	/// Dead state
	protected virtual void UpdateDeadState()
	{
		nav.Stop ();
		nav.speed = 0.0f;
		
	}
	
	
}

