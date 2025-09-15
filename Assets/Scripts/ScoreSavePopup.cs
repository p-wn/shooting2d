using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSavePopup : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public Button saveBtn;
    public GameObject saveCompleteText;
    private void Start()
    {
        saveBtn.interactable = false;
        saveCompleteText.SetActive(false);
    }
    public void SaveBtn()
    {
        GameManager.instance.rankingManager.AddScore(playerNameInput.text, GameManager.instance.Score);
        saveBtn.interactable = false;
        saveCompleteText.SetActive(true);
        Destroy(gameObject, 1f);
    }
    public void CancelBtn()
    {
        Destroy(gameObject);
    }
    public void InputCheck() //입력체크
    {
        if(string.IsNullOrEmpty(playerNameInput.text))
            saveBtn.interactable = false;
        else
            saveBtn.interactable = true;    
    }
}

