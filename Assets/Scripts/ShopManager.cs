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

	Customer CurrentCustomer { get; set; }
	[SerializeField] List<Item> allItems;
	List<Item> inventory;

	float t = 0;

	void Start() {
		inventory = new List<Item>(maxInventoryCount);

		for (int i = 0; i < maxInventoryCount; i++) {
			AddRandomItemToInventory();
		}
	}

	void Update() {
		if (CurrentCustomer == null) {
			if ((t += Time.deltaTime) >= intervalBetweenCustomers) {
				SpawnNewCustomer();
				t = 0;
			}
		}
	}

	void AddRandomItemToInventory() {
		var item = GetRandomItem();
		allItems.Remove(item);
		inventory.Add(item);
		GetComponent<ShelvesManager>().AddItemToShelves(item);

		Item GetRandomItem() => allItems[Random.Range(0, allItems.Count)];
	}

	void SpawnNewCustomer() {
		float currentMulti = 10f;
		float standardPayment = 1f;
		int wrongAnswers = 0;

		var newCustomer = Instantiate(customerPrefabs[Random.Range(0, customerPrefabs.Count)]);
		newCustomer.wantedItem = inventory[Random.Range(0, inventory.Count)];
		newCustomer.OnDespawn += () => {
			inventory.Remove(newCustomer.wantedItem);
			GetComponent<ShelvesManager>().RemoveItem(newCustomer.wantedItem);
			AddRandomItemToInventory();
			CurrentCustomer = null;
		};
		newCustomer.OnCorrectAnswer += () => {
			UIManager.AddMoney((int) (currentMulti * standardPayment));
			newCustomer.Leave();
		};
		newCustomer.OnWrongAnswer += () => {
			currentMulti *= 0.8f;
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