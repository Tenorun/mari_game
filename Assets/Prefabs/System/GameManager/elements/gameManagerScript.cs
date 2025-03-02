using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public enum mode
    {
        title,
        wait,
        push,
        fall,
        result,
        upgrade,
        config
    }
    public mode current;

    // 업그레이드한 파워
    // 미는 힘
    public int pushLevel = 1;
    // 활공력
    public int glideLevel = 1;
    // 장애물 저항력
    public int resistanceLevel = 1;
    // 수익 레벨
    public int incomeLevel = 1;

    // 낙하 기록
    public int money;
    public float fallRecord;

    // 이미지의 PixelPerUnit 기본 값
    public const int defaultPPU = 32;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        current = mode.wait;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
