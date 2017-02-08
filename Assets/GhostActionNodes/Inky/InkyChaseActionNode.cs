using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourMachine;

[NodeInfo(category = "Custom/", icon = "DefaultAsset", description = "Pinky Chase.")]
public class InkyChaseActionNode : ActionNode
{

    public FsmEvent scatterEvent;
    public FsmEvent deathEvent;

    PacmanController pacman;
    GhostController blinky;
    GhostController pinky;
    GhostController controller;

    public override void Reset()
    {
        scatterEvent = new ConcreteFsmEvent();
        deathEvent = new ConcreteFsmEvent();

        pacman = GameObject.FindObjectOfType<PacmanController>();
        blinky = GameObject.Find("Blinky").GetComponent<GhostController>();
        pinky = GameObject.Find("Pinky").GetComponent<GhostController>();
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

        // scatter transition
        if (GameDirector.Instance.state == GameDirector.States.enState_PacmanInvincible)
        {
            if (scatterEvent.id != 0)
            {
                owner.root.SendEvent(scatterEvent.id);
                return Status.Success;
            }
            return Status.Failure;
        }

        // aims for point between Pinky and Blinky
        Vector2 pinkyPos = new Vector2(pinky.transform.position.x, pinky.transform.position.y);
        Vector2 blinkyPos = new Vector2(blinky.transform.position.x, blinky.transform.position.y);

        controller.moveToLocation = blinkyPos + (pinkyPos - blinkyPos) / 2;

        return Status.Running;
    }
}
