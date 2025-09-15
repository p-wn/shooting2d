using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyGroups;
    public GameObject bossPrefab;

    public float bossTimer = 30;
    public float enemyInterval = 5;
    private void Start()
    {
        StartCoroutine(RandomSpawn());
    }
    public void SpawnStart()
    {
        int r = Random.Range(0, enemyGroups.Length);
        var enemyG = Instantiate(enemyGroups[r]);
        float animLength = enemyG.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        //에네미 그룹의 애니메이터에서 현재 재생중인 스테이트의 길이를 알아낸다음
        Destroy(enemyG, animLength); //해당 시간 이후에 에네미 그룹 파괴
    }

    IEnumerator RandomSpawn()
    {
        yield return new WaitForSeconds(3f);
        while (true)
        {
            if (bossTimer <= 0) break;
            SpawnStart();
            yield return new WaitForSeconds(enemyInterval); //enemyInterval만큼 기다리고 반복
            bossTimer -= enemyInterval; //타이머에서 enemyInterval만큼 차감
        }
        GameObject boss = Instantiate(bossPrefab);
        yield return new WaitUntil(() => boss == null); //조건을 만족 할때까지 대기
        GameManager.instance.player.WeaponOnOff(false);
        yield return new WaitForSeconds(3f);
        GameManager.instance.uiManager.StageClear();
        //스테이지 클리어 연출 및 다음스테이지 이동

    }
}
