using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	
	public float force;
	public float lifetime;
	public int mans = 0;
	
	[HideInInspector]
	public PlayerController player;
	
	void OnTriggerEnter(Collider other){
		if(!other.gameObject.CompareTag("Enemy"))
			return;
		
		other.gameObject.GetComponent<Enemy>().Hit();
		if (!other.gameObject.GetComponent<Enemy>().ang)
		{
			mans++;
		}
	}
	
	IEnumerator DestroySelf(){
		yield return new WaitForSeconds(lifetime);
		
		player.DisableBullet(gameObject);
	}
}
