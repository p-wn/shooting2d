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
    public GameObject bossHpBarPanel; //체력바 껏다 켯다 하는용으로 등록할 변수
    public Image bossHpbar; //체력바 게이지용 이미지
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
        GameManager.instance.Initailize(); //UI 초기화
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
      
        StartCoroutine(Fade(true));//화면을 점점 어둡게 
        //씬이동        
    }
    IEnumerator Fade(bool isOut)
    {
        blackImage.gameObject.SetActive(true); //검은색 이미지 오브젝트 활성화
        yield return new WaitForSeconds(1f);
        Color c = blackImage.color; //이미지의 색상 속성값을 컬러 변수로 뽑음
        float fadeTime = 1; // 페이드될 시간
        if (!isOut)
        {
            fadeTime *= -1;  //페이드타임을 음수로 만들어서 점점 수치가 낮아지도록
            c.a = 1; //검은색부터 시작해서 점점 투명해지도록
        }
        else
            c.a = 0; //투명부터 시작해서 점점 검어지도록    
        while (true)
        {
            c.a += fadeTime * Time.deltaTime; //알파값 변경
            blackImage.color = c; //이미지에 적용
            if (c.a >= 1 || c.a <= 0) break;  //알파값이 0이거나 1이되면 브레이크
            yield return null; // 한프레임만큼 대기후 반복
        }
        if(c.a <= 0)//알파값이 0이면 검정이미지 끄기
        {
            readyText.SetActive(false);
            blackImage.gameObject.SetActive(false); 
        }
        else//알파값이 1이면 다음 씬으로 이동
        {            
            if(nextScene == "Ranking") //다음 씬이 랭킹인 경우에 점수에 따라 저장 팝업 띄우기
            {
                GameObject savePopup = null;
                if (GameManager.instance.rankingManager.IsRanking(GameManager.instance.Score))
                    savePopup = Instantiate(GameManager.instance.rankingManager.savePopup, blackImage.transform);

                while (true) //세이브 팝업이 사라질때까지 대기
                {
                    if (savePopup == null) break;
                     yield return null;
                }
            }

            if (GameManager.instance.player != null) //만약에 씬 이동 전에 게임오버 될경우 다음씬 넘어가는거 방지
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
        //게임오버의 자식으로 세이브 팝업 생성
    }

    public void SceneMove(string _sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneName);
    }

}
