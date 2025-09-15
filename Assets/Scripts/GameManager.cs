using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] itemArray;
    public static GameManager instance;
    public PlayerSpawner playerSpawner; //매 스테이지 플레이어생성기 연결
    public PlayerControl player; //매 스테이지 플레이어 연결
    public UIManager uiManager;
    public RankingManager rankingManager;
    int score = 0;
    int life = 2;
    int bomb = 2;
    public int weaponIndex = 0;
   
    public int Score { get => score; }
    public int Life { get => life; }
    public int Bomb { get => bomb; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        rankingManager = GetComponent<RankingManager>();
    }
    public void Initailize()//ui 초기화용
    {
        ScoreChange(0);
        LifeCountChange(0);
        bombCountChange(0);
    }
    public void ScoreChange(int _value)
    {
        score += _value;
        uiManager.ScoreTextUpdate(score);
    }
    public void LifeCountChange(int _value)
    {
        life += _value;
        uiManager.LifeTextUpdate(life);
    }
    public void bombCountChange(int _value)
    {
        bomb += _value;
        uiManager.BombTextUpdate(bomb);
    }

    public GameObject GetItem() //랜덤한 아이템 선택
    {
        int i = 0;
        foreach (var v in itemArray)
        {
            i += v.GetComponent<Item>().weight;
        }

        int rnd = Random.Range(0, i);
        int w = 0;
        foreach(var v in itemArray)
        {
            w += v.GetComponent<Item>().weight;
            if(rnd <= w)
            {
                return v;
            }
        }
        return null; // 아이템이 하나도 선택안되면 null
                     //이론상 이쪽으로 빠져나올일은 없다고함
    }

    public GameObject GetItem(int index) //해당 인덱스의 아이템을 선택
    {
        GameObject item = itemArray[index];
        return item;
    }

}
