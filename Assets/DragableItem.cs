using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragableItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler {
	PolygonCollider2D col;
	SpriteRenderer spr;

	Vector3 OriginalPosition;
	public Item item { get; private set; }


	public void ConfirmInitialization(Item item) {
		OriginalPosition = transform.position;

		spr = GetComponent<SpriteRenderer>();
		spr.sprite = item.sprite;
		col = gameObject.AddComponent<PolygonCollider2D>();

		this.item = item;
	}


	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) => spr.color = Color.green;
	void IPointerExitHandler.OnPointerExit(PointerEventData eventData) => spr.color = Color.white;

	void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) => spr.sortingOrder++;
	void IDragHandler.OnDrag(PointerEventData eventData) => transform.position = GetMousePos();
	void IEndDragHandler.OnEndDrag(PointerEventData eventData) {
		var hitCol = Physics2D.OverlapBox(GetMousePos(), Vector2.one * 0.25f, 0, (1 << 10));

		if (hitCol != null) { hitCol.GetComponent<Customer>().OnDropItem(item); }
		StartCoroutine(MoveToOriginalPos());
	}

	IEnumerator MoveToOriginalPos() {
		const float moveSpeed = 25f;

		while (true) {
			transform.position = Vector3.MoveTowards(transform.position, OriginalPosition, moveSpeed * Time.deltaTime);
			if (transform.position == OriginalPosition) { break; }

			yield return null;
		}

		spr.sortingOrder--;
	}

	static Vector3 GetMousePos() {
		var mousePos = Input.mousePosition;
		var worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, -10));

		return new Vector3(worldMousePos.x, worldMousePos.y, 0);
	}
}
