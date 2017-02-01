using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinky : MonoBehaviour
{

    Transform pacMan;

    GhostController ghostController;

    private void Start()
    {
        pacMan = GameObject.Find("Pacman").transform;
        ghostController = GetComponent<GhostController>();
    }

    private void Update()
    {
        ghostController.moveToLocation = pacMan.position;
    }
}
