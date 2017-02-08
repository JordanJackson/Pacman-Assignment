using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourMachine;

[NodeInfo(category = "Custom/", icon = "DefaultAsset", description = "Pinky Scatter.")]
public class PinkyScatterActionNode : ActionNode
{
    public Vector2[] scatterLocations;

    public FsmEvent chaseEvent;
    public FsmEvent deathEvent;

    int locationIndex = 0;

    GhostController controller;

    public override void Reset()
    {
        chaseEvent = new ConcreteFsmEvent();
        deathEvent = new ConcreteFsmEvent();

        controller = owner.root.gameObject.GetComponent<GhostController>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override Status Update()
    {

        // death transition
        if (controller.isDead)
        {
            if (deathEvent.id != 0)
            {
                owner.root.SendEvent(deathEvent.id);
                return Status.Success;
            }
            return Status.Failure;
        }

        // chase transition
        if (GameDirector.Instance.state == GameDirector.States.enState_Normal)
        {
            if (chaseEvent.id != 0)
            {
                owner.root.SendEvent(chaseEvent.id);
                return Status.Success;
            }
            return Status.Failure;
        }

        if (Vector2.Distance(new Vector2(controller.transform.position.x, controller.transform.position.y), scatterLocations[locationIndex]) < 5)
        {
            locationIndex++;
            locationIndex = locationIndex % scatterLocations.Length;
        }

        // pinky goes from corner to corner
        controller.moveToLocation = scatterLocations[locationIndex];

        return Status.Running;
    }
}
