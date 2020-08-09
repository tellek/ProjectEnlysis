using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostEngineControl : MonoBehaviour {

    public bool isPlayer = true;
    public string TriggerKey = "";

    public float Itensity = 2.2f;
    public float LightIntensity = 3.0f;
    public float IdleItensity = 1.4f;
    public float IdleLightIntensity = 1.0f;
    public float SecondsToWait = 1f;

    private Renderer thisRenderer;
    private AudioSource EngineSound;
    private Light EngineLight;
    private bool isActive = false;

    void Start()
    {
        thisRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (!isPlayer) return;
        if (string.IsNullOrEmpty(TriggerKey)) return;
        if (Input.GetKeyDown(TriggerKey)) StartCoroutine(TriggerBoost());
    }

    IEnumerator TriggerBoost()
    {
        EngageEngine();
        yield return new WaitForSeconds(SecondsToWait);
        DisableEngine();
    }

    private void EngageEngine()
    {
        thisRenderer.material.SetFloat("_Glow", Itensity); // Raises shader Intensity
        EngineLight.intensity = LightIntensity;
        isActive = true;
    }

    private void DisableEngine()
    {
        thisRenderer.material.SetFloat("_Glow", IdleItensity); // Lowers shader Intensity
        EngineLight.intensity = IdleLightIntensity;
        isActive = false;
    }
}
