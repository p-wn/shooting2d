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
        };//�÷��̾� ���� ������ �Է¹���
        scoreData.scores.Add(score); //����Ʈ�� �߰�
        scoreData.scores.Sort((a, b) => b.score.CompareTo(a.score));//�������� ����
        if (scoreData.scores.Count > 10)
            scoreData.scores.RemoveAt(scoreData.scores.Count - 1); //���� ���� ���� �����͸� ����Ʈ���� ����

        SaveScore(); //����� ��ŷ���� ����
    }
    public void SaveScore()
    {

        string json = JsonUtility.ToJson(scoreData);
        print(json);
        File.WriteAllText(savePath, json);
    }

    public void LoadScore()
    {
        if(File.Exists(savePath)) //savePath��ο� ������ �ִ��� üũ
        {
            string json = File.ReadAllText(savePath);
            scoreData = JsonUtility.FromJson<ScoreData>(json);
            //json ���ڿ��� ScoreData Ŭ���� �������� ������ȭ
            print($"{scoreData.scores[0].playerName} : {scoreData.scores[0].score}");
            print($"{scoreData.scores[1].playerName} : {scoreData.scores[1].score}");
            print($"{scoreData.scores[2].playerName} : {scoreData.scores[2].score}");
            //������ Ȯ�ο� print
        }
        else
        {
            scoreData = new ScoreData();
        }
    }

    public bool IsRanking(int _score) //��ŷ�� ����� �ƴ��� üũ
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
