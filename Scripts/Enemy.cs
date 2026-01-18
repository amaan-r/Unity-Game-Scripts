using UnityEngine;

public class Enemy : Entity
{ 
    private bool PlayerDetected;
    private float actualMoveSpeed;

    protected override void Awake()
    {
        base.Awake();
        
        float difficultyMultiplier = Start_Screen.instance != null ? Start_Screen.DifficultyMultiplier : 1f;
        actualMoveSpeed = MoveSpeed * difficultyMultiplier;
    }

    protected override void Update()
    {
        base.Update();
        HandleAttack();
    }

    protected override void HandleAttack()
    {
        if(PlayerDetected)
        {
            anim.SetTrigger("attack");
        }
    }

    protected override void HandleMovement()
    {
        if (CanMove)
            rb.linearVelocity = new Vector2(FacingDirection * actualMoveSpeed, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();
        PlayerDetected = Physics2D.OverlapCircle(AttackPoint.position, AttackRadius, WhatIsTarget);
    }
}