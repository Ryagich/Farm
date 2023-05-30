using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorExtensions
{
    public static Color WithA(this Color color, float a) => new Color(color.r, color.g, color.b, a);
}
