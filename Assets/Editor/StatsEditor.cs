using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(Stats))]
public class StatsEditor : Editor {

	private Stats _stats;

	private void OnEnable()
    {
		_stats = (Stats)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();

		EditorGUILayout.Space();
		
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Damage:");
		if (GUILayout.Button("I"))
        {
            _stats.DamageHull(25, Resistance.Impact);
        }
        if (GUILayout.Button("V"))
        {
            _stats.DamageHull(25, Resistance.Volt);
        }
        if (GUILayout.Button("H"))
        {
            _stats.DamageHull(25, Resistance.Heat);
        }
		if (GUILayout.Button("R"))
        {
            _stats.DamageHull(25, Resistance.Radiation);
        }
		if (GUILayout.Button("N"))
        {
            _stats.DamageHull(25, Resistance.Nanobots);
        }
		if (GUILayout.Button("Repair"))
        {
            _stats.RepairHull(50);
        }
        EditorGUILayout.EndHorizontal();
		if (GUILayout.Button("Update All Stats"))
        {
            _stats.UpdateAllStats();
        }


        serializedObject.ApplyModifiedProperties();
    }

}
