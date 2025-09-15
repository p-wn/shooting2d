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
    {//점수데이터 불러오는게 끝나고 프리팹 생성하기 위해 데이터갯수가 1개 이상이 될때까지 대기
        yield return new WaitUntil(() => 
            GameManager.instance.rankingManager.GetScoreData().scores.Count > 0);
        RankingSpawn();
    }
}
