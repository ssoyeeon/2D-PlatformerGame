using UnityEngine;
using UnityEngine.InputSystem;

// ==========================================
// 1. Enum (열거형): 무기 종류 정의 (0, 1, 2)
// ==========================================
public enum WeaponType
{
    Pistol,  // 0
    Rifle,   // 1
    Shotgun  // 2
}

public class FPSWeaponSystem : MonoBehaviour
{
    // [Enum 변수] 현재 장착 무기
    public WeaponType currentWeapon = WeaponType.Pistol;

    // [배열] 각 무기 타입별 최대 탄약 및 현재 탄약 (0: 권총, 1: 소총, 2: 샷건)
    public int[] maxAmmoCounts = { 10, 30, 8 };
    public int[] currentAmmoCounts = { 10, 30, 8 };

    void Update()
    {
        // 키보드나 마우스 연결이 끊겨있는 경우를 대비한 예외 처리
        if (Keyboard.current == null || Mouse.current == null) return;

        // --------------------------------------------------
        // [기능 1] New Input System: 숫자키(1, 2, 3) 누르면 무기 변경
        // --------------------------------------------------
        if (Keyboard.current.digit1Key.wasPressedThisFrame) currentWeapon = WeaponType.Pistol;
        if (Keyboard.current.digit2Key.wasPressedThisFrame) currentWeapon = WeaponType.Rifle;
        if (Keyboard.current.digit3Key.wasPressedThisFrame) currentWeapon = WeaponType.Shotgun;

        // --------------------------------------------------
        // [기능 2] New Input System: 마우스 휠(위/아래) 무기 순환 교체
        // --------------------------------------------------
        float scrollY = Mouse.current.scroll.y.ReadValue();

        if (scrollY != 0f)
        {
            int totalWeapons = 3;
            int currentIndex = (int)currentWeapon;

            if (scrollY > 0f) // 휠 위로 올려서 다음 무기
            {
                currentIndex = (currentIndex + 1) % totalWeapons;
            }
            else if (scrollY < 0f) // 휠 아래로 내려서 이전 무기
            {
                currentIndex = (currentIndex - 1 + totalWeapons) % totalWeapons;
            }

            currentWeapon = (WeaponType)currentIndex;
            Debug.Log($"[무기 교체] 현재 무기: {currentWeapon}");
        }

        // --------------------------------------------------
        // [기능 3] New Input System: 발사(마우스 좌클릭) 및 재장전(R키)
        // --------------------------------------------------
        if (Mouse.current.leftButton.wasPressedThisFrame) FireWeapon();
        if (Keyboard.current.rKey.wasPressedThisFrame) ReloadAllWeapons();
    }

    // 무기 발사 로직
    void FireWeapon()
    {
        // Enum을 int로 변환하여 탄약 배열의 인덱스로 활용
        int weaponIndex = (int)currentWeapon;

        // [if문] 탄약 검사
        if (currentAmmoCounts[weaponIndex] <= 0)
        {
            Debug.Log($"{currentWeapon}의 탄약이 없습니다! R키를 누르세요.");
            return;
        }

        currentAmmoCounts[weaponIndex]--; // 탄약 1발 차감

        // [switch문] 현재 무기 타입에 따른 연출 분기
        switch (currentWeapon)
        {
            case WeaponType.Pistol:
                Debug.Log($"[권총] 탕! (남은 탄약: {currentAmmoCounts[weaponIndex]}발)");
                break;

            case WeaponType.Rifle:
                Debug.Log($"[소총] 두두두! (남은 탄약: {currentAmmoCounts[weaponIndex]}발)");
                break;

            case WeaponType.Shotgun:
                Debug.Log($"[샷건] 쿠쾅! (남은 탄약: {currentAmmoCounts[weaponIndex]}발)");
                break;
        }
    }

    // 전체 재장전 로직
    void ReloadAllWeapons()
    {
        Debug.Log("=== 전체 무기 재장전 시작 ===");

        // [for문] 탄약 배열을 순회하며 최대 수치로 복구
        for (int i = 0; i < currentAmmoCounts.Length; i++)
        {
            currentAmmoCounts[i] = maxAmmoCounts[i];
        }

        Debug.Log("모든 무기 재장전 완료!");
    }
}