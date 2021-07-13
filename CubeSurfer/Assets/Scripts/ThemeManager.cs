using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : Singleton<ThemeManager>
{
    public List<Theme> themes;

    protected override void Awake()
    {
        isDestroyableOnLoad = false;
        base.Awake();
    }
}
