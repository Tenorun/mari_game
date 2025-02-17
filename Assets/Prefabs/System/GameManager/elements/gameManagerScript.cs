using UnityEngine;

public class gameManagerScript : MonoBehaviour
{
    public enum mode
    {
        title,
        wait,
        push,
        fall,
        result,
        shop,
        config
    }

    public mode current;

    int currentMoney;
    float fallRecord;

    const int defaultPPU = 32;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        current = mode.fall;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
