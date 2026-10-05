using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Animator playerAnimation;

    private int isWalkingHash = Animator.StringToHash("isWalking");
    private int jumpTriggerHash = Animator.StringToHash("Jump"); // Trigger 사용

    void Start()
    {
        playerAnimation = GetComponent<Animator>();
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // 1. 걷기 처리 (Bool)
        bool isMoving = Keyboard.current.wKey.isPressed ||
                        Keyboard.current.aKey.isPressed ||
                        Keyboard.current.sKey.isPressed ||
                        Keyboard.current.dKey.isPressed;

        playerAnimation.SetBool(isWalkingHash, isMoving);

        // 2. 점프 처리 (Trigger) - else문으로 끄지 않음!
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            playerAnimation.SetTrigger(jumpTriggerHash);
        }
    }
}