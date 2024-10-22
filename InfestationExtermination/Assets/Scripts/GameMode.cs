using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mode
{
    BuildMode,
    WaveMode,
    Pause
}
public class GameMode : MonoBehaviour
{
    private Mode mode = Mode.BuildMode;

    public Mode Mode1
    {
        get => mode;
        set => mode = value;
    }
}
