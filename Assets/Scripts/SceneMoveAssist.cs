using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneMoveAssist : MonoBehaviour
{
    //public string nextScene = "Title";
    public void SceneMoveButton(string nextScene = "Title")
    {
        if (GameManager.instance != null && nextScene == "Stage1") Destroy(GameManager.instance.gameObject);
        //stage1�� ���°� �������̴ϱ� ���� �ʱ�ȭ�� ���� ���� ���ӸŴ����� �ı� 
        SceneManager.LoadScene(nextScene);

    }
}
