using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    GameObject player;
    GameObject manager;

    // Item Varient
    // 1: Speed Down
    // 2: Float Height Down
    // 3: Speed Up
    // 4: Float Height Up
    public int ItemVarientNum;
    float speed = 5.0f; // 아이템이 위로 이동하는 속도

    float KillTime = 0f;

    void Start()
    {
        player = GameObject.Find("Player");
        manager = GameObject.FindWithTag("Manager");
    }

    void Update()
    {
        // 위쪽으로 이동
        transform.position += Vector3.up * speed * Time.deltaTime;

        KillTime += Time.deltaTime;
        if(KillTime >= 10f || manager.GetComponent<GameManagerScript>().current != GameManagerScript.mode.fall)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            Destroy(gameObject); // Player에 닿으면 아이템 제거
        }
    }
}
