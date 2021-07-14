using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLights : MonoBehaviour
{
    [Header("Light Materials")]
    [SerializeField] Material ledsMaterial;
    [SerializeField] Material lightsMaterial;

    [Header("Audio")]
    [SerializeField] AudioClip switchOnAudio;
    [SerializeField] AudioClip switchOffAudio;
    [SerializeField] AudioSource lightsAudio;

    private Color _turnedLightsOffColor = new Color(r: 0, g: 0, b: 0, a: 0);
    private Color _lightBulbsEmissionColor = new Color(r: 1.304f, g: 1.270f, b: 1.086f, a: 1f);
    private Color _ledStripsEmissionColor = new Color(r: 1.662f, g: 1.662f, b: 1.662f, a: 1f);

    private const float SWITCH_ON_PITCH = 1.8f;
    private const float SWITCH_OFF_PITCH = 0.8f;

    void OnEnable() => 
        SwitchLamps(_lightBulbsEmissionColor, _ledStripsEmissionColor, SWITCH_ON_PITCH, switchOnAudio);

    void OnDisable() => 
        SwitchLamps(_turnedLightsOffColor, _turnedLightsOffColor, SWITCH_OFF_PITCH, switchOffAudio);

    void SwitchLamps(Color lampsColor, Color ledsColor, float pitch, AudioClip switchingAudio)
    {
        lightsMaterial.SetColor("_EmissionColor", lampsColor);
        ledsMaterial.SetColor("_EmissionColor", ledsColor);

        lightsAudio.pitch = pitch;
        lightsAudio.PlayOneShot(switchingAudio);
    }
}
