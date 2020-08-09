using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RangedWeaponController))]
public class RangedWeaponEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        RangedWeaponController ws = (RangedWeaponController)target;

        //if (GUILayout.Button("Start Firing Weapon"))
        //{
        //    ws.StartFiringWeapon();
        //}

        //if (GUILayout.Button("Stop Firing Weapon"))
        //{
        //    ws.StopFiringWeapon();
        //}
    }
}