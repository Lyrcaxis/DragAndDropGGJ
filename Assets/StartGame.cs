using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class StartGame : MonoBehaviour {
	public AudioSource mainSource;
	public AudioClip mainLoop;

	void Update() {
		if (mainSource.loop || mainSource.isPlaying) { return; }
		mainSource.clip = mainLoop;
		mainSource.loop = true;
		mainSource.Play();
	}

	void LateUpdate() {
		var T = Mathf.InverseLerp(0, 21.26f, transform.position.y);
		var vol = Mathf.Lerp(0.05f, 0.25f, T);

		mainSource.volume = Mathf.MoveTowards(mainSource.volume, vol, 0.02f * Time.deltaTime);
	}

	public void InitiateGameBegin() => FindObjectOfType<ShopManager>().canSpawn = true;
}
