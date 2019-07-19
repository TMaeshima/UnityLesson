using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineTest : MonoBehaviour
{
    enum State : int
    {
        A,
        B,
        C,
    }

    StateMachine<State> stateMachine 
    = new StateMachine<State>();
    [SerializeField] float time;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine.Add(State.A, 
        //Updateラムダ
        ()=> 
        {
            time += Time.deltaTime;
            if (time > 3.0f)
            {
                stateMachine.ChangeState(State.B);
            }
        },
        //Enterラムダ
        ()=>
        {
            Debug.LogFormat($"EnterState = {State.A}");
            //Debug.LogFormat("EnterState = {0}",State.A);
            //printf("EnterState = %d",State.A);
        },
        //Exitラムダ
        ()=>
        {
            Debug.LogFormat($"ExitState = {State.A}");
        });

        stateMachine.Add(State.B, 
        //Updateラムダ
        () =>
        {
            time += Time.deltaTime;
            if (time > 5.0f)
            {
                stateMachine.ChangeState(State.C);
            }
        });

        stateMachine.Add(State.C,
        //Updateラムダ
        () =>
        {
            time += Time.deltaTime;
            if (time > 2.0f)
            {
                stateMachine.ChangeState(State.A);
            }
        }
        //Enterラムダ
        ,() =>
        {
            Debug.LogFormat($"EnterState = {State.C}");
            time = 0.0f;
        },
        //Exitラムダ
        () =>
        {
            Debug.LogFormat($"ExitState = {State.C}");
            time = 0.0f;
        });
        stateMachine.ChangeState(State.A);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }
}
