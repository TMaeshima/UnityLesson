using System;
using System.Collections.Generic;

//<T>・・・ジェネリック(テンプレート)クラスTの部分が
//使用者の好きな型に変えれる
public class StateMachine<T>
{
    private class State
    {
        //readonly・・・再代入不可な変数修飾子
        //、関数の呼び出しやメンバ変数への代入は可能.
        private readonly Action mEnterAction;  
        // 開始時に呼び出されるデリゲート(関数)
        private readonly Action mUpdateAction; 
        // 更新時に呼び出されるデリゲート(関数)
        private readonly Action mExitAction;   
        // 終了時に呼び出されるデリゲート(関数)

        public State(Action updateAct = null
                    ,Action enterAct = null
                    ,Action exitAct = null)
        {
            // ??演算子・・・代入する変数の中身がnullの場合??の
            //右側にあるものが代入され.
            // nullを許容したくないときに使える.
            mUpdateAction  = updateAct ?? delegate { };
            mEnterAction   = enterAct  ?? delegate { };
            mExitAction    = exitAct   ?? delegate { };
        }
        public void Enter()
        {
            mEnterAction();
        }

        public void Update()
        {
            mUpdateAction();
        }

        public void Exit()
        {
            mExitAction();
        }
    }

    private Dictionary<T, State> mStateDictionary
     = new Dictionary<T, State>();
    private State mCurrentState;                                                

    public void Add(T key, Action updateAct = null
                        , Action enterAct = null
                        , Action exitAct = null)
    {
        mStateDictionary.Add(key, new State(updateAct
                                            , enterAct
                                            , exitAct));
    }

    public void ChangeState(T key)
    {
        //?演算子・・・変数の中身がnullでなければ
        //変数にアクセスする.
        mCurrentState?.Exit();
        mCurrentState = mStateDictionary?[key];
        mCurrentState?.Enter();
    }

    public void Update()
    {
        if (mCurrentState == null)
        {
            return;
        }
        mCurrentState.Update();
    }

    public void Clear()
    {
        mStateDictionary.Clear();
        mCurrentState = null;
    }
}