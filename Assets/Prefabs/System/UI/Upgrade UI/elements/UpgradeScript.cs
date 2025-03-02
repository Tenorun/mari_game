using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable] // Inspector���� ���̵��� ����
public class ButtonData
{
    public GameObject btnObject;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI priceText;

    public string name;
    public int price;
}

public class UpgradeScript : MonoBehaviour
{
    GameObject manager;
    GameObject statusPannel;

    public ButtonData[] btns = new ButtonData[4];

    int currentSelect = 0;
    private void UpdateBtn(int btnNum)
    {
        // ��ư �̸� ������Ʈ
        btns[btnNum].nameText.text = btns[btnNum].name;

        int levelNum = 0;

        // ���� �� ���
        switch (btnNum)
        {
            case 0:
                levelNum = manager.GetComponent<GameManagerScript>().pushLevel;
                break;
            case 1:
                levelNum = manager.GetComponent<GameManagerScript>().glideLevel;
                break;
            case 2:
                levelNum = manager.GetComponent<GameManagerScript>().resistanceLevel;
                break;
            case 3:
                levelNum = manager.GetComponent<GameManagerScript>().incomeLevel;
                break;
            default:
                Debug.LogError($"btnNum: {btnNum}�� �ش��ϴ� ��ư�� �����ϴ�.");
                break;
        }

        // ���� ǥ��, ����, ���� ǥ�� ������Ʈ
        btns[btnNum].levelText.text = $"Lv.{levelNum}";
        btns[btnNum].price = Mathf.RoundToInt(Mathf.Pow(2.5f, levelNum) * 100);
        btns[btnNum].priceText.text = $"$ {btns[btnNum].price}";

        // ��ư ���� ���� ������Ʈ
        Image btnImage = btns[btnNum].btnObject.GetComponent<Image>();
        if (btnImage != null)
        {
            if (btnNum == currentSelect)
                btnImage.color = new Color(1f, 0.243f, 0.710f); // #FF3EB5 (RGB: 255, 62, 181 -> 1, 0.243, 0.710)
            else
                btnImage.color = Color.white; // �⺻ ���� (���)
        }
    }
    void Start()
    {
        manager = GameObject.FindWithTag("Manager");
        statusPannel = GameObject.Find("Status Pannel");

        currentSelect = 0;
        UpdateBtn(0);
        UpdateBtn(1);
        UpdateBtn(2);
        UpdateBtn(3);
    }
    float prevVert;
    float vert;
    void Update()
    {
        // ����
        prevVert = vert;
        vert = Input.GetAxisRaw("Vertical");

        if (vert > 0.3f && prevVert <= 0.3f && currentSelect > 0) currentSelect--;
        else if (vert < -0.3f && prevVert >= -0.3f && currentSelect < 3) currentSelect++;
        UpdateBtn(0);
        UpdateBtn(1);
        UpdateBtn(2);
        UpdateBtn(3);

        // ����
        if (Input.GetButtonDown("Submit"))
        {
            if (manager.GetComponent<GameManagerScript>().money >= btns[currentSelect].price)
            {
                manager.GetComponent<GameManagerScript>().money -= btns[currentSelect].price;
                statusPannel.GetComponent<StatusPannelScript>().UpdateMoneyAndRecord();

                switch (currentSelect)
                {
                    case 0:
                        manager.GetComponent<GameManagerScript>().pushLevel++;
                        break;
                    case 1:
                        manager.GetComponent<GameManagerScript>().glideLevel++;
                        break;
                    case 2:
                        manager.GetComponent<GameManagerScript>().resistanceLevel++;
                        break;
                    case 3:
                        manager.GetComponent<GameManagerScript>().incomeLevel++;
                        break;
                    default:
                        Debug.LogError($"currentSelect: {currentSelect}�� �ش��ϴ� ���׷��̵尡 �����ϴ�.");
                        break;
                }
            }
            else
            {
                Debug.Log($"���׷��̵� �Ϸ��� $ {btns[currentSelect].price - manager.GetComponent<GameManagerScript>().money}�� �� �ʿ��մϴ�.");
            }
        }

        // ���׷��̵� �ݱ� (eŰ)
        if (Input.GetButtonDown("Upgrade Menu"))
        {
            manager.GetComponent<GameManagerScript>().current = GameManagerScript.mode.wait;
            gameObject.SetActive(false);
        }
    }
}
