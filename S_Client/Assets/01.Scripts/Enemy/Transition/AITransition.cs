using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITransition : MonoBehaviour
{
    public List<AIDecision> decisions = new List<AIDecision>();
    public void SetUp(Transform parent)
    {
        decisions.ForEach(d => d.SetUp(parent));
    }
    public bool CheckTransition()
    {
        bool result = false;
        foreach(AIDecision decision in decisions)
        {
            result=decision.MakeADecision();
            if(decision.IsReverse)
                result = !result;
            if(result == false)
                break;
        }
        return result;
    }
}
