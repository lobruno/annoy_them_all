using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayer : MonoBehaviour
{

	public Joystick joystick;
	public CharacterController controller;
	public Animator anim;
	public Image bulletIndicator;
	public Transform targetIndicator;
	public Transform target;
	public ParticleSystem movementEffect;
	public GameObject sound;
	public GameObject cone1;
	public GameObject cone2;
	public Bullet bull;
	public Bullet bullUlt;
	public GameObject ulta;
	public GameObject ultaBut;
	public Text score;


	public float speed;
	public float gravity;
	public GameObject people;

	Vector3 moveDirection;


	[HideInInspector]
	public bool safe;
	public int mans;

	Vector2 direction;
	Vector2 direction1;

	GameManager manager;

	bool useUlt = false;

	bool gameStart = false;

	void Start()
	{
		manager = GameObject.FindObjectOfType<GameManager>();
	}

	void Update()
	{
		if (!manager.gameStarted)
		{ return; }
		else { StartCoroutine(Wait()); }

		if (!gameStart)
			return;

		direction = joystick.direction;

        if (direction.x != 0|| direction.y != 0)
        {
			direction1.x = direction.x;
			direction1.y = direction.y;
			sound.SetActive(true);
			cone1.SetActive(true);
			cone2.SetActive(true);
		}


        if (controller.isGrounded)
		{
			moveDirection = new Vector3(direction1.x, 0, direction1.y);

			Quaternion targetRotation = moveDirection != Vector3.zero ? Quaternion.LookRotation(moveDirection) : transform.rotation;
			transform.rotation = targetRotation;

			moveDirection = moveDirection * speed;
		}

		moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);
		controller.Move(moveDirection * Time.deltaTime);

		mans = bull.mans + bullUlt.mans;

		bulletIndicator.fillAmount = (mans)/ (float)people.transform.childCount;

		score.text = "" + mans;

		if(bulletIndicator.fillAmount > 0.5 && !useUlt)
		{
			ultaBut.SetActive(true);
		}

		if (anim.GetBool("Moving") != (direction1 != Vector2.zero))
		{
			anim.SetBool("Moving", direction1 != Vector2.zero);

			if (direction1 != Vector2.zero)
			{
				movementEffect.Play();
			}
			else
			{
				movementEffect.Stop();
			}
		}


		if (!safe)
		{
			Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
			Quaternion targetIndicatorRotation = Quaternion.LookRotation((targetPosition - transform.position).normalized);
			targetIndicator.rotation = Quaternion.Slerp(targetIndicator.rotation, targetIndicatorRotation, Time.deltaTime * 50);
		}
	}


	public void SwitchSafeState(bool safe)
	{
		this.safe = safe;

		targetIndicator.gameObject.SetActive(!safe);
	}

	public void Ulta()
	{
		ulta.SetActive(true);
		useUlt = true;
		ultaBut.SetActive(false);
	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds(1f);
		gameStart = true;
	}


}