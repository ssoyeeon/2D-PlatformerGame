using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    public float speed = 5f;
    public GameObject player;

    void Start()
    {
        
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector2(moveHorizontal, 0f);
        player.transform.position += movement * speed * Time.deltaTime;

    }
}
