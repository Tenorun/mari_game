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

    // ���׷��̵��� �Ŀ�
    // �̴� ��
    public int pushLevel = 1;
    // Ȱ����
    public int glideLevel = 1;
    // ��ֹ� ���׷�
    public int resistanceLevel = 1;
    // ���� ����
    public int incomeLevel = 1;

    // ���� ���
    public int money;
    public float fallRecord;

    // �̹����� PixelPerUnit �⺻ ��
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
