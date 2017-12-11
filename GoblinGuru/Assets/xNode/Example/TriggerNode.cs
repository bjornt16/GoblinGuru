using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using UnityEngine.Events;

[System.Serializable]
public class TriggerNode : Node
{
    public UnityEvent trigger;

    public void Trigger()
    {
        trigger.Invoke();
    }
}
