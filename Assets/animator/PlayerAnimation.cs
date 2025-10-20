using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;  

    private Vector2 moveInput;
    private bool isRunning;

    void Update()
    {
        if (animator == null) return;

        bool isMoving = moveInput.magnitude > 0.1f;
        isRunning = isMoving && Keyboard.current.leftShiftKey.isPressed;

        animator.SetBool("isWalking", isMoving && !isRunning);
        animator.SetBool("isRunning", isRunning);
    }

    
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}
