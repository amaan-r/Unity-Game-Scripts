using UnityEngine;

public class FoodBasket : Entity 
{
    private Transform player;
    
    protected override void Awake()
    {
        base.Awake();
        player = FindFirstObjectByType<Player>()?.transform;
        
        if (player == null)
        {
            Debug.LogWarning("Player not found in Awake, will retry in Update");
        }
    }
    
    protected override void Update()
    {
        if (player == null)
        {
            player = FindFirstObjectByType<Player>()?.transform;
        }
        
        HandleFlip();
    }

    protected override void HandleFlip()
    {
        if (player == null)
        {
            return;
        }
        
        if (player.position.x > transform.position.x && FacingRight == false)
            Flip();
        else if (player.position.x < transform.position.x && FacingRight == true)
            Flip();
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