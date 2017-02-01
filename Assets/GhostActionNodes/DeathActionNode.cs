using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourMachine;

[NodeInfo(category = "Custom/", icon = "DefaultAsset", description = "Ghost Dead.")]
public class DeathActionNode : ActionNode {

    public float deadSpeed = 10.0f;

    GhostController controller;

    public FsmEvent reviveEvent;

    public override void Reset()
    {
        controller = owner.root.gameObject.GetComponent<GhostController>();
    }

    public override void OnEnable()
    {
        base.OnEnable();

        controller.moveToLocation = controller.ReturnLocation;
        controller.speed = deadSpeed;
    }

    public override Status Update()
    {
        // check if in cage again
        if (new Vector2(controller.transform.position.x, controller.transform.position.y) == controller.ReturnLocation)
        {
            if (reviveEvent.id != 0)
            {
                owner.root.SendEvent(reviveEvent.id);
                return Status.Success;
            }
            return Status.Failure;
        }

        return Status.Running;
    }
}
