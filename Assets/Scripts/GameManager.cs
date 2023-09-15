using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {
	
	public Animator startPanel;
	public Animator gamePanel;
	public Animator gameOverPanel;
	public Animator WinPanel;
	public Animator startOverlay;
	public Animator moveCamera;
	public Animator transition;
	
	public Text bestText;
	public Text scoreText;

	public Text bestTextWin;
	public Text scoreTextWin;

	[HideInInspector]
	public bool gameStarted;
	bool win;
	
	public NewPlayer player;

	
	
	void Start(){
		int index = PlayerPrefs.GetInt("scene", 0);
		if (index != 0 && SceneManager.GetActiveScene().buildIndex != index)
		{
			SceneManager.LoadScene(index);
		}
		player = GameObject.FindObjectOfType<NewPlayer>();

		TinySauce.OnGameStarted(SceneManager.GetActiveScene().buildIndex + 1);

		var eventSystems = FindObjectsOfType<EventSystem>();
		int i = 0;
		foreach (var eventSystem in eventSystems)
		{
			i++;
		}
		if (i > 1) { eventSystems[1].gameObject.SetActive(false);  }
	}
	
	void Update(){
		if(Input.GetMouseButtonDown(0)){
			if (!gameStarted) {
				StartGame();
			}
			else if (win){
				StartCoroutine(NextGame());
			}
			else if (!gamePanel.gameObject.activeSelf) {
				StartCoroutine(RestartGame());
			}
		}
	}
	
	void StartGame(){
		gameStarted = true;
		
		moveCamera.SetTrigger("MoveCamera");
		startPanel.SetTrigger("Fade");
		startOverlay.SetTrigger("Fade");
		gamePanel.SetBool("Visible", true);
	}
    
	public void GameOver(){
		if(!gamePanel.gameObject.activeSelf)
			return;
		
		int score = player.mans;
		
		if(score > PlayerPrefs.GetInt("Best"))
			PlayerPrefs.SetInt("Best", score);
		
		bestText.text = PlayerPrefs.GetInt("Best") + "";
		scoreText.text = score + "";

		TinySauce.OnGameFinished(false, score, SceneManager.GetActiveScene().buildIndex + 1);

		gamePanel.gameObject.SetActive(false);
		gameOverPanel.SetTrigger("Game over");
		Destroy(player.gameObject);
		
	}



	public void Win()
	{
		if (!gamePanel.gameObject.activeSelf)
			return;

		int score = player.mans;
		win = true;

		if (score > PlayerPrefs.GetInt("Best"))
			PlayerPrefs.SetInt("Best", score);

		bestTextWin.text = PlayerPrefs.GetInt("Best") + "";
		scoreTextWin.text = score + "";

		TinySauce.OnGameFinished(true, score, SceneManager.GetActiveScene().buildIndex + 1);

		gamePanel.gameObject.SetActive(false);
		WinPanel.SetTrigger("Game over");
		player.speed = 0.0f;

	}



	IEnumerator RestartGame(){
		transition.SetTrigger("Transition");
		
		yield return new WaitForSeconds(0.5f);
		
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	IEnumerator NextGame()
	{
		transition.SetTrigger("Transition");

		yield return new WaitForSeconds(0.5f);

		if (SceneManager.GetActiveScene().buildIndex == 8)
		{
			SceneManager.LoadScene(0);
			PlayerPrefs.SetInt("scene", 0);
		}
		else
		{
			PlayerPrefs.SetInt("scene", SceneManager.GetActiveScene().buildIndex + 1);
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
	}
}
