using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class UIManager : MonoBehaviour
{
	public TextMeshProUGUI money;

	public int CurrentCash { get; set; }

	static UIManager instance;

	void Awake() {
		instance = this;
		money.text = "0 $";
	}

	public static void AddMoney(int cashToAdd) {
		instance.CurrentCash += cashToAdd;
		instance.money.text = $"{instance.CurrentCash} $";
	}
}
