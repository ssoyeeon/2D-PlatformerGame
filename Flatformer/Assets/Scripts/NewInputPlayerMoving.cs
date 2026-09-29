using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewInputPlayerMoving : MonoBehaviour
{
    public float moveSpeed = 5f;        // 이동 속도
    public float jumpForce = 15f;       // 위로 올려주는 힘 (수치를 조금 높여야 잘 뜁니다)
    public float climbSpeed = 3f;       // 사다리 타는 속도 

    private Rigidbody2D rb;

    public bool isAtLadder;             // 사다리에 닿아있는 여부
    public bool isClimbing;             // 사다리를 올라가고 있는지 여부 

    public float jumpTimer = 1f;
    private float originalGravity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
    }

    private void Update()
    {
        if (Keyboard.current == null) return;

        float moveX = 0f;
        float moveY = 0f;

        if (Keyboard.current.aKey.isPressed) moveX -= 1f;
        if (Keyboard.current.dKey.isPressed) moveX += 1f;
        if (Keyboard.current.wKey.isPressed) moveY += 1f;
        if (Keyboard.current.sKey.isPressed) moveY -= 1f;

        if (jumpTimer > 0f)
        {
            jumpTimer -= Time.deltaTime;
        }

        if (isAtLadder && Mathf.Abs(moveY) > 0f)
        {
            isClimbing = true;
        }

        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(moveX * moveSpeed, moveY * climbSpeed);
        }
        else
        {
            rb.gravityScale = originalGravity; 

            rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

            if (Keyboard.current.spaceKey.wasPressedThisFrame && jumpTimer <= 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpTimer = 1f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isAtLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isAtLadder = false;
            isClimbing = false;
        }
    }
}