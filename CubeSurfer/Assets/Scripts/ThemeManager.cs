using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ThemeColor
{
    public string name;
    public Color fogColor;
    public Color backgroundColor;
    public Material playerMaterial;
    public Material groundMaterial;
    public Material multiplierMaterial;
    public Material cloudsMaterial;
    public Material obstaclesMaterial;
}

public class ThemeManager : Singleton<ThemeManager>
{
    public List<ThemeColor> themes;

    protected override void Awake()
    {
        isDestroyableOnLoad = false;
        base.Awake();
    }
}
