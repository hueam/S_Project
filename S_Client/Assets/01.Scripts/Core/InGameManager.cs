using System.Collections;
using System.Collections.Generic;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour,IManager
{
    [SerializeField]GameObject playerPrefab,otherPlayerPreb;
    [SerializeField]List<Transform> spawnTrms = new List<Transform>();
    [SerializeField]TextMeshProUGUI bulletText;
    [SerializeField]TextMeshProUGUI hpText;

    [SerializeField]TextMeshProUGUI scoreText;

    [SerializeField]RectTransform settingPanel;


    public void Init(Transform parent)
    {
       ((UIManager)GameManager.Instance.Managers[Managers.UIManager]).SetMainUi();
    }
    public GameObject SpawnPlayer(bool isMy = true)
    {
        return Instantiate(isMy ? playerPrefab : otherPlayerPreb, spawnTrms[Random.Range(0,spawnTrms.Count)].position, Quaternion.identity);
    }
    public void ReSpawn(GameObject obj)
    {
        CharacterController movement= obj.GetComponent<CharacterController>();
        movement.enabled = false;
        obj.transform.position = spawnTrms[Random.Range(0,spawnTrms.Count)].position;
        obj.GetComponent<PlayerHealth>().Init(true);
        ((Client)GameManager.Instance.Managers[Managers.Client]).SendData((int)Events.InGame,(int)InGameTypes.ReSapwn,JsonUtility.ToJson(new TransformPaket(obj.transform.position,obj.transform.rotation)));
        movement.enabled = true;
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if (settingPanel.gameObject.activeSelf == true)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                settingPanel.gameObject.SetActive(false);
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                settingPanel.gameObject.SetActive(true);
            }
        }
    }
    public void SetBulletText(int currentAmmo, int maxAmmo)
    {
        bulletText.text = $"{currentAmmo}/{maxAmmo}";
    }
    public void SetHpText(int MaxHP, int currentHP)
    {
        hpText.text = $"{currentHP}/{MaxHP}";
    }
    public void SetScoreText(int currentScore)
    {
        scoreText.text = $"{currentScore}/20";
    }
    public void ExitGame()
    {
        ((Client)GameManager.Instance.Managers[Managers.Client]).ExitGame();
        GameManager.Instance.LoadScene((int)SceneTypes.Intro);
    }
}
