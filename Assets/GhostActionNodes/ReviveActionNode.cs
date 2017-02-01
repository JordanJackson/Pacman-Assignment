using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourMachine;

[NodeInfo(category = "Custom/", icon = "DefaultAsset", description = "Ghost Revive.")]
public class ReviveActionNode : ActionNode {

    public float reviveSpeed = 5.0f;

    GhostController controller;

    public FsmEvent readyEvent;

    public override void Reset()
    {
        controller = owner.root.gameObject.GetComponent<GhostController>();
    }

    public override void OnEnable()
    {
        base.OnEnable();

        controller.Revive();
        controller.speed = reviveSpeed;
    }

    public override Status Update()
    {
        if (readyEvent.id != 0)
        {
            owner.root.SendEvent(readyEvent.id);
            return Status.Success;
        }
        return Status.Failure;
    }
}
