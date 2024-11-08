using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Build,
    Wave,
    Pause
}
public class GameState : MonoBehaviour
{
    private State state = State.Build;

    public State State1
    {
        get => state;
        set => state = value;
    }
}
