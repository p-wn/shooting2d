using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject explosionFx; //�ı��� ����ȿ��
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
        currentHP -= dmg; //dmg��ŭ ü�°���
        if (currentHP <= 0) //0���� �۾�����
        {
            GameManager.instance.ScoreChange(score);
            Instantiate(explosionFx, transform.position, Quaternion.identity); //���� ����Ʈ ǥ��
            int r = Random.Range(0, 100);
            if (r < 50) //r�� 50���� ���� ��쿡�� ����
            {
                GameObject item = GameManager.instance.GetItem(); //���ӸŴ������� ������ ������ ��������
                if (item != null) //������ �������� null�� �ƴϸ� ����
                    Instantiate(item, transform.position, Quaternion.identity);
            }
            Destroy(gameObject); //�ı�
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Out"))
        {
            if (transform.parent.childCount <= 1)  //�ڽ��� �θ���� �ڽĿ�����Ʈ�� 0~1���϶� �θ� �ı� 
                Destroy(transform.parent.gameObject);
            else
                Destroy(gameObject); //�ƴϸ� �ڱ⸸ �ı�
        }
    }
}
