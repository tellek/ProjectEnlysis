/*  Camera Follow C# Script (version: 1.5)
	SPACE for UNITY - Space Scene Construction Kit
	https://www.imphenzia.com/space-for-unity
	(c) 2017 Imphenzia AB

    DESCRIPTION:
    Smooth camera follow script used to follow an object (Transform)

    INSTRUCTIONS:
    Attach this script to a camera (e.g. Main Camera) and specify which target Transform to
    follow.

    You can also modify parameters such as:
     updateMode		(to reduce camera jitter - camera can be updated in FixedUpdate, Update, or LateUpdate)
     followMode		(whether camera should chase behind Transform or as a spectator that follows the Transform)
     distance		(minimum distance to target)
     chaseHeight	(height over target in chase mode)
     followDamping	(smoothness of movement, lower value is smoother)
     lookAtDamping	(smoothness for rotation, lower value is smoother)
     freezeKey		(freeze camera movement while this key is pressed)

    Version History
    1.5     - Added ORBIT follow mode
    1.02    - Renamed to SU_CameraFollow to avoid naming conflicts.
    1.01    - Initial Release.
*/

using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{
    public Transform target;

    // Distance to follow from (this is the minimum distance, 
    // depending on damping the distance will increase at speed)
    public float distance = 60.0f;
    // Height for chase mode camera
    public float chaseHeight = 15.0f;

    // Follow (movement) damping. Lower value = smoother
    //public float followDamping = 0.3f;
    // Look at (rotational) damping. Lower value = smoother
    //public float lookAtDamping = 4.0f;

    public float lookOffset = 10f;

    //public float smoothTime = 0.3F;
    //private Vector3 velocity = Vector3.zero;

    private void FixedUpdate()
    {
        // Return if no target is set
        if (target == null) return;

        Quaternion _lookAt;

        // Smooth lookat interpolation
        _lookAt = target.rotation;
        //transform.rotation = Quaternion.Lerp(transform.rotation, _lookAt, Time.deltaTime * lookAtDamping);
        var pos = new Vector3(target.position.x, target.position.y, target.position.z);
        transform.LookAt(pos);

        // Smooth follow interpolation
        //transform.position = Vector3.Lerp(transform.position, target.position - target.forward * distance + target.up * chaseHeight, Time.deltaTime * followDamping * 2);


        // Define a target position above and behind the target transform
        //Vector3 targetPosition = target.TransformPoint(new Vector3(0, 5, -10));

        // Smoothly move the camera towards that target position
        //transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        transform.position = target.TransformPoint(new Vector3(0, chaseHeight, distance));
    }
}
