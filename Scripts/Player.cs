using UnityEngine;

public class Player : Entity
{

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5;
    private float xInput;
    private bool CanJump = true; 

    protected override void Update()
    {
        base.Update();
        HandleInput();
    }
    protected override void HandleMovement()
    {
         if(CanMove)
        {
            rb.linearVelocity = new Vector2(xInput *MoveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }
    private void TryToJump() {
        if(IsOnGround && CanJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
     private void HandleInput(){
        xInput = Input.GetAxisRaw("Horizontal");
        if(Input.GetKeyDown(KeyCode.Space))
        {
           TryToJump();
        }

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
           HandleAttack();
        }
    }

    public override void ActiveJumpAndMovement(bool enable)
    {
        base.ActiveJumpAndMovement(enable);
        CanJump = enable;
    }

     protected override void Die()
    {
        if (UI.instance != null)
        {
            UI.instance.EnableGameOverUI();
        }
        Destroy(gameObject);
    }
}
