using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StealMechanic : MonoBehaviour {
    enum StealState { Idle, Stealing }

	StealState state = default;
	Item stealingItem;

	[Header("Selection Panel Settings")]
	[SerializeField] GameObject stealSelectionPanel = default;
	[SerializeField] Transform layout = default;
	[SerializeField] Item selectedItem = default;


	[Header("Active Stealing settings")]
	[SerializeField] GameObject activeStealingPanel = default;
	[SerializeField] Image stealingImage = default;
	[SerializeField] float t = default;

	void Update() {
		if (state == StealState.Stealing) {
			t += Time.deltaTime;

			var T = t / Settings.TotalTimeToSteal;

			if (T >= 1) { FinishStealItem(); }
			else {
				var rt = stealingImage.GetComponent<RectTransform>();

				var offset = rt.offsetMax;
				offset.x = Mathf.Lerp(0, 500, T);
				rt.offsetMax = offset;
			}
		}
	}

	public void ConfirmStealItem() {
        stealingItem = default;
        stealingImage.sprite = stealingItem.sprite;
		t = 0;
    }

	void FinishStealItem() {
		state = StealState.Idle;
	}
}

public class Settings {
	public static float TotalTimeToSteal = 50;

}

public class ItemUI : MonoBehaviour {
	public Item item { get; private set; }
	public Button button = default;

	[SerializeField] Image image = default;

	public void SetItem(Item item) {
		this.item = item;
		image.sprite = item.sprite;
	}
}