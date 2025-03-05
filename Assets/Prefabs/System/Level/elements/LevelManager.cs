using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class LevelManager : MonoBehaviour
{

    // �Ŵ���, �б� ǥ�ñ�
    GameObject manager;
    GameObject indicator;

    // ��� �̹��� ������Ʈ
    GameObject BgImage;
    GameObject statusPannel;
    GameObject upgradeMenu;

    // �÷��̾�
    GameObject player;


    // ������ ������
    public GameObject[] itemPrefabs;
    // �����۵��� �θ� ������Ʈ
    GameObject itemParent;

    // ������ ���� �� ����
    float spawnTime = 0f;
    float obstacleSpawnRate;

    // �б� ��ư�� ���� �ð�
    float pressTime = 0f;

    // ���߿� ���ִ� ����, ���� �Ÿ�
    public float floatHeight = 0;
    public float fallDistance = 0;

    public int loopAmo = 0;

    // �ӵ� ���
    public float travelSpeedMultiplier = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �Ŵ��� ã��(���� ������ ������ �׷� �û���!!!)
        manager = GameObject.FindWithTag("Manager");

        BgImage = GameObject.Find("bgScroll");

        indicator = GameObject.Find("Push Indicator");
        indicator.SetActive(false);

        statusPannel = GameObject.Find("Status Pannel");

        upgradeMenu = GameObject.Find("Upgrade Menu");
        upgradeMenu.SetActive(false);

        itemParent = GameObject.Find("Items");

        player = GameObject.Find("Player");


        // ���� ��ġ �ʱ�ȭ
        player.transform.position = new Vector2(0, 3);
    }
    // Update is called once per frame
    void Update()
    {

        switch (manager.GetComponent<GameManagerScript>().current)
        {
            // ��� ����(wait)
            case GameManagerScript.mode.wait:
                if (Input.GetButtonDown("Submit"))
                {
                    // Ȯ��(�����̽� Ű) ������ �б� ��� ����
                    manager.GetComponent<GameManagerScript>().current = GameManagerScript.mode.push;
                    pressTime = 0f;
                    // �б� ǥ�ñ� ��
                    indicator.SetActive(true);
                }
                else if (Input.GetButtonDown("Upgrade Menu"))
                {
                    // ���׷��̵� �޴� ��ư(e Ű) ������ ���׷��̵� ��� ����
                    manager.GetComponent<GameManagerScript>().current = GameManagerScript.mode.upgrade;
                    upgradeMenu.SetActive(true);
                }
                break;
            // �б� ����(push)
            case GameManagerScript.mode.push:
                // ��ȯ �ð�, ��ư ���� �ð� ���ϱ�
                const float cycleTime = 1.5f;
                pressTime += Time.deltaTime;

                // -|sin(pressTime*(pi/cycleTime))| + 1
                float inputPower = -1 * Mathf.Abs(Mathf.Cos(pressTime * (Mathf.PI / cycleTime))) + 1;
                indicator.GetComponent<pushBarScript>().moveIndicator(inputPower);

                // ��ư ���� �б� �ϼ�
                if (Input.GetButtonUp("Submit"))
                {
                    // ���� ��带 ���� ���� �ٲٰ�, �б� ǥ�ñ⸦ ����
                    manager.GetComponent<GameManagerScript>().current = GameManagerScript.mode.fall;
                    indicator.SetActive(false);

                    // ���� ���
                    floatHeight = 0.5f + inputPower * Mathf.Pow(manager.GetComponent<GameManagerScript>().pushLevel, 1.25f) * 15;

                    // ���� �Ÿ�, �ӵ� ��� �ʱ�ȭ
                    fallDistance = 0f;
                    travelSpeedMultiplier = 1f;

                    loopAmo = 0;
                }
                break;
            case GameManagerScript.mode.fall:
                // ����
                floatHeight -= Time.deltaTime * Mathf.Pow(0.8f,Mathf.Log(manager.GetComponent<GameManagerScript>().glideLevel)) * 2;
                // ������ �Ÿ�
                fallDistance += Time.deltaTime * travelSpeedMultiplier * 10f;

                // ��� �����̱�
                BgImage.GetComponent<BgScroll>().scrollBG(fallDistance);

                // ���� ���̿�, ���߿� ���ִ� �Ÿ� ǥ�� ������Ʈ
                statusPannel.GetComponent<StatusPannelScript>().UpdateFallAndFloatHeight(fallDistance, floatHeight);

                // ��ֹ� ����
                spawnTime += Time.deltaTime;

                if (spawnTime >= 0.01)
                {
                    // ��ֹ� ���� Ȯ�� ���ϱ�
                    // 1.5 * 1.1^(fallDistance/50) Ȯ�� (10m ���� Ȯ�� 1.1�� ����)
                    obstacleSpawnRate = 0.75f * Mathf.Pow(1.1f, fallDistance / 10);

                    for (int i = Mathf.RoundToInt(spawnTime * 100); i > 0; i--)
                    {
                        // ��ֹ� ����
                        if (Random.Range(0.0f, 100.0f) <= obstacleSpawnRate)
                        {
                            // 0 �Ǵ� 1�� ���� �� ����
                            int randomIndex = Random.Range(0, 2);

                            // ��ֹ� ��ġ ����
                            float randomX = Random.Range(-9f, 9f);
                            Vector2 spawnPosition = new Vector2(randomX, -10f);

                            // ��ֹ� ���� �� �θ� ����
                            GameObject newItem = Instantiate(itemPrefabs[randomIndex], spawnPosition, Quaternion.identity);
                            newItem.transform.SetParent(itemParent.transform);
                        }

                        // 0.01�� ���� 0.75% Ȯ���� ������ ����
                        if (Random.Range(0.0f, 100.0f) <= 0.75f)
                        {
                            int randomIndex = Random.Range(2, 4);

                            // ������ ��ġ ����
                            float randomX = Random.Range(-9f, 9f);
                            Vector2 spawnPosition = new Vector2(randomX, -10f);

                            // ��ֹ� ���� �� �θ� ����
                            GameObject newItem = Instantiate(itemPrefabs[randomIndex], spawnPosition, Quaternion.identity);
                            newItem.transform.SetParent(itemParent.transform);
                        }
                    }
                    // ���� Ÿ�̸� �ʱ�ȭ
                    spawnTime = 0f;
                }


                // ��ܿ� ������ ����
                if (floatHeight <= 0)
                {
                    // �ְ��� �����ϸ� ������Ʈ
                    if (manager.GetComponent<GameManagerScript>().fallRecord < fallDistance)
                    {
                        manager.GetComponent<GameManagerScript>().fallRecord = fallDistance;
                    }
                    // ���� �� ����Ͽ� ���ϱ�
                    int rewardMoney = Mathf.RoundToInt(fallDistance * Mathf.Pow(manager.GetComponent<GameManagerScript>().incomeLevel, 1.25f));
                    manager.GetComponent<GameManagerScript>().money += rewardMoney;

                    // ��� ���� �ٲٱ�
                    manager.GetComponent<GameManagerScript>().current = GameManagerScript.mode.result;
                }
                break;
            case GameManagerScript.mode.result:
                // ��� ���� �ٲٱ�
                if (Input.GetButtonDown("Submit"))
                {
                    statusPannel.GetComponent<StatusPannelScript>().UpdateMoneyAndRecord();
                    manager.GetComponent<GameManagerScript>().current = GameManagerScript.mode.wait;

                    // ���� ��ġ �ʱ�ȭ
                    player.transform.position = new Vector2(0, 3);
                }
                break;
        }
    }
}
