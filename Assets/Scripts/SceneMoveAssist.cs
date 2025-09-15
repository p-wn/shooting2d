using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneMoveAssist : MonoBehaviour
{
    //public string nextScene = "Title";
    public void SceneMoveButton(string nextScene = "Title")
    {
        if (GameManager.instance != null && nextScene == "Stage1") Destroy(GameManager.instance.gameObject);
        //stage1로 가는건 새게임이니까 점수 초기화를 위해 기존 게임매니져는 파괴 
        SceneManager.LoadScene(nextScene);

    }
}
