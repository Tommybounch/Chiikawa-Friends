using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    protected Vector3 targetPosition;
    protected enum State{Roam, Chase};
    [SerializeField] protected float chaseDist,roamDist;
    protected State currentState;
    public Animator animator;
    public AnimatorStateInfo currState;
    float x,y;

    protected override void CustomStart() {
        y = Camera.main.orthographicSize;
        x = Camera.main.aspect*y;
        currentState = State.Roam;
        targetPosition = (Vector2) transform.position
        + new Vector2(Random.Range(-roamDist, roamDist), Random.Range(-roamDist, roamDist));
    }

    void Update()
    {
        if(GameObject.FindWithTag("Player") != null){
            if (currentState == State.Roam){
                if(getDistance(transform.position, targetPosition)<1f) {
                    targetPosition = (Vector2) transform.position + new
                    Vector2(Random.Range(-roamDist, roamDist), Random.Range(-roamDist, roamDist));
                }
                
                if(getDistance(transform.position, Player.Instance.transform.position) < chaseDist) {
                    currentState = State.Chase;
                }
            }
            else if(currentState == State.Chase) {
                targetPosition = Player.Instance.transform.position;
                if(getDistance(transform.position, Player.Instance.transform.position) > chaseDist*1.2f) {
                    currentState = State.Roam;
                }
            }
            targetPosition = getCoords(targetPosition);
            moveDirection = -getDirection(transform.position, targetPosition).normalized;
        }
    }
    
    protected override void Move() {
        rigidBody.rotation = 0;
        currState = animator.GetCurrentAnimatorStateInfo(0); 
        if(currState.IsName("Red Jump Start-up - Animation")){
            rigidBody.velocity = Vector3.zero;
        }
        if(currState.IsName("Dead")){
            rigidBody.velocity = Vector3.zero;
        }
        else if(moveDirection.magnitude > 0) {
            rigidBody.velocity = moveDirection * moveSpeed;
        }
        else {
            rigidBody.velocity = Vector3.zero;
        }
        transform.up += ((Vector3)moveDirection-transform.up)/5;
        transform.rotation = Quaternion.identity;

        if (Player.Instance.transform.position.x < transform.position.x) {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public float getDistance(Vector3 from, Vector3 to){
        Vector3 coords = getCoords(to);
        float magnitude = (x+y)*2;
        for(int _x = -1; _x<2; _x++){
            for(int _y = -1; _y<2; _y++){
                if(Vector3.Distance(from, coords + new Vector3(_x*x*2, _y*y*2, 0)) < magnitude){
                    magnitude = Vector3.Distance(from, coords+ new Vector3(_x*x*2, _y*y*2, 0));
                }
            }
        }
        return magnitude;
    }
    public Vector3 getDirection(Vector3 from, Vector3 to){
        Vector3 coords = getCoords(to);
        Vector3 result = Vector3.zero;
        float magnitude = (x+y)*2;
        for(int _x = -1; _x<2; _x++){
            for(int _y = -1; _y<2; _y++){
                if(Vector3.Distance(from, coords + new Vector3(_x*x*2, _y*y*2, 0)) < magnitude){
                    magnitude = Vector3.Distance(from,coords+ new Vector3(_x*x*2, _y*y*2, 0));
                    result = coords + new Vector3(_x*x*2, _y*y*2, 0);
                }
            }
        }
        return from - result;
    }

    public Vector3 getCoords(Vector3 vect){
        float _x = vect.x;
        float _y = vect.y;
        while(_x>x || _x<-x){
            _x = (_x>x) ? _x-x*2 : _x+x*2;
        }
        while(_y>y || _y<-y){
            _y = (_y>y) ? _y-y*2 : _y+y*2;
        }
        return new Vector2(_x, _y);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player Weapon")){
            this.takeDamage();
        }
        else if(other.CompareTag("Player")){
            other.GetComponent<Player>().takeDamage();
        }
    }
    public void takeDamage() {
        health = health - 1;
        if(health<=0) {
            StartCoroutine(deathAnimation());
        }
        else{
            StartCoroutine(damageAnimation());
        }
    }

    IEnumerator damageAnimation(){
        animator.SetBool("Hurt", true);
        yield return new WaitForSeconds(1);
        animator.SetBool("Hurt", false);
    }

    IEnumerator deathAnimation(){
        animator.SetBool("Dead", true);
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }
}
