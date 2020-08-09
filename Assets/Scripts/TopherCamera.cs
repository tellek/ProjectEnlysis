using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

/// <summary>
/// Topher's Smooth Camera
/// Usage: Just make the camera this script is attached to a child of your player gameobject and adjust accordingly.
/// </summary>
public class TopherCamera : MonoBehaviour
{
    [Range(0f, 6f)]
    public float HorizontalMultiplier = 2f;
    [Range(0f, 5f)]
    public float VerticalMultiplier = 1f;
    [Range(0f, 4f)]
    public float SmoothTime = 0.5f;
    [Range(0f, 5f)]
    public float BoostingCameraDistance = 2f;

    private float startingYPosition = 3.5f;
    private float startingZPosition = -5.8f;
    private Vector3 velocity = Vector3.zero;
    private Rewired.Player player;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
    }

    void Start()
    {
        startingYPosition = transform.localPosition.y;
        startingZPosition = transform.localPosition.z;
    }

    void Update()
    {
        var hPos = new Vector3(
                player.GetAxis("XThrust") * HorizontalMultiplier,
                transform.localPosition.y,
                transform.localPosition.z);

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, hPos, ref velocity, SmoothTime);

        var vPos = new Vector3(
                transform.localPosition.x,
                startingYPosition + player.GetAxis("YThrust") * VerticalMultiplier,
                transform.localPosition.z);

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, vPos, ref velocity, SmoothTime);

        Vector3 fPos;
        if (player.GetButton("Boost") && player.GetAxis("ZThrust") > 0f)
        {
            fPos = new Vector3(
                transform.localPosition.x,
                transform.localPosition.y,
                startingZPosition - BoostingCameraDistance);
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, fPos, ref velocity, SmoothTime * 2);
        }
        else
        {
            fPos = new Vector3(
                transform.localPosition.x,
                transform.localPosition.y,
                startingZPosition + player.GetAxis("ZThrust"));
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, fPos, ref velocity, SmoothTime);
        }

        


    }
}
