using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class StartGame : MonoBehaviour {
	public AudioSource mainSource;
	public AudioClip mainLoop;

	bool finished;

	void Update() {
		if (mainSource.loop || mainSource.isPlaying) { return; }
		mainSource.clip = mainLoop;
		mainSource.loop = true;
		mainSource.Play();
	}

	void LateUpdate() {
		if (finished) { return; }
		var T = Mathf.InverseLerp(0, 21.26f, transform.position.y);
		var vol = Mathf.Lerp(0.05f, 0.25f, T);
		mainSource.volume = Mathf.MoveTowards(mainSource.volume, vol, 0.02f * Time.deltaTime);

		if (mainSource.volume == 0.05f) { finished = true; }
	}

	public void InitiateGameBegin() => FindObjectOfType<ShopManager>().canSpawn = true;
}
