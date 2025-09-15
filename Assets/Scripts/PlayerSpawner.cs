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
        GameManager.instance.player = player.GetComponent<PlayerControl>(); //���ӸŴ����� �÷��̾� ���
        GameManager.instance.player.Invincible(false); //�÷��̾� ���� ����
        
        while (true)
        {
            player.transform.position += Vector3.up  * Time.deltaTime;
            //�÷��̾ ������ �̵�
            if (player.transform.position.y >= transform.position.y + 3.2f)
            {  //�÷��̾ ������ġ���� 3.2���� ������ ������ ����
                break;
            }
            yield return null;
        }
        GameManager.instance.player.Invincible(true); //�÷��̾� ���� �ٽ� �����ϰ�
    }
}
