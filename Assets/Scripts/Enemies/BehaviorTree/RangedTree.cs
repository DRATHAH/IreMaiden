using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class RangedTree : MonoBehaviour
{
    BehaviorTree tree;
    public PlayerHealth player;

    public enum ActionState
    {
        IDLE,
        WORKING
    }
    ActionState state = ActionState.IDLE;
    NavMeshAgent agent;
    Node.Status treeStatus = Node.Status.RUNNING;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        tree = new BehaviorTree();
        // Leaf detect player
        // Selector Attack or Retreat
                // Leaf playerTooClose
                // Leaf player too far
            // Sequence attack player
                // Leaf get player within range
                // Leaf attack player
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
