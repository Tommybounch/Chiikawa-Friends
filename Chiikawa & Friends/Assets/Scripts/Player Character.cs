using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    private float defaultHealth;
    public static PlayerCharacter Instance;
    public Animator animator;

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

    void changeAnimatorParameters(string keyPress){
        Debug.Log(animator.parameters);
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
