using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class ShelvesManager : MonoBehaviour {
	[SerializeField] DragableItem itemPrefab = default;

	List<Transform> shelves;

	Dictionary<Transform, DragableItem> itemObjects;

	void Awake() {
		var shelvesParent = transform.Find("Shelves");
		shelves = new List<Transform>(shelvesParent.childCount);
		itemObjects = new Dictionary<Transform, DragableItem>(shelvesParent.childCount);

		for (int i = 0; i < shelvesParent.childCount; i++) {
			shelves.Add(shelvesParent.GetChild(i));
			itemObjects.Add(shelvesParent.GetChild(i), null);
		}
	}

	public void AddItemToShelves(Item item) {
		var emptyShelf = itemObjects.First(x => x.Value == null);

		var newItem = Instantiate(itemPrefab);
		newItem.transform.position = emptyShelf.Key.position;
		newItem.ConfirmInitialization(item);

		itemObjects[emptyShelf.Key] = newItem;
	}

	public void RemoveItem(Item item) {
		var occupiedShelf = itemObjects.First(x => x.Value != null && x.Value.item == item);
		Destroy(itemObjects[occupiedShelf.Key].gameObject);
		itemObjects[occupiedShelf.Key] = null;
	}

}
