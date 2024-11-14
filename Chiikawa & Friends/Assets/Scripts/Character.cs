using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected Rigidbody2D rigidBody;
    protected SpriteRenderer spriteRenderer;
    public int health;
    [SerializeField]protected float moveSpeed;
    protected Vector2 moveDirection;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        CustomStart();
    }

    abstract protected void CustomStart();
    
    abstract protected void Move();
    
    void FixedUpdate() {
        Move();
    }

    public void takeDamage() {
        health = health - 1;
        if(health<=0) {
            Destroy(gameObject);
        }
    }
}
