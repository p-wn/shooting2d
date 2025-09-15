using UnityEngine;

public enum ItemType //������ ���� �̸� ����
{
    weapon, bomb, life, score
}
public class Item : MonoBehaviour
{
    public ItemType itemType = ItemType.weapon; //������ ����
    public int itemValue = 0;
    public int weight = 20; //��íȮ��

    private void Start()
    {
        Destroy(gameObject, 5);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            ItemEffect();
            Destroy(gameObject); //������ �ı�
        }
    }
    void ItemEffect()
    {
        switch (itemType)
        {
            case ItemType.weapon:
                GameManager.instance.player.WeaponChange(itemValue);
                break;
            case ItemType.bomb:
                GameManager.instance.bombCountChange(itemValue);
                break;
            case ItemType.life:
                GameManager.instance.LifeCountChange(itemValue);
                break;
            case ItemType.score:
                GameManager.instance.ScoreChange(itemValue);
                break;
            default:
                break;
        }
    }

    

}
