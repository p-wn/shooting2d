using System.Collections;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    private void Start()
    {
        GameManager.instance.playerSpawner = this;
        SpawnPlayer();
    }
    public void SpawnPlayer(float delayTime = 0)
    {
        StartCoroutine(PlayerSpawnRoutine(delayTime));
    }
    IEnumerator PlayerSpawnRoutine(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        var player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        GameManager.instance.player = player.GetComponent<PlayerControl>(); //게임매니져에 플레이어 등록
        GameManager.instance.player.Invincible(false); //플레이어 조작 막기
        
        while (true)
        {
            player.transform.position += Vector3.up  * Time.deltaTime;
            //플레이어가 앞으로 이동
            if (player.transform.position.y >= transform.position.y + 3.2f)
            {  //플레이어가 시작위치보다 3.2정도 앞으로 나오면 정지
                break;
            }
            yield return null;
        }
        GameManager.instance.player.Invincible(true); //플레이어 조작 다시 가능하게
    }
}
