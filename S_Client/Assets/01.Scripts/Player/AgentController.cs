using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    private Dictionary<StateType, IState> _stateDictionary = null;  //������ �ִ� ���µ� ����
    private IState _currentState; //���� ���� ����

    public String CurrentState;

    public PlayerMovement PlayerMovementCompo { get; private set; }
    public bool IsDead{get; private set;}

    public void SetDead()
    {
        IsDead = true;
    }

    private void Awake()
    {
        _stateDictionary = new Dictionary<StateType, IState>();
        Transform stateTrm = transform.Find("States");


        foreach( StateType state in Enum.GetValues(typeof (StateType)) )
        {
            IState stateScript = stateTrm.GetComponent($"{state}State") as IState;
            if(stateScript == null)
            {
                Debug.LogError($"There is no script : {state}");
                return;
            }
            stateScript.SetUp(transform);
            _stateDictionary.Add(state, stateScript);
        }

        PlayerMovementCompo = GetComponent<PlayerMovement>();

    }


    private void Start()
    {
        ChangeState(StateType.Normal);
    }

    public void ChangeState(StateType type)
    {
        _currentState?.OnExitState(); //���� ���� ������
        _currentState = _stateDictionary[type];
        _currentState?.OnEnterState(); //�������� ����
    }

    private void Update()
    {
        if(IsDead) return;
        _currentState?.UpdateState();
    }
}
