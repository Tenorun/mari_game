using UnityEngine;

public class BgScroll : MonoBehaviour
{
    public GameObject[] BgImages = new GameObject[2];

    // �̹������� ���� ��
    float spacingValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // spacingValue �� ���ϱ�

        // BgImages[0]�� ��������Ʈ ������Ʈ ��������
        SpriteRenderer spriteRenderer = BgImages[0].GetComponent<SpriteRenderer>();

        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            spacingValue = spriteRenderer.sprite.bounds.size.y;
        }
        else
        {
            Debug.LogError("BgImages[0]�� SpriteRenderer �Ǵ� Sprite�� �����ϴ�.");
        }

        BgImages[0].transform.position = new Vector2(0, 0);
        BgImages[1].transform.position = new Vector2(0, -spacingValue);
    }

    public void scrollBG(float travelDistance)
    {
        float moveAmo = travelDistance * 1.5f % spacingValue;

        BgImages[0].transform.position = new Vector2(0, moveAmo);
        BgImages[1].transform.position = new Vector2(0, moveAmo - spacingValue);
    }
}
