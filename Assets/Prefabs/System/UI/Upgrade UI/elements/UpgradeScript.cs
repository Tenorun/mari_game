using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable] // Inspector에서 보이도록 설정
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
        // 버튼 이름 업데이트
        btns[btnNum].nameText.text = btns[btnNum].name;

        int levelNum = 0;

        // 레벨 값 얻기
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
                Debug.LogError($"btnNum: {btnNum}에 해당하는 버튼이 없습니다.");
                break;
        }

        // 레벨 표시, 가격, 가격 표시 업데이트
        btns[btnNum].levelText.text = $"Lv.{levelNum}";
        btns[btnNum].price = Mathf.RoundToInt(Mathf.Pow(2.5f, levelNum) * 100);
        btns[btnNum].priceText.text = $"$ {btns[btnNum].price}";

        // 버튼 선택 여부 업데이트
        Image btnImage = btns[btnNum].btnObject.GetComponent<Image>();
        if (btnImage != null)
        {
            if (btnNum == currentSelect)
                btnImage.color = new Color(1f, 0.243f, 0.710f); // #FF3EB5 (RGB: 255, 62, 181 -> 1, 0.243, 0.710)
            else
                btnImage.color = Color.white; // 기본 색상 (흰색)
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
        // 조작
        prevVert = vert;
        vert = Input.GetAxisRaw("Vertical");

        if (vert > 0.3f && prevVert <= 0.3f && currentSelect > 0) currentSelect--;
        else if (vert < -0.3f && prevVert >= -0.3f && currentSelect < 3) currentSelect++;
        UpdateBtn(0);
        UpdateBtn(1);
        UpdateBtn(2);
        UpdateBtn(3);

        // 구매
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
                        Debug.LogError($"currentSelect: {currentSelect}에 해당하는 업그레이드가 없습니다.");
                        break;
                }
            }
            else
            {
                Debug.Log($"업그레이드 하려면 $ {btns[currentSelect].price - manager.GetComponent<GameManagerScript>().money}가 더 필요합니다.");
            }
        }

        // 업그레이드 닫기 (e키)
        if (Input.GetButtonDown("Upgrade Menu"))
        {
            manager.GetComponent<GameManagerScript>().current = GameManagerScript.mode.wait;
            gameObject.SetActive(false);
        }
    }
}
