using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourMachine;

[NodeInfo(category = "Custom/", icon = "DefaultAsset", description = "Clyde Caged.")]
public class ClydeCagedActionNode : ActionNode
{

    public FsmEvent readyEvent;

    bool ready = true;

    public override void Reset()
    {
        readyEvent = new ConcreteFsmEvent();
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override Status Update()
    {
        if (GameDirector.Instance.pelletsRemaining <= GameDirector.Instance.totalPellets * 0.66f)
        {
            if (readyEvent.id != 0)
            {
                owner.root.SendEvent(readyEvent.id);
                return Status.Success;
            }
            return Status.Failure;
        }


        return Status.Running;
    }
}
