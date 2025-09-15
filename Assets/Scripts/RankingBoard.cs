using System.Collections;
using UnityEngine;

public class RankingBoard : MonoBehaviour
{
    public RankingPrefab rankingPrefab;
    public Transform rankingBase;
    void Start()
    {
        StartCoroutine(RankingUpdate());
    }
    void RankingSpawn()
    {
        ScoreData data = GameManager.instance.rankingManager.GetScoreData();       
        for(int i = 0; i < data.scores.Count; i++)
        {
            var r = Instantiate(rankingPrefab, rankingBase);
            r.DisplayRank(data.scores[i], i+1);
        }
    }
    IEnumerator RankingUpdate()
    {//���������� �ҷ����°� ������ ������ �����ϱ� ���� �����Ͱ����� 1�� �̻��� �ɶ����� ���
        yield return new WaitUntil(() => 
            GameManager.instance.rankingManager.GetScoreData().scores.Count > 0);
        RankingSpawn();
    }
}
