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

        // 입력 받기
        if (Keyboard.current.aKey.isPressed) moveX -= 1f;
        if (Keyboard.current.dKey.isPressed) moveX += 1f;
        if (Keyboard.current.wKey.isPressed) moveY += 1f;
        if (Keyboard.current.sKey.isPressed) moveY -= 1f;

        // 점프 타이머 감소
        if (jumpTimer > 0f)
        {
            jumpTimer -= Time.deltaTime;
        }

        // 사다리 구역에 있고, W나 S키를 눌렀다면 등반 모드 시작
        if (isAtLadder && Mathf.Abs(moveY) > 0f)
        {
            isClimbing = true;
        }

        // 물리 및 이동 처리
        if (isClimbing)
        {
            rb.gravityScale = 0f; // 중력 끄기
            // 사다리 이동: 상하좌우 모두 코드로 제어
            rb.linearVelocity = new Vector2(moveX * moveSpeed, moveY * climbSpeed);
        }
        else
        {
            rb.gravityScale = originalGravity; // 중력 복구

            // 1. 좌우 이동 (Y축은 현재 물리 속도 유지)
            rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

            // 2. 점프 (사다리 안 탈 때만 가능하도록 else 블록 안으로 이동)
            if (Keyboard.current.spaceKey.wasPressedThisFrame && jumpTimer <= 0f)
            {
                // Y축 속도만 점프력으로 덮어씌움
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpTimer = 1f; // 타이머 리셋
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