using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Theme", menuName = "Theme")]
public class Theme : ScriptableObject
{
    public new string name;
    public Color fogColor;
    public Color backgroundColor;
    public Material playerMaterial;
    public Material groundMaterial;
    public Material multiplierMaterial;
    public Material cloudsMaterial;
    public Material obstaclesMaterial;
}
