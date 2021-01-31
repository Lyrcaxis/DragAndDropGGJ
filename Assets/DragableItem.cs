using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class DragableItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler {
	public Color clr;
	SpriteRenderer spr;

	Vector3 OriginalPosition;
	public Item item { get; private set; }


	int initialSortingOrder;
	int initialLayer;

	void Awake() {
		spr = GetComponent<SpriteRenderer>();
		initialLayer = spr.sortingLayerID;
		initialSortingOrder = spr.sortingOrder;
	}

	public void ConfirmInitialization(Item item, int orderInLayer) {
		OriginalPosition = transform.position;

		spr.sprite = item.sprite;
		spr.sortingOrder = orderInLayer;
		gameObject.AddComponent<PolygonCollider2D>();

		this.item = item;
		StartCoroutine(FadeInAlpha());


		IEnumerator FadeInAlpha() {
			float t = 0;
			while (t < 1) {
				t += Time.deltaTime;
				spr.color = new Color(1, 1, 1, t);
				yield return null;
			}
		}
	}


	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) => spr.color = clr;
	void IPointerExitHandler.OnPointerExit(PointerEventData eventData) => spr.color = Color.white;

	void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) => spr.sortingLayerName = "Items";
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

		spr.sortingLayerID = initialLayer;
	}

	static Vector3 GetMousePos() {
		var mousePos = Input.mousePosition;
		var worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, -10));

		return new Vector3(worldMousePos.x, worldMousePos.y, 0);
	}
}
