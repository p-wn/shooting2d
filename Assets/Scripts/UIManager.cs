using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI bombText;
    public TextMeshProUGUI stageText;
    public GameObject bossHpBarPanel; //ü�¹� ���� �ִ� �ϴ¿����� ����� ����
    public Image bossHpbar; //ü�¹� �������� �̹���
    public GameObject clearText, readyText;
    public Image blackImage;
    public GameObject playerHUD;
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameoverScore;

    public string nextScene = "Stage2";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverPanel.SetActive(false);
        readyText.SetActive(true);
        StartCoroutine(Fade(false));
        GameManager.instance.uiManager = this;
        GameManager.instance.Initailize(); //UI �ʱ�ȭ
        bossHpBarPanel.SetActive(false);
    }

    public void ScoreTextUpdate(int _score)
    {
        scoreText.text = $"{_score:000000}";
    }
    public void LifeTextUpdate(int _life)
    {
        lifeText.text = $"{_life}";
    }
    public void BombTextUpdate(int _bomb)
    {
        bombText.text = $"{_bomb}";
    }


    public void BossHpbarRefresh(int _currentHp, int _maxHp)
    {
        bossHpbar.fillAmount = (float)_currentHp / _maxHp;
    }
    public void StageClear()
    {
        clearText.SetActive(true);
      
        StartCoroutine(Fade(true));//ȭ���� ���� ��Ӱ� 
        //���̵�        
    }
    IEnumerator Fade(bool isOut)
    {
        blackImage.gameObject.SetActive(true); //������ �̹��� ������Ʈ Ȱ��ȭ
        yield return new WaitForSeconds(1f);
        Color c = blackImage.color; //�̹����� ���� �Ӽ����� �÷� ������ ����
        float fadeTime = 1; // ���̵�� �ð�
        if (!isOut)
        {
            fadeTime *= -1;  //���̵�Ÿ���� ������ ���� ���� ��ġ�� ����������
            c.a = 1; //���������� �����ؼ� ���� ������������
        }
        else
            c.a = 0; //������� �����ؼ� ���� �˾�������    
        while (true)
        {
            c.a += fadeTime * Time.deltaTime; //���İ� ����
            blackImage.color = c; //�̹����� ����
            if (c.a >= 1 || c.a <= 0) break;  //���İ��� 0�̰ų� 1�̵Ǹ� �극��ũ
            yield return null; // �������Ӹ�ŭ ����� �ݺ�
        }
        if(c.a <= 0)//���İ��� 0�̸� �����̹��� ����
        {
            readyText.SetActive(false);
            blackImage.gameObject.SetActive(false); 
        }
        else//���İ��� 1�̸� ���� ������ �̵�
        {            
            if(nextScene == "Ranking") //���� ���� ��ŷ�� ��쿡 ������ ���� ���� �˾� ����
            {
                GameObject savePopup = null;
                if (GameManager.instance.rankingManager.IsRanking(GameManager.instance.Score))
                    savePopup = Instantiate(GameManager.instance.rankingManager.savePopup, blackImage.transform);

                while (true) //���̺� �˾��� ����������� ���
                {
                    if (savePopup == null) break;
                     yield return null;
                }
            }

            if (GameManager.instance.player != null) //���࿡ �� �̵� ���� ���ӿ��� �ɰ�� ������ �Ѿ�°� ����
                SceneMove(nextScene);
        }
    }

    public void GameOver()
    {
        playerHUD.SetActive(false);
        gameOverPanel.SetActive(true);
        gameoverScore.text = $"{GameManager.instance.Score:000000}";

        if(GameManager.instance.rankingManager.IsRanking(GameManager.instance.Score))
            Instantiate(GameManager.instance.rankingManager.savePopup, gameOverPanel.transform);
        //���ӿ����� �ڽ����� ���̺� �˾� ����
    }

    public void SceneMove(string _sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneName);
    }

}
