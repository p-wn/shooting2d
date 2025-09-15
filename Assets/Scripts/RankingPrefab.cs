
using TMPro;
using UnityEngine;

public class RankingPrefab : MonoBehaviour
{

    public TextMeshProUGUI rank;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI score;

    public GameObject[] rankImages;

    public void DisplayRank(ScoreEntry _scoreEntry, int _rank)
    {
        rank.text = _rank.ToString(); 
        playerName.text = _scoreEntry.playerName.ToString();
        score.text = _scoreEntry.score.ToString();
        for (int i = 0; i < rankImages.Length; i++)
        {//_rank는 1~3의값이라서  -1한 랭크 이미지를 표시
            if (i == _rank-1 && _rank <=3 )
            { 
                rankImages[i].SetActive(true);
             }
            else rankImages[i].SetActive(false);
        }
        if(_rank == 1)
        {
            rank.color = Color.black;
        }
  
    }
}
