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
        //���׹� �׷��� �ִϸ����Ϳ��� ���� ������� ������Ʈ�� ���̸� �˾Ƴ�����
        Destroy(enemyG, animLength); //�ش� �ð� ���Ŀ� ���׹� �׷� �ı�
    }

    IEnumerator RandomSpawn()
    {
        yield return new WaitForSeconds(3f);
        while (true)
        {
            if (bossTimer <= 0) break;
            SpawnStart();
            yield return new WaitForSeconds(enemyInterval); //enemyInterval��ŭ ��ٸ��� �ݺ�
            bossTimer -= enemyInterval; //Ÿ�̸ӿ��� enemyInterval��ŭ ����
        }
        GameObject boss = Instantiate(bossPrefab);
        yield return new WaitUntil(() => boss == null); //������ ���� �Ҷ����� ���
        GameManager.instance.player.WeaponOnOff(false);
        yield return new WaitForSeconds(3f);
        GameManager.instance.uiManager.StageClear();
        //�������� Ŭ���� ���� �� ������������ �̵�

    }
}
