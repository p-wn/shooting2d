using UnityEngine;

public class BossEnemy : Enemy
{
   
    UIManager uiManager;
    public BulletParticle[] bulletParticles;
    private void OnEnable()
    {   //보스 활성화시 체력바 표시 및 초기화
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
            uiManager.bossHpBarPanel.SetActive(false); //보스 체력바 숨기기
            Vector3 offset = new Vector3(Random.Range(0, 2), Random.Range(0, 2), 0); //아이템 위치 offset
            GameObject item = GameManager.instance.GetItem(); //게임매니져에서 랜덤한 아이템 가져오기
            if (item != null) //가져온 아이템이 null이 아니면 생성
                Instantiate(item, transform.position + offset, Quaternion.identity); //보스는 무조건 아이템 하나 생성
            //이후 스테이지 클리어 관련 처리
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
