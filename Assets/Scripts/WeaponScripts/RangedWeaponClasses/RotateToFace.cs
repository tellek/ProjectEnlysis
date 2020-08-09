using System;
using UnityEngine;

namespace Assets.Scripts.WeaponScripts.RangedWeaponClasses
{
    public static class RotateToFace
    {
        public static void FaceTarget(GameObject target, Transform rotator, bool lockX, bool lockY, bool lockZ)
        {
            rotator.LookAt(target.transform);
            var rot = rotator.localRotation;
            if (lockX) rot.x = 0;
            if (lockY) rot.y = 0;
            if (lockZ) rot.z = 0;
            rotator.localRotation = rot;
        }
    }
}
