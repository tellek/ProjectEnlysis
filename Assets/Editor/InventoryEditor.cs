using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InventoryController))]
public class InventoryEditor : Editor {

    private SerializedProperty itemProperties;

    private const string inventoryPropItems = "Items";

    private InventoryController _inv;
    private int addedItems = 0;

    private void OnEnable()
    {
        itemProperties = serializedObject.FindProperty(inventoryPropItems);
        _inv = (InventoryController)target;
        _inv.AddItem("Item1", 5);
        _inv.AddItem("Item2", 10);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();

        if (GUILayout.Button("Add Item"))
        {
            _inv.AddItem("New Item " + addedItems++, 1);
        }

        //for (int i = 0; i < _inv.Items.Count; i++)
        //{
        //    SetItemsGUI(i);
        //}

        foreach (var item in _inv.Items)
        {
            SetItemsGUI(item);
        }

        serializedObject.ApplyModifiedProperties();

        
    }

    private void SetItemsGUI(InventoryItem item)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.LabelField("Name", item.Name);
        EditorGUILayout.IntField("Amount", item.Amount);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add 1"))
        {
            _inv.AddItem(item.Name, 1);
        }
        if (GUILayout.Button("Remove 1"))
        {
            _inv.RemoveItem(item.Name, 1);
        }
        if (GUILayout.Button("Delete"))
        {
            _inv.RemoveItem(item.Name, item.Amount);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }
}
