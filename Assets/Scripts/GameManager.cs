using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] itemArray;
    public static GameManager instance;
    public PlayerSpawner playerSpawner; //�� �������� �÷��̾������ ����
    public PlayerControl player; //�� �������� �÷��̾� ����
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
    public void Initailize()//ui �ʱ�ȭ��
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

    public GameObject GetItem() //������ ������ ����
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
        return null; // �������� �ϳ��� ���þȵǸ� null
                     //�̷л� �������� ������������ ���ٰ���
    }

    public GameObject GetItem(int index) //�ش� �ε����� �������� ����
    {
        GameObject item = itemArray[index];
        return item;
    }

}
