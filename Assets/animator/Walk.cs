using UnityEngine;
using UnityEngine.InputSystem;

public class Walk : MonoBehaviour
{
    public Animator animator; 

    private Vector2 moveInput;
    private bool isMoving;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        isMoving = moveInput.magnitude > 0f;
        animator.SetBool("isWalking", isMoving);
    }
}
