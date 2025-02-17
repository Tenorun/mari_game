using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float playerSpeed = 10f;

    public float hor;
    public float vert;

    // ������(1:x), ���� �Է� ����
    private float inertia = 3f;
    public Vector2 currDirection;


    private Rigidbody2D body2D;
    GameObject manager;

    // ���� �Է�
    void DirectionInput(Vector2 prevDirection)
    {
        // x,y�� �Է� �ޱ�
        hor = Input.GetAxisRaw("Horizontal");
        vert = Input.GetAxisRaw("Vertical");

        // (���� �Է� + ���� ���� * ������) / (1 + ������)
        currDirection = new Vector2((hor + prevDirection.x * inertia) / (1 + inertia),
            (vert + prevDirection.y * inertia) / (1 + inertia));
    }

    void Start()
    {
        body2D = GetComponent<Rigidbody2D>();
        manager = GameObject.FindWithTag("Manager");
    }

    private void Update()
    {
        if(manager.GetComponent<gameManagerScript>().current == gameManagerScript.mode.fall)
        {
            // �Է¹ޱ�
            DirectionInput(currDirection);
            // �����̱�
            body2D.linearVelocity = new Vector2(currDirection.x * playerSpeed, currDirection.y * playerSpeed);
        }
    }
}
