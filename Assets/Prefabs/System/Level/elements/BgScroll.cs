using UnityEngine;

public class BgScroll : MonoBehaviour
{
    public GameObject[] BgImages = new GameObject[2];

    // 이미지간의 간격 값
    float spacingValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // spacingValue 값 구하기

        // BgImages[0]의 스프라이트 컴포넌트 가져오기
        SpriteRenderer spriteRenderer = BgImages[0].GetComponent<SpriteRenderer>();

        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            spacingValue = spriteRenderer.sprite.bounds.size.y;
        }
        else
        {
            Debug.LogError("BgImages[0]에 SpriteRenderer 또는 Sprite가 없습니다.");
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
