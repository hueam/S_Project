using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    AgentController agentController;
    private int maxHP = 50;
    public int MaxHP{
        get=>maxHP;
        set{
            maxHP = value;
            ((InGameManager)GameManager.Instance.SceneController).SetHpText(maxHP, currentHP);
        }
    }
    private int currentHP;
    public int CurrentHP{
        get=>currentHP;
        set{
            currentHP = value;
            ((InGameManager)GameManager.Instance.SceneController).SetHpText(maxHP, currentHP);
        }
    }

    public void Init(bool client) {
        CurrentHP = maxHP;
        if(client == true)
        {
           agentController = GetComponent<AgentController>();
        }
    }
    public void HitDamage(int damage)
    {
        Debug.Log($"{damage}아야!");
        CurrentHP -= damage;
        if(CurrentHP <= 0)
        {
            Client client = ((Client)GameManager.Instance.Managers[Managers.Client]);
            client.SendData((int)Events.InGame,(int)InGameTypes.Die,client.socket.Id);
            CurrentHP = 0;
            agentController.ChangeState(StateType.Die);
        }
    }
}
