using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    public static Animation LoadingAnimator { get; set; }
    public static string[] LoadingAnimations { get; } = { "FadeInLoading", "FadeOutLoading" };

    public static int Level { get; set; } = 0;
}
