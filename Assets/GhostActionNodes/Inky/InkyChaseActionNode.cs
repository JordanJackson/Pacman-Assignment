using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourMachine;

[NodeInfo(category = "Custom/", icon = "DefaultAsset", description = "Pinky Chase.")]
public class InkyChaseActionNode : ActionNode
{

    public FsmEvent scatterEvent;
    public FsmEvent deathEvent;

    bool scatter = false;

    float currentTime;
    public float chaseTime;

    PacmanController pacman;
    GhostController blinky;
    GhostController controller;

    public override void Reset()
    {
        scatterEvent = new ConcreteFsmEvent();
        deathEvent = new ConcreteFsmEvent();

        pacman = GameObject.FindObjectOfType<PacmanController>();
        blinky = GameObject.Find("Blinky").GetComponent<GhostController>();
        controller = owner.root.gameObject.GetComponent<GhostController>();
    }

    public override void OnEnable()
    {
        base.OnEnable();


        currentTime = 0.0f;
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
        currentTime += Time.deltaTime;

        if (currentTime > chaseTime)
        {
            if (scatterEvent.id != 0)
            {
                owner.root.SendEvent(scatterEvent.id);
                return Status.Success;
            }
            return Status.Failure;
        }

        // 2 * Vector from blinky to 2 * Pacman pos + Pacman Direction * 2
        Vector2 pacPos = new Vector2(pacman.transform.position.x, pacman.transform.position.y);
        pacPos += 2 * pacman.MoveDirections[(int)pacman.moveDirection];
        Vector2 blinkyPos = new Vector2(blinky.transform.position.x, blinky.transform.position.y);
        pacPos -= blinkyPos;
        pacPos *= 2;
        controller.moveToLocation = blinkyPos + pacPos;

        return Status.Running;
    }
}
