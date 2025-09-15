using UnityEngine;

public class BossEnemy : Enemy
{
   
    UIManager uiManager;
    public BulletParticle[] bulletParticles;
    private void OnEnable()
    {   //���� Ȱ��ȭ�� ü�¹� ǥ�� �� �ʱ�ȭ
        isBoss = true;
        uiManager = GameManager.instance.uiManager;
        uiManager.bossHpBarPanel.SetActive(true);
        uiManager.BossHpbarRefresh(currentHP, maxHP);
    }

    public override void DamagedEnemy(int dmg)
    {
        
        base.DamagedEnemy(dmg);
        uiManager.BossHpbarRefresh(currentHP, maxHP);
        if(currentHP <= 0)
        {
            uiManager.bossHpBarPanel.SetActive(false); //���� ü�¹� �����
            Vector3 offset = new Vector3(Random.Range(0, 2), Random.Range(0, 2), 0); //������ ��ġ offset
            GameObject item = GameManager.instance.GetItem(); //���ӸŴ������� ������ ������ ��������
            if (item != null) //������ �������� null�� �ƴϸ� ����
                Instantiate(item, transform.position + offset, Quaternion.identity); //������ ������ ������ �ϳ� ����
            //���� �������� Ŭ���� ���� ó��
        }
    }

    public void BulletStart(int index)
    {
        bulletParticles[index].ps.Play();
    }
    public void BulletStop(int index)
    {
        bulletParticles[index].ps.Stop();
    }
}
