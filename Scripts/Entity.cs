using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour 
{
    protected Rigidbody2D rb;
    protected Animator anim;
    protected Collider2D col;
    protected SpriteRenderer sr;

    [Header("Health")]
    [SerializeField] private int MaxHealth = 1;
    [SerializeField] private int CurrentHealth;
    [SerializeField] protected Material DamageMaterial; 
    [SerializeField] protected float DamageFeedbackDuration = 0.2f;

    private Coroutine DamageFeedbackCoroutine;

    [Header("AttackDetails")]
    [SerializeField] protected float AttackRadius;
    [SerializeField] protected Transform AttackPoint;
    [SerializeField] protected LayerMask WhatIsTarget;

    [Header("Movements")]
    [SerializeField] protected float MoveSpeed = 3f;
   

    [Header("Collisions")]
    [SerializeField] private float GroundDistance;
    [SerializeField] private LayerMask GroundLayer;

    protected bool FacingRight = true;
    protected bool CanMove = true; 
    protected int FacingDirection = 1;
    protected bool IsOnGround;

    protected virtual void Awake() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        CurrentHealth = MaxHealth;
        col = GetComponent<Collider2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }
    
    protected virtual void Update()
    {
        HandleMovement();
        HandleAnimations();
        HandleFlip();
        HandleCollision();
    }

    public void DamageTargets(){
        if (AttackPoint == null) return; 
        
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRadius, WhatIsTarget); 
        foreach (Collider2D enemy in enemyColliders)
        {
            Entity entityTarget = enemy.GetComponent<Entity>();
            if (entityTarget != null)
            {
                entityTarget.TakeDamage();
            }
        }
    }

    protected virtual void TakeDamage()
    {
        CurrentHealth -= 1;
        FeedbackDamage(); 
        if(CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void FeedbackDamage()
    {
        if(DamageFeedbackCoroutine != null)
        {
            StopCoroutine(DamageFeedbackCoroutine);
        }
        StartCoroutine(DamageFeedbackCo());
    }

    private IEnumerator DamageFeedbackCo()
    {
        Material OriginalMat = sr.material;
        sr.material = DamageMaterial;
        yield return new WaitForSeconds(DamageFeedbackDuration);
        sr.material = OriginalMat;
    }

    protected virtual void Die()
    {
        anim.enabled = false;
        col.enabled = false;
        rb.gravityScale = 12;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15); 
        Destroy(gameObject, 3);
    }

    public virtual void ActiveJumpAndMovement(bool enable){
        CanMove = enable;
    }

    protected void HandleAnimations(){
        anim.SetBool("IsOnGround",IsOnGround);
        anim.SetFloat("X-Axis", rb.linearVelocity.x);
        anim.SetFloat("Y-Axis", rb.linearVelocity.y);
    }



    protected virtual void HandleAttack(){
        if(IsOnGround)
        {
            anim.SetTrigger("attack");
        }
    }


    protected virtual void HandleMovement(){
    
    }
    
    protected virtual void HandleCollision(){
        IsOnGround = Physics2D.Raycast(transform.position, Vector2.down, GroundDistance, GroundLayer);
    }

    protected virtual void HandleFlip(){
        if(rb.linearVelocity.x > 0 && FacingRight == false)
            Flip();
        else if (rb.linearVelocity.x < 0 && FacingRight == true)
            Flip();
    }

    public void Flip(){
        transform.Rotate(0,180,0);
        FacingRight = !FacingRight;
        FacingDirection = FacingDirection * -1;
    }

    private void OnDrawGizmos(){
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -GroundDistance));
        if (AttackPoint != null)
        {
            Gizmos.DrawWireSphere(AttackPoint.position, AttackRadius);
        }
    }

}