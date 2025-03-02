using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class LevelManager : MonoBehaviour
{
    GameObject manager;
    GameObject indicator;

    GameObject BgImage;
    GameObject statusPannel;
    GameObject upgradeMenu;

    // 밀기 버튼을 누른 시간
    float pressTime = 0f;

    // 공중에 떠있는 높이, 낙하 거리
    public float floatHeight = 0;
    public float fallDistance = 0;

    public int loopAmo = 0;

    // 속도 배수
    public float travelSpeedMultiplier = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 매니저 찾기(여기 주인장 나오라 그래 씹새야!!!)
        manager = GameObject.FindWithTag("Manager");

        BgImage = GameObject.Find("bgScroll");

        indicator = GameObject.Find("Push Indicator");
        indicator.SetActive(false);

        statusPannel = GameObject.Find("Status Pannel");

        upgradeMenu = GameObject.Find("Upgrade Menu");
        upgradeMenu.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

        switch (manager.GetComponent<GameManagerScript>().current)
        {
            // 대기 상태(wait)
            case GameManagerScript.mode.wait:
                if (Input.GetButtonDown("Submit"))
                {
                    // 확인(스페이스 키) 누르면 밀기 모드 진입
                    manager.GetComponent<GameManagerScript>().current = GameManagerScript.mode.push;
                    pressTime = 0f;
                    // 밀기 표시기 켬
                    indicator.SetActive(true);
                }
                else if (Input.GetButtonDown("Upgrade Menu"))
                {
                    // 업그레이드 메뉴 버튼(e 키) 누르면 업그레이드 모드 진입
                    manager.GetComponent<GameManagerScript>().current = GameManagerScript.mode.upgrade;
                    upgradeMenu.SetActive(true);
                }
                break;
            // 밀기 상태(push)
            case GameManagerScript.mode.push:
                // 순환 시간, 버튼 누른 시간 더하기
                const float cycleTime = 1.5f;
                pressTime += Time.deltaTime;

                // -|sin(pressTime*(pi/cycleTime))| + 1
                float inputPower = -1 * Mathf.Abs(Mathf.Cos(pressTime * (Mathf.PI / cycleTime))) + 1;
                indicator.GetComponent<pushBarScript>().moveIndicator(inputPower);

                // 버튼 떼서 밀기 완수
                if (Input.GetButtonUp("Submit"))
                {
                    // 현재 모드를 낙하 모드로 바꾸고, 밀기 표시기를 끄기
                    manager.GetComponent<GameManagerScript>().current = GameManagerScript.mode.fall;
                    indicator.SetActive(false);

                    // 낙하 계산
                    floatHeight = 0.5f + inputPower * Mathf.Pow(manager.GetComponent<GameManagerScript>().pushLevel, 1.25f) * 10;

                    // 낙하 거리, 속도 배수 초기화
                    fallDistance = 0f;
                    travelSpeedMultiplier = 1f;

                    loopAmo = 0;
                }
                break;
            case GameManagerScript.mode.fall:
                // 낙하
                floatHeight -= Time.deltaTime * Mathf.Pow(0.8f,Mathf.Log(manager.GetComponent<GameManagerScript>().glideLevel)) * 2;
                // 떨어진 거리
                fallDistance += Time.deltaTime * travelSpeedMultiplier * 10f;

                // 배경 움직이기
                BgImage.GetComponent<BgScroll>().scrollBG(fallDistance);

                // 낙하 높이와, 공중에 떠있는 거리 표시 업데이트
                statusPannel.GetComponent<StatusPannelScript>().UpdateFallAndFloatHeight(fallDistance, floatHeight);

                // 계단에 떨어져 끝남
                if(floatHeight <= 0)
                {
                    // 최고기록 돌파하면 업데이트
                    if (manager.GetComponent<GameManagerScript>().fallRecord < fallDistance)
                    {
                        manager.GetComponent<GameManagerScript>().fallRecord = fallDistance;
                    }
                    // 보상 돈 계산하여 더하기
                    int rewardMoney = Mathf.RoundToInt(fallDistance * Mathf.Pow(manager.GetComponent<GameManagerScript>().incomeLevel, 1.25f));
                    manager.GetComponent<GameManagerScript>().money += rewardMoney;

                    // 결과 모드로 바꾸기
                    manager.GetComponent<GameManagerScript>().current = GameManagerScript.mode.result;
                }
                break;
            case GameManagerScript.mode.result:
                // 대기 모드로 바꾸기
                if (Input.GetButtonDown("Submit"))
                {
                    statusPannel.GetComponent<StatusPannelScript>().UpdateMoneyAndRecord();
                    manager.GetComponent<GameManagerScript>().current = GameManagerScript.mode.wait;
                }
                break;
        }
    }
}
