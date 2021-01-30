using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidePanel : MonoBehaviour {
	public static SidePanel ActivePanel;
	public float moveSpeed = 1000;

	public void TogglePanel() {
		if (ActivePanel != this) {
			if (ActivePanel != null) { ActivePanel.Disappear(2, Appear); }
			else { Appear(); }
		}
		else { Disappear(1.5f); }
	}

	void Appear() {
		StopAllCoroutines();
		StartCoroutine(AnimatePanel(1350));
		ActivePanel = this;
	}
	void Disappear(float speedMulti = 1, System.Action queuedAction = null) {
		StopAllCoroutines();
		StartCoroutine(AnimatePanel(1920, speedMulti, queuedAction));
		if (ActivePanel == this) { ActivePanel = null; }
	}

	IEnumerator AnimatePanel(int targetX, float speedMulti = 1, System.Action callback = null) {
		var rTransform = GetComponent<RectTransform>();
		var offset = rTransform.offsetMax;

		while (true) {
			offset.x = Mathf.MoveTowards(offset.x, -targetX, speedMulti * moveSpeed * Time.deltaTime);
			rTransform.offsetMax = offset;

			if (offset.x == -targetX) { break; }
			yield return null;
		}

		callback?.Invoke();
	}
}
