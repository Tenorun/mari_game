using TMPro;
using UnityEngine;

public class StatusPannelScript : MonoBehaviour
{
    GameObject manager;

    // Display GameObjects
    GameObject moneyDisplay;
    GameObject fallHeightDisplay;
    GameObject floHeightDisplay;
    GameObject recordFallDisplay;

    // TMP Components
    TextMeshProUGUI moneyText;
    TextMeshProUGUI fallHeightText;
    TextMeshProUGUI floHeightText;
    TextMeshProUGUI recordFallText;

    void Start()
    {
        // Find UI Elements
        moneyDisplay = GameObject.Find("Money Display");
        fallHeightDisplay = GameObject.Find("Fall Height Display");
        floHeightDisplay = GameObject.Find("Float Height Display");
        recordFallDisplay = GameObject.Find("Record Display");

        // Find TMP Components
        moneyText = moneyDisplay?.GetComponent<TextMeshProUGUI>();
        fallHeightText = fallHeightDisplay?.GetComponent<TextMeshProUGUI>();
        floHeightText = floHeightDisplay?.GetComponent<TextMeshProUGUI>();
        recordFallText = recordFallDisplay?.GetComponent<TextMeshProUGUI>();

        // Find Manager
        manager = GameObject.FindWithTag("Manager");

        // Debugging
        if (moneyDisplay == null) Debug.LogError("Money Display GameObject not found!");
        if (fallHeightDisplay == null) Debug.LogError("Fall Height Display GameObject not found!");
        if (floHeightDisplay == null) Debug.LogError("Float Height Display GameObject not found!");
        if (recordFallDisplay == null) Debug.LogError("Record Display GameObject not found!");
        if (moneyText == null) Debug.LogError("TextMeshProUGUI component missing on Money Display!");
        if (fallHeightText == null) Debug.LogError("TextMeshProUGUI component missing on Fall Height Display!");
        if (floHeightText == null) Debug.LogError("TextMeshProUGUI component missing on Float Height Display!");
        if (recordFallText == null) Debug.LogError("TextMeshProUGUI component missing on Record Display!");
        if (manager == null) Debug.LogError("Manager object not found! Check if it has the 'Manager' tag.");
        if (manager != null && manager.GetComponent<GameManagerScript>() == null)
            Debug.LogError("GameManagerScript component is missing on Manager GameObject!");

        UpdateFallAndFloatHeight(0, 0);
        UpdateMoneyAndRecord();
    }

    public void UpdateFallAndFloatHeight(float fallHeight, float floHeight)
    {
        if (fallHeightText != null && floHeightText != null)
        {
            fallHeightText.text = "현재 낙하 높이: " + fallHeight.ToString("F2");
            floHeightText.text = "떠있는 높이: " + floHeight.ToString("F2");
        }
        else
        {
            Debug.LogError("Fall/Float height text is null!");
        }
    }

    public void UpdateMoneyAndRecord()
    {
        if (manager != null)
        {
            GameManagerScript gameManager = manager.GetComponent<GameManagerScript>();
            if (gameManager != null)
            {
                if (moneyText != null)
                    moneyText.text = "가진 돈: " + gameManager.money.ToString();
                if (recordFallText != null)
                    recordFallText.text = "낙하 최고기록: " + gameManager.fallRecord.ToString("F2");
            }
            else
            {
                Debug.LogError("GameManagerScript is missing from the Manager GameObject!");
            }
        }
        else
        {
            Debug.LogError("Manager is null!");
        }
    }
}
