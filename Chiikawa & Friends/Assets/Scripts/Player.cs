using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private float defaultHealth;
    public static Player Instance;
    public Animator animator;
    private AnimatorStateInfo currState;
    public Inventory inventory;

    

    void Awake() {
        inventory = new Inventory(45);
        
    }

    public void DropItem(Collectable item)
    {
        Vector3 spawnLocation = transform.position;

        float randX = Random.Range(-1f, 1f);
        float randY = Random.Range(-1f, 1f);

        Vector3 spawnOffset = new Vector3(randX, randY, 0f).normalized;

        Collectable droppedItem = Instantiate(item, spawnLocation + spawnOffset, Quaternion.identity);

        droppedItem.rb2d.AddForce(spawnOffset * 2f, ForceMode2D.Impulse);

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
            if(!animator.GetBool("Hurt")){
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
        }
        else{
            moveDirection = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")).normalized;
            if(!animator.GetBool("Hurt")){
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

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);

            if(GameManager.instance.tileManager.IsInteractable(position))
            {
                Debug.Log("Tile is interactable!");
                GameManager.instance.tileManager.SetInteracted(position);
            }
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);

            if(GameManager.instance.tileManager.IsInteractable(position))
            {
                Debug.Log("Tile is Plantable");
                GameManager.instance.tileManager.SetPlanted(position);
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

    public void takeDamage() {
        Debug.Log("Took Damage");
        StartCoroutine(damageAnimation());
        health = health - 1;
        if(health<=0) {
            Destroy(gameObject);
        }
    }

    IEnumerator damageAnimation(){
        changeAnimatorParameters("Hurt");
        yield return new WaitForSeconds(.25f);
        changeAnimatorParameters("NoPress");
    }
}
