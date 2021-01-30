using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class ShelvesManager : MonoBehaviour {
	[SerializeField] DragableItem itemPrefab = default;

	Dictionary<Sprite, SpriteRenderer> spriteDict;
	List<DragableItem> existingItems;

	void Awake() {
		var shelvesParent = transform.Find("Shelves");

		spriteDict = new Dictionary<Sprite, SpriteRenderer>();
		existingItems = new List<DragableItem>();

		for (int i = 0; i < shelvesParent.childCount; i++) {
			var child = shelvesParent.GetChild(i).GetComponent<SpriteRenderer>();
			spriteDict[child.sprite] = child;
			child.color = Color.clear;
		}
	}

	public void AddItemToShelves(Item item) {
		var itemPos = spriteDict[item.sprite].transform.position;

		var newItem = Instantiate(itemPrefab);
		newItem.transform.position = itemPos;
		newItem.ConfirmInitialization(item, spriteDict[item.sprite].sortingOrder);

		existingItems.Add(newItem);
	}

	public void RemoveItem(Item item) {
		var shelfItem = existingItems.Find(x => x.item == item);
		existingItems.Remove(shelfItem);
		Destroy(shelfItem.gameObject);
	}
}
