using UnityEngine;

public class Entity_AnimationEvents : MonoBehaviour
{

    private Entity entity;

    private void Awake(){
        entity = GetComponentInParent<Entity>();
    }

    public void DamageTargets(){
        entity.DamageTargets();
    }

    private void DeactivateJumpAndMovement(){
        entity.ActiveJumpAndMovement(false);
    }

    private void ActiveJumpAndMovement(){
        entity.ActiveJumpAndMovement(true);
    }
}
