using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

	[SerializeField] Button playButton = default;
	[SerializeField] Button creditsButton = default;
	[SerializeField] Button backButton = default;
	[SerializeField] Animator buttonsAnim = default;
	[SerializeField] Animator cameraAnim = default;
	
	void Start() {
		playButton.onClick.AddListener(Play);
		creditsButton.onClick.AddListener(Credits);
		backButton.onClick.AddListener(UnCredits);
	}

	void Play() {
		cameraAnim.Play("Play");
		playButton.onClick.RemoveAllListeners();
		creditsButton.onClick.RemoveAllListeners();
	}
	void Credits() { buttonsAnim.Play("Credits"); }
	void UnCredits() { buttonsAnim.Play("Back"); }
}
