using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIDecision : MonoBehaviour
{
    public bool IsReverse = false;
    public virtual void SetUp(Transform parnet)
    {

    }
    public abstract bool MakeADecision();
}
