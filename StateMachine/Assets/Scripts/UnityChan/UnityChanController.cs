using UnityEngine;

//AddComponentしたときにGameObjectに
//RequireComponentで指定したComponentがなければ
//AddComponentしてくれる命令.
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]

public class UnityChanController : MonoBehaviour
{

    enum AnimationState :int
    {
        Idel,
        Run,
        Jump
    }

    StateMachine<AnimationState>    stateMachine        = new StateMachine<AnimationState>();
    Animator                        animator            = null;
    AnimatorBehaviour               animatorBehavior    = null;
    Rigidbody                       rigidbody           = null;
    Vector3                         velocity            = Vector3.zero;

    [SerializeField] float rotateSpeed = 1.0f;
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] float jumpPower = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        InitializeComponent();
        InitializedState();
    }

    void InitializedState()
    {
        stateMachine.Add(AnimationState.Idel
            , IdelUpdate
            , IdelInitialize
            );
        stateMachine.Add(AnimationState.Run
            , RunUpdate
            , RunInitialize
            );
        stateMachine.Add(AnimationState.Jump
           , JumpUpdate
           , JumpInitialize
           , JumpEnd
           );
        stateMachine.ChangeState(AnimationState.Idel);
    }

    void InitializeComponent()
    {
        animator            = GetComponent<Animator>();
        rigidbody           = GetComponent<Rigidbody>();
        animatorBehavior    
        = animator.GetBehaviour<AnimatorBehaviour>();
    }

    void IdelUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            stateMachine.ChangeState(AnimationState.Run);
        }
        JumpChange();
        DirectionChange();
    }

    void IdelInitialize()
    {
        animator.CrossFadeInFixedTime("Idle", 0.0f);
    }

    void Move(float aMoveSpeed)
    {
        velocity = transform.TransformDirection(new Vector3(0,0,aMoveSpeed));
        var position = transform.position + velocity;
        rigidbody.MovePosition(position);
    }

    void RunInitialize()
    {
        animator.CrossFadeInFixedTime("Run",0.0f);
    }

    void RunUpdate()
    {
        if (Input.GetKey(KeyCode.W) == false)
        {
            stateMachine.ChangeState(AnimationState.Idel);
            return;
        }
        JumpChange();
        DirectionChange();
        Move(moveSpeed);
    }

    void JumpInitialize()
    {
        animator.CrossFadeInFixedTime("Jump",0.0f);
        rigidbody.AddForce(0.0f,jumpPower,0.0f,ForceMode.Impulse);
        animatorBehavior.EndCallBack = ()=>{stateMachine.ChangeState(AnimationState.Idel);};
    }

    void JumpUpdate()
    {
        /*
        var animeState = 
        animator.GetCurrentAnimatorStateInfo(0);
        if(animeState.normalizedTime > 1.0f)
        {
            stateMachine.ChangeState(AnimationState.Idel);
        }
         */

         
        if(Input.GetKey(KeyCode.W))
        {
            if(animatorBehavior.NormalizedTime > 0.65f)
            {
                stateMachine.ChangeState(AnimationState.Run);
            }
            else
            {
                Move(moveSpeed / 2);                
            }
        }
    }

    void JumpEnd()
    {
        animatorBehavior.EndCallBack = ()=>{};
    }


    void JumpChange()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            stateMachine.ChangeState(AnimationState.Jump);
        }
    }

    void DirectionChange()
    {
        float rot = 0.0f;
        if(Input.GetKey(KeyCode.A))
        {
            rot -= rotateSpeed;
        }

        if(Input.GetKey(KeyCode.D))
        {
            rot += rotateSpeed;
        }
        transform.Rotate(0.0f,rot,0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
        var animeState = 
        animator.GetCurrentAnimatorStateInfo(0);
        //Unityのデフォルトで取得できるアニメーション再生時間
        Debug.LogFormat(
            $"before normalizeTime{animeState.normalizedTime}");
        //新たに作り直して0~1の間で取得できるようにした
        //アニメーション再生時間
        Debug.LogFormat(
            $"after normalizeTime = {animatorBehavior.NormalizedTime}");
        if(animatorBehavior.NormalizedTime > 1.0f)
        {
            animatorBehavior.ResetTime();
        }
    }
}
