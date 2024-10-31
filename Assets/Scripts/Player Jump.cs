using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [Header("Salto")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float maxJumpForce = 10f; 
    [SerializeField] private float groundDistance = 0.6f; 
    [SerializeField] private LayerMask groundLayer; 

    private bool isGround; 
    private Rigidbody2D myRBD; 
    private float jumpCharge; 

    private void Awake()
    {
        myRBD = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        DrawRaycast();
        HandleJumpCharge();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && isGround)
        {
            jumpCharge = jumpForce; // Aca salto con fuerza normal
            Jump();
        }
        else if (context.performed && isGround)
        {
        }
        else if (context.canceled && isGround)
        {
            Jump();// Aca salto con la fuerza maxima acumulada
        }
    }

    private void Jump()
    {
        myRBD.AddForce(Vector2.up * jumpCharge, ForceMode2D.Impulse);
        jumpCharge = 0;
    }

    private void HandleJumpCharge()
    {
        if (isGround && Keyboard.current.spaceKey.isPressed)
        {
            jumpCharge += Time.deltaTime * (maxJumpForce - jumpForce); // Aumento la carga del salto 
            jumpCharge = Mathf.Min(jumpCharge, maxJumpForce); // Limito la fuerza del salto largo
        }
    }

    private void DrawRaycast()
    {
        isGround = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundDistance);
    }
}
