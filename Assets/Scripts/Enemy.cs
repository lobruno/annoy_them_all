using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    
	public NavMeshAgent agent;
	public Animator anim;
	
	public float spawnHeight;
	public float spawnMoveSpeed;
	public float jumpAttackDistance;
	
	public float walkspeed;
	public float runspeed;
	
	public Color normalColor;
	public Color angryColor;
	
	Transform player;
	public MoveArea area;
	public GameObject coin;
	GameManager manager;
	
	bool spawned;
	bool angry;
	public bool ang;
	
	Vector3 randomTarget;
	NavMeshPath path;
	Renderer rend;

	
	void Start(){
		PlayerController controller = GameObject.FindObjectOfType<PlayerController>();
		
		if(controller != null)
			player = controller.gameObject.transform;
		
		manager = GameObject.FindObjectOfType<GameManager>();
		
		rend = GetComponentInChildren<Renderer>();
		rend.material.color = normalColor;
		
		path = new NavMeshPath();
	
		agent.speed = walkspeed;
		agent.enabled = false;
		
	}
	
	void Update(){

		if (!manager.gameStarted)
		{ return; }
		

		if (!spawned){
			transform.Translate(Vector3.up * Time.deltaTime * -spawnMoveSpeed);
			
			if(transform.position.y <= 0){
				transform.position = new Vector3(transform.position.x, 0, transform.position.z);
				spawned = true;
				agent.enabled = true;
				
				anim.SetInteger("State", 1);
				randomTarget = area.RandomPosition();
			}
			
			return;
		}
		
		if(angry && player != null){
			agent.CalculatePath(player.position, path);
			
			if(path.status != NavMeshPathStatus.PathComplete){	
				
				if(anim.GetInteger("State") != 1)
					ContinueWalking();
				
				RandomWalk();
				
				return;
			}
			
			rend.material.color = angryColor;
			agent.destination = player.position;
			
			if(anim.GetInteger("State") != 2){
				anim.SetInteger("State", 2);
				agent.speed = runspeed;
				agent.stoppingDistance = jumpAttackDistance;
			}
			
			if(Vector3.Distance(transform.position, player.position) < agent.stoppingDistance + 0.1f){
				agent.isStopped = true;
				transform.LookAt(player.position);
				anim.SetInteger("State", 3);
				spawned = false;
				
				StartCoroutine(Attack());
			}
		}
		else{
			RandomWalk();
		}
	}
	
	void ContinueWalking(){
		anim.SetInteger("State", 1);
		randomTarget = area.RandomPosition();
		agent.speed = walkspeed;
		agent.isStopped = false;
		spawned = true;
	}
	
	void RandomWalk(){
		if(Vector3.Distance(transform.position, randomTarget) < agent.stoppingDistance + 0.1f){
			randomTarget = area.RandomPosition();
		}
		else{
			agent.destination = randomTarget;
		}
	}
	
	public void Hit(){
		
		if(angry){
			ang = true;
		}
		else{
			angry = true;
			StartCoroutine(Coin());
		}
	}
	
	IEnumerator Attack(){
		yield return new WaitForSeconds(0.5f);
		
		if(player != null && Vector3.Distance(transform.position, player.position) > agent.stoppingDistance + 0.1f){
			agent.isStopped = false;
			anim.SetInteger("State", 2);
			spawned = true;
		}
		else{
			manager.GameOver();
			ContinueWalking();
		}
	}


	IEnumerator Coin()
	{
		coin.SetActive(true);
		yield return new WaitForSeconds(1f);
		coin.SetActive(false);
	}
	
}
