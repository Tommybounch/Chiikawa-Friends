using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Animator playerAnimator;
    public Animator weaponAnimator;
    public AnimatorStateInfo currState;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        currState = playerAnimator.GetCurrentAnimatorStateInfo(0);
        if(currState.IsName("AttackRight")){
            changeAnimatorParameters("Right");
        }
        else if(currState.IsName("AttackLeft")){
            changeAnimatorParameters("Left");
        }
        else if(currState.IsName("AttackDown")){
            changeAnimatorParameters("Down");
        }
        else if(currState.IsName("AttackUp")){
            changeAnimatorParameters("Up");
        }
        else{
            changeAnimatorParameters("");
        }
    }

    void changeAnimatorParameters(string keyPress){
        foreach(AnimatorControllerParameter param in weaponAnimator.parameters){
            if(param.name != keyPress){
                weaponAnimator.SetBool(param.name, false);
            }
            else{
                weaponAnimator.SetBool(param.name, true);
            }
        }
    }
}
