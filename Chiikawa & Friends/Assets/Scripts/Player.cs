using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private float defaultHealth;
    public static Player Instance;
    public Animator animator;
    private AnimatorStateInfo currState;

    void Awake() {
        if(Instance == null){
        Instance = this;}
    }

    protected override void CustomStart() {
        defaultHealth = health;
    }

    protected override void Move() {
        if(moveDirection.magnitude > 0){
            rigidBody.velocity = moveDirection * moveSpeed;
        }
        else{
            rigidBody.velocity = Vector3.zero;
        }
    }

    void Update() {
        if(Input.GetKey(KeyCode.Mouse0)){
            moveDirection = new Vector2(0, 0);
            currState = animator.GetCurrentAnimatorStateInfo(0);
            if(currState.IsName("WalkingRightwards") || currState.IsName("IdleRight")){
                changeAnimatorParameters("AttackRight");
            }
            else if(currState.IsName("WalkingLeftwards") || currState.IsName("IdleLeft")){
                changeAnimatorParameters("AttackLeft");
            }
            else if(currState.IsName("WalkingDownwards") || currState.IsName("IdleDown")){
                changeAnimatorParameters("AttackDown");
            }
            else if(currState.IsName("WalkingUpwards") || currState.IsName("IdleUp")){
                changeAnimatorParameters("AttackUp");
            }
        }
        else{
            moveDirection = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")).normalized;
            if(Input.GetKey(KeyCode.D)){
                changeAnimatorParameters("RightPress");
            }
            else if(Input.GetKey(KeyCode.A)){
                changeAnimatorParameters("LeftPress");
            }
            else if(Input.GetKey(KeyCode.S)){
                changeAnimatorParameters("DownPress");
            }
            else if(Input.GetKey(KeyCode.W)){
                changeAnimatorParameters("UpPress");
            }
            else{
                changeAnimatorParameters("NoPress");
            }
        }
    }

    void changeAnimatorParameters(string keyPress){
        foreach(AnimatorControllerParameter param in animator.parameters){
            if(param.name != keyPress){
                animator.SetBool(param.name, false);
            }
            else{
                animator.SetBool(param.name, true);
            }
        }
    }
}
