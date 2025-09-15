using UnityEngine;

public enum ItemType //아이템 종류 이름 정의
{
    weapon, bomb, life, score
}
public class Item : MonoBehaviour
{
    public ItemType itemType = ItemType.weapon; //아이템 종류
    public int itemValue = 0;
    public int weight = 20; //가챠확률

    private void Start()
    {
        Destroy(gameObject, 5);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            ItemEffect();
            Destroy(gameObject); //아이템 파괴
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
