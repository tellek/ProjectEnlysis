using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EngineController : MonoBehaviour {

    public float LowItensity = 1.4f;
    public float MediumItensity = 1.8f;
    public float HighItensity = 2.2f;
    public bool isPlayer = true;
    public float MediumVolume = 0.5f;
    public float HighVolume = 1.0f;
    public float LowLightIntensity = 1.0f;
    public float MediumLightIntensity = 2.0f;
    public float HighLightIntensity = 3.0f;

    private Renderer thisRenderer;
    private AudioSource EngineSound;
    private Light EngineLight;
    private bool isMoving = false;

    void Start()
    {
        thisRenderer = GetComponent<Renderer>();
        EngineSound = GetComponent<AudioSource>();
        EngineSound.enabled = false;
        EngineLight = GetComponent<Light>();
    }

	void Update () {
        if (!isPlayer) return;

        if (Input.GetKeyDown("w")) EngageEngine();

        if (Input.GetKeyUp("w")) DisableEngine();

        if (isMoving)
        {
            if (Input.GetKey("left shift"))
            {
                EngageBoost();
            }
            else EngageEngine(false);
        }
	}

    private void EngageEngine(bool triggerIsMoving = true)
    {
        EngineSound.mute = false;
        EngineSound.enabled = true;
        EngineSound.volume = MediumVolume;
        thisRenderer.material.SetFloat("_Glow", MediumItensity); // Raises shader Intensity
        EngineLight.intensity = MediumLightIntensity;
        if (triggerIsMoving) isMoving = true;
    }

    private void EngageBoost()
    {
        EngineSound.mute = false;
        EngineSound.enabled = true;
        EngineSound.volume = HighVolume;
        thisRenderer.material.SetFloat("_Glow", HighItensity); // Raises shader Intensity
        EngineLight.intensity = HighLightIntensity;
    }

    private void DisableEngine()
    {
        EngineSound.mute = true;
        thisRenderer.material.SetFloat("_Glow", LowItensity); // Lowers shader Intensity
        EngineLight.intensity = LowLightIntensity;
        isMoving = false;
    }
}
