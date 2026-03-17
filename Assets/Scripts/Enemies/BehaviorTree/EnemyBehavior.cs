using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    BehaviorTree tree;
    public GameObject diamond;
    public GameObject van;
    public GameObject backdoor;
    public GameObject frontdoor;
    [Range(0, 1000)]
    public int money = 800;

    public enum ActionState
    {
        IDLE,
        WORKING
    }
    ActionState state = ActionState.IDLE;

    NavMeshAgent agent;
    Node.Status treeStatus = Node.Status.RUNNING;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        tree = new BehaviorTree();
        Leaf hasGotMoney = new Leaf("Has Got Money", HasMoney);
        Sequence steal = new Sequence("Steal Something");
        Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
        Leaf goToVan = new Leaf("Go To Van", GoToVan);
        Leaf goToBackDoor = new Leaf("Go To Backdoor", GoToBackDoor);
        Leaf goToFrontDoor = new Leaf("Go To Frontdoor", GoToFrontDoor);
        Selector openDoor = new Selector("Open Door");

        //openDoor.AddChild(goToBackDoor);
        //openDoor.AddChild(goToFrontDoor);

        steal.AddChild(hasGotMoney);
        //steal.AddChild(openDoor);
        steal.AddChild(goToDiamond);
        steal.AddChild(goToVan);
        tree.AddChild(steal);

        tree.PrintTree();

    }

    private void Update()
    {
        if (treeStatus != Node.Status.SUCCESS)
        {
            treeStatus = tree.Process();
        }
    }

    public Node.Status HasMoney()
    {
        if (money <= 500)
        {
            return Node.Status.SUCCESS;
        }
        return Node.Status.FAILURE;
    }

    public Node.Status GoToDiamond()
    {
        Node.Status s = GoToLocation(diamond.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            diamond.transform.parent = gameObject.transform;
        }
        return s;
    }

    public Node.Status GoToVan()
    {
        Node.Status s = GoToLocation(van.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            money += 500;
            Destroy(diamond);
        }

        return s;
    }

    public Node.Status GoToBackDoor()
    {
        return GoToDoor(backdoor);
    }

    public Node.Status GoToFrontDoor()
    {
        return GoToDoor(frontdoor);
    }

    public Node.Status GoToDoor(GameObject door)
    {
        Node.Status s = GoToLocation(door.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            //if (!door.GetComponent<Lock>().isLocked)
            //{
                door.SetActive(false);
                return Node.Status.SUCCESS;
            //}
            //return Node.Status.FAILURE;
        }
        else
        {
            return s;
        }
    }

    Node.Status GoToLocation(Vector3 destination)
    {
        float distanceToTarget = Vector3.Distance(destination, transform.position);
        if (state == ActionState.IDLE)
        {
            agent.SetDestination(destination);
            state = ActionState.WORKING;
        }
        else if (Vector3.Distance(agent.pathEndPosition, destination) >= 2)
        {
            state = ActionState.IDLE;
            return Node.Status.FAILURE;
        }
        else if (distanceToTarget < 2)
        {
            state = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }
        return Node.Status.RUNNING;
    }
}
