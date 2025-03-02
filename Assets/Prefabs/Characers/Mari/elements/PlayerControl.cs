using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float playerSpeed = 10f;

    public float hor;
    public float vert;

    // 관성비(1:x), 현재 입력 방향
    private float inertia = 7.5f;
    public Vector2 currDirection;


    private Rigidbody2D body2D;
    GameObject manager;

    // 방향 입력
    void DirectionInput(Vector2 prevDirection)
    {
        // x,y축 입력 받기
        hor = Input.GetAxisRaw("Horizontal");
        vert = Input.GetAxisRaw("Vertical");

        // (현재 입력 + 이전 방향 * 관성비) / (1 + 관성비)
        currDirection = new Vector2((hor + prevDirection.x * inertia) / (1 + inertia),
            (vert + prevDirection.y * inertia) / (1 + inertia));
    }

    void StopMove()
    {
        hor = 0f;
        vert = 0f;

        currDirection = Vector2.zero;
        body2D.linearVelocity = new Vector2(0,0);
    }

    void Start()
    {
        body2D = GetComponent<Rigidbody2D>();
        manager = GameObject.FindWithTag("Manager");
    }

    private void Update()
    {
        if (manager.GetComponent<GameManagerScript>().current == GameManagerScript.mode.fall)
        {
            // 입력받기
            DirectionInput(currDirection);
            // 움직이기
            body2D.linearVelocity = new Vector2(currDirection.x * playerSpeed, currDirection.y * playerSpeed);
        }
        else
        {
            StopMove();
        }
    }
}
