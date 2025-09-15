using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class ScoreEntry
{
    public string playerName;
    public int score;
}

[Serializable]
public class ScoreData
{
    public List<ScoreEntry> scores = new List<ScoreEntry>();
}
public class RankingManager : MonoBehaviour
{
    string savePath;
    ScoreData scoreData = new ScoreData();
    public GameObject savePopup;
    private void Awake()
    {
        savePath = Path.Combine(Application.dataPath, "rankingData.json");
    }
    private void Start()
    {
        LoadScore();
    }

    public void AddScore(string _name, int _score)
    {
        ScoreEntry score = new ScoreEntry {
            playerName = _name,
            score = _score
        };//플레이어 점수 데이터 입력받음
        scoreData.scores.Add(score); //리스트에 추가
        scoreData.scores.Sort((a, b) => b.score.CompareTo(a.score));//내림차순 정렬
        if (scoreData.scores.Count > 10)
            scoreData.scores.RemoveAt(scoreData.scores.Count - 1); //제일 낮은 점수 데이터를 리스트에서 삭제

        SaveScore(); //변경된 랭킹으로 저장
    }
    public void SaveScore()
    {

        string json = JsonUtility.ToJson(scoreData);
        print(json);
        File.WriteAllText(savePath, json);
    }

    public void LoadScore()
    {
        if(File.Exists(savePath)) //savePath경로에 파일이 있는지 체크
        {
            string json = File.ReadAllText(savePath);
            scoreData = JsonUtility.FromJson<ScoreData>(json);
            //json 문자열을 ScoreData 클래스 형식으로 역직렬화
            print($"{scoreData.scores[0].playerName} : {scoreData.scores[0].score}");
            print($"{scoreData.scores[1].playerName} : {scoreData.scores[1].score}");
            print($"{scoreData.scores[2].playerName} : {scoreData.scores[2].score}");
            //데이터 확인용 print
        }
        else
        {
            scoreData = new ScoreData();
        }
    }

    public bool IsRanking(int _score) //랭킹에 드는지 아닌지 체크
    {
        if(scoreData.scores.Count < 10)
            return true;
        
        if (scoreData.scores[scoreData.scores.Count-1].score < _score)
            return true;
        else
            return false;
    }

    public ScoreData GetScoreData()
    {
        return scoreData; 
    }
}
