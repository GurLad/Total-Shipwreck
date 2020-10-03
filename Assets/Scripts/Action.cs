using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType { Move, Pick, Use }
public class Action
{
    public ActionType Type;
    public Vector3 pos;

    public Action(ActionType type)
    {
        Type = type;
    }

    public Action(ActionType type, Vector3 pos)
    {
        Type = type;
        this.pos = pos;
    }
}
