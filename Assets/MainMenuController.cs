using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

	[SerializeField] Button playButton = default;
    [SerializeField] Button creditsButton = default;
	[SerializeField] Button backButton = default;

	void Start() {
        playButton.onClick.AddListener(Play);
        creditsButton.onClick.AddListener(Credits);
        backButton.onClick.AddListener(UnCredits);
    }

    void Play() { }
    void Credits() { }
    void UnCredits() { }
}
