using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Customer : MonoBehaviour {

	[SerializeField] AudioSource source = default;

	public Item wantedItem { get; set; }

	public System.Action OnCorrectAnswer { get; set; }
	public System.Action OnWrongAnswer { get; set; }


	public System.Action OnDespawn { get; set; }


	SpriteRenderer bubble { get; set; }
	SpriteRenderer note { get; set; }


	public void Initialize() {
		var spr = GetComponent<SpriteRenderer>();
		bubble = transform.GetChild(0).GetComponent<SpriteRenderer>();
		note = bubble.transform.GetChild(0).GetComponent<SpriteRenderer>();
		//note.sprite = wantedItem.sprite;

		bubble.color = note.color = Color.clear;

		StartCoroutine(InitializationAnim());


		IEnumerator InitializationAnim() {
			float t = 0;
			while (t < 1) {
				t += Time.deltaTime;
				spr.color = new Color(1, 1, 1, t);
				yield return null;
			}

			yield return new WaitForSeconds(0.5f);

			t = 0;
			while (t < 1) {
				t += Time.deltaTime * 2f;
				bubble.color = new Color(1, 1, 1, t);
				note.color = new Color(1, 1, 1, t);
				yield return null;
			}

			yield return new WaitForSeconds(0.5f);

			if (wantedItem.clip != null) {
				source = gameObject.AddComponent<AudioSource>();
				source.clip = wantedItem.clip;
				source.loop = true;
				source.Play();
			}
		}
	}

	public void OnDropItem(Item item) {
		//bool t = ItemEvaluator.EvaluateItem(this, item);

		if (item == wantedItem) {
			OnCorrectAnswer?.Invoke();
		}
		else {
			OnWrongAnswer?.Invoke();
		}
	}

	public void Leave() {
		OnDespawn?.Invoke();
		//source.Stop();

		var spr = GetComponent<SpriteRenderer>();
		StartCoroutine(LeaveAnim());

		IEnumerator LeaveAnim() {
			float t = 0;
			while (t < 1) {
				t += Time.deltaTime * 4f;
				bubble.color = new Color(1, 1, 1, 1 - t);
				note.color = new Color(1, 1, 1, 1 - t);
				if (source) { source.volume = 1 - t; }
				yield return null;
			}
			t = 0;
			while (t < 1) {
				t += 2 * Time.deltaTime;
				spr.color = new Color(1, 1, 1, 1 - t);
				yield return null;
			}

			Destroy(gameObject);
		}
	}
}

public class ItemEvaluator : MonoBehaviour {
	[SerializeField] AudioClip correctSound = default;
	[SerializeField] AudioClip wrongSound = default;

	public int CorrectChoices, WrongChoices;

	static ItemEvaluator instance;


	public static void EvaluateItem(Customer customer, Item givenItem) {
		bool wasCorrect = givenItem == customer.wantedItem;
		if (wasCorrect) {
			AudioSource.PlayClipAtPoint(instance.correctSound, Vector3.zero);
			instance.CorrectChoices++;
		}
		else {
			AudioSource.PlayClipAtPoint(instance.wrongSound, Vector3.zero);
			instance.WrongChoices++;
		}
	}

}