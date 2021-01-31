using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class ShopManager : MonoBehaviour {
	[SerializeField] List<Customer> customerPrefabs = default;
	[Space]
	[SerializeField] float intervalBetweenCustomers = default;
	[SerializeField] float currentMultiplier = default;
	[Space]
	[SerializeField] int maxInventoryCount = default;

	[SerializeField] AudioSource doorSrc = default;
	[SerializeField] GameObject door = default;
	[SerializeField] AudioClip correctClip = default;
	[SerializeField] AudioClip wrongClip = default;
	Customer CurrentCustomer { get; set; }
	[SerializeField] List<Item> allItems;
	List<Item> inventory;

	float t = 0;

	int totalCorrectAnswers;

	public bool canSpawn = true;

	void Start() {
		inventory = new List<Item>(maxInventoryCount);

		for (int i = 0; i < maxInventoryCount; i++) {
			AddRandomItemToInventory();
		}
	}

	void Update() {
		if (!canSpawn) { return; }

		if (CurrentCustomer == null) {
			if ((t += Time.deltaTime) >= intervalBetweenCustomers) {
				SpawnNewCustomer();
				t = 0;
			}
		}
	}

	void AddRandomItemToInventory() {
		if (allItems.Count == 0) { return; }

		var item = GetRandomItem();
		do {
			allItems.Remove(item);
			inventory.Add(item);
			GetComponent<ShelvesManager>().AddItemToShelves(item);

			if (allItems.Count == 0) { return; }
			item = GetRandomItem();
		} 
		while (item.clip == null);

		Item GetRandomItem() => allItems[Random.Range(0, allItems.Count)];
	}

	void SpawnNewCustomer() {
		float currentMulti = 10f;
		float standardPayment = 1f;
		int wrongAnswers = 0;

		var validItems = inventory.Where(x => x.clip != null).ToList();
		if (validItems.Count == 0) {
			Debug.Log("YOU WIN");
			return;
		}



		var newCustomer = Instantiate(customerPrefabs[Random.Range(0, customerPrefabs.Count)]);
		newCustomer.wantedItem = validItems[Random.Range(0, validItems.Count)];
		newCustomer.OnDespawn += () => {
			CurrentCustomer = null;
		};
		newCustomer.OnCorrectAnswer += () => {
			UIManager.AddMoney((int) (currentMulti * standardPayment));
			AudioSource.PlayClipAtPoint(correctClip, Vector3.zero);
			inventory.Remove(newCustomer.wantedItem);
			GetComponent<ShelvesManager>().RemoveItem(newCustomer.wantedItem);
			AddRandomItemToInventory();
			newCustomer.Leave();
		};
		newCustomer.OnWrongAnswer += () => {
			currentMulti *= 0.8f;
			AudioSource.PlayClipAtPoint(wrongClip, Vector3.zero);
			if (++wrongAnswers == 3) { newCustomer.Leave(); }
		};
		newCustomer.Initialize();

		CurrentCustomer = newCustomer;

		StartCoroutine(OnCustomerWalkAnim());
	}

	IEnumerator OnCustomerWalkAnim() {
		doorSrc.Play();
		door.gameObject.SetActive(true);
		yield return new WaitForSeconds(0.5f);
		door.gameObject.SetActive(false);
	}

}