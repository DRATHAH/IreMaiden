using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Selector : Node
{
    public Selector(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        foreach(Node child in children)
        {
            if (child.Process() == Status.SUCCESS)
            {
                return Status.SUCCESS;
            }
            else if (child.Process() == Status.RUNNING)
            {
                return Status.RUNNING;
            }
        }
        return Status.FAILURE;
    }
}
