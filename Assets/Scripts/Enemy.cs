using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject explosionFx; //파괴시 폭발효과
    public int maxHP = 100;
    public int score = 1000;
    public int currentHP;
    public bool isBoss = false;
    public ParticleSystem[] psArray;

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        currentHP = maxHP;
        psArray = GetComponentsInChildren<ParticleSystem>();
       // FireStart(false);
    }

    public void FireStart(bool isOn)
    {
        foreach (ParticleSystem ps in psArray)
        {
            ps.gameObject.SetActive(isOn);
        }
    }

    public virtual void DamagedEnemy(int dmg)
    {
        currentHP -= dmg; //dmg만큼 체력감소
        if (currentHP <= 0) //0보다 작아지면
        {
            GameManager.instance.ScoreChange(score);
            Instantiate(explosionFx, transform.position, Quaternion.identity); //폭발 이펙트 표시
            int r = Random.Range(0, 100);
            if (r < 50) //r이 50보다 작은 경우에만 생성
            {
                GameObject item = GameManager.instance.GetItem(); //게임매니져에서 랜덤한 아이템 가져오기
                if (item != null) //가져온 아이템이 null이 아니면 생성
                    Instantiate(item, transform.position, Quaternion.identity);
            }
            Destroy(gameObject); //파괴
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Out"))
        {
            if (transform.parent.childCount <= 1)  //자신의 부모기준 자식오브젝트가 0~1개일때 부모를 파괴 
                Destroy(transform.parent.gameObject);
            else
                Destroy(gameObject); //아니면 자기만 파괴
        }
    }
}
