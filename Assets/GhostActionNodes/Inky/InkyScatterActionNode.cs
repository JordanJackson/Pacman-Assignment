using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourMachine;

[NodeInfo(category = "Custom/", icon = "DefaultAsset", description = "Pinky Scatter.")]
public class InkyScatterActionNode : ActionNode
{
    public FsmEvent chaseEvent;
    public FsmEvent deathEvent;

    GhostController pinky;
    GhostController blinky;
    GhostController clyde;

    GhostController controller;

    public override void Reset()
    {
        chaseEvent = new ConcreteFsmEvent();
        deathEvent = new ConcreteFsmEvent();

        blinky = GameObject.Find("Blinky").GetComponent<GhostController>();
        pinky = GameObject.Find("Pinky").GetComponent<GhostController>();
        clyde = GameObject.Find("Clyde").GetComponent<GhostController>();
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

        Transform target;
        if (Vector3.Distance(pinky.transform.position, controller.transform.position) <= Vector3.Distance(blinky.transform.position, controller.transform.position)
            && Vector3.Distance(pinky.transform.position, controller.transform.position) <= Vector3.Distance(clyde.transform.position, controller.transform.position))
        {
            target = pinky.transform;
        }
        else if (Vector3.Distance(blinky.transform.position, controller.transform.position) <= Vector3.Distance(clyde.transform.position, controller.transform.position))
        {
            target = blinky.transform;
        }
        else
        {
            target = clyde.transform;
        }


        // Inky follows the closest ghost to him
        controller.moveToLocation = target.position;

        return Status.Running;
    }
}
