using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AgentController : MonoBehaviour
{
    private Dictionary<StateType, IState> _stateDictionary = null;  //������ �ִ� ���µ� ����
    private IState _currentState; //���� ���� ����

    public StateType CurrentState; 

    public Gun currentGun;

    public Camera overlayCam;

    public PlayerMovement PlayerMovementCompo { get; private set; }
    public PlayerHealth PlayerHealthCompo {get; private set; }
    public bool IsDead;

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
        if(GameManager.Instance.SceneEnum == SceneTypes.InGame)
        {
            PlayerHealthCompo = GetComponent<PlayerHealth>();
            PlayerHealthCompo.Init(true);
        }
        var cameraData = Define.MainCam.GetUniversalAdditionalCameraData();
        cameraData.cameraStack.Add(overlayCam);
    }

    private void Start()
    {
        if(GameManager.Instance.SceneEnum == SceneTypes.InGame)
        {
            currentGun.Init(transform);
        }
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
        _currentState?.UpdateState();
    }
}
