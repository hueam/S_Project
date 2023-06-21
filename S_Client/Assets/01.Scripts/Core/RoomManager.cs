using System.Collections;
using System.Collections.Generic;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour, IManager
{
    [SerializeField] Transform spawnTrm;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject otherPlayerPrefab;
    [SerializeField] Button readyBtn, startBtn;
    [SerializeField] TextMeshProUGUI peopleText;
    [SerializeField] TextMeshProUGUI roomIDTxt;
    [SerializeField] GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;

    int maxPeople;
    int currentPeople;

    bool isReady = false;
    public void Init(Transform parent)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ((UIManager)GameManager.Instance.Managers[Managers.UIManager]).SetMainUi();
        roomIDTxt.text = GameManager.Instance.roomID;
    }
    public GameObject SpawnPlayer(bool isMy = true)
    {
        maxPeople++;
        peopleText.text = $"{currentPeople}/{maxPeople}";
        if (currentPeople < maxPeople)
        {
            startBtn.gameObject.SetActive(false);
        }
        return Instantiate(isMy ? playerPrefab : otherPlayerPrefab, spawnTrm.position, Quaternion.identity);
    }
    public void OnReadyBtn()
    {
        currentPeople += isReady ? -1 : +1;
        peopleText.text = $"{currentPeople}/{maxPeople}";
        if (currentPeople == maxPeople)
        {
            startBtn.gameObject.SetActive(true);

        }
        else if (currentPeople < maxPeople)
        {
            startBtn.gameObject.SetActive(false);

        }
        readyBtn.image.color = isReady ? Color.red : Color.black;
        isReady = !isReady;
        ((Client)GameManager.Instance.Managers[Managers.Client]).SendData((int)Events.Room, (int)RoomTypes.Ready, isReady.ToString()); ;

    }
    public void OnPlayBtn()
    {
        ((Client)GameManager.Instance.Managers[Managers.Client]).SendData((int)Events.Room, (int)RoomTypes.Start, "");
    }
    void Update()
    {
        //Check if the left Mouse button is clicked
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            m_PointerEventData = new PointerEventData(EventSystem.current);
            m_PointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();

            m_Raycaster.Raycast(m_PointerEventData, results);
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.TryGetComponent<Button>(out readyBtn))
                {
                    readyBtn.onClick?.Invoke();
                }
            }
        }
    }
    public void OtherExit(bool isReady)
    {
        currentPeople += isReady ? 1 : -1;
        maxPeople--;
        peopleText.text = $"{currentPeople}/{maxPeople}";
        if (currentPeople == maxPeople)
        {
            startBtn.interactable = true;
        }
        else if (currentPeople < maxPeople)
        {
            startBtn.interactable = false;
        }
    }
    public void OtherReady(bool isReady)
    {
        currentPeople += isReady ? 1 : -1;
        peopleText.text = $"{currentPeople}/{maxPeople}";
        if (currentPeople == maxPeople)
        {
            startBtn.gameObject.SetActive(true);

        }
        else if (currentPeople < maxPeople)
        {
            startBtn.gameObject.SetActive(false);

        }
    }
    public void OnApplicationQuit()
    {

    }

}
