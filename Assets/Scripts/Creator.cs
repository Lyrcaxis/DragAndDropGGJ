//using UnityEngine;
//using UnityEditor;
//using System.Collections.Generic;
//using System.Text.RegularExpressions;

//public class Creator : MonoBehaviour {
//	[ContextMenu("Create SOs")]
//	void CreateScriptableObjects() {
//		var GUIDs = AssetDatabase.FindAssets("", new[] { "Assets/Sprites/Items" });
//		foreach (var GUID in GUIDs) {
//			var itemPath = AssetDatabase.GUIDToAssetPath(GUID);
//			var item = AssetDatabase.LoadAssetAtPath<Sprite>(itemPath);

//			var newSO = Item.CreateInstance<Item>();
//			newSO.name = item.name;
//			newSO.sprite = item;
//			AssetDatabase.CreateAsset(newSO, $"Assets/ScriptableObjects/Items/{newSO.name}.asset");
//		}
//	}
//}
