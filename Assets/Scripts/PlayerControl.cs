
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public GameObject[] weaponArray; //�����
    public GameObject bombPrefab; //��ź����Ʈ ������
    public GameObject explosionFx;
    public float moveSpd = 200f;
    Rigidbody2D rb;
    Vector2 moveDir = Vector2.zero;
    Vector2 screenSize;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        WeaponChange(GameManager.instance.weaponIndex);
        WeaponOnOff(false);
        Vector3 spriteSize = gameObject.GetComponent<SpriteRenderer>().localBounds.size; //�÷��̾� ��������Ʈ ũ����
        float height = Camera.main.orthographicSize ; //ȭ�� ���� ���
        float width = (height * ((float)Screen.width / Screen.height)); //ȭ�� ���� ��� * ȭ�� ����)
        screenSize = new Vector2(width - (spriteSize.x / 2), height - (spriteSize.y / 2)); //�÷��̾��� ũ���� ���ݸ�ŭ ����

        print($"{spriteSize} / screensize: {screenSize}");
    }
    private void FixedUpdate()
    {
        Movement();
    }
    void Movement()
    {
        if (Mathf.Abs(transform.position.x) >= screenSize.x && transform.position.x * moveDir.x > 0)
        {//�÷��̾��� x��ǥ�� ���밪(-5�� +5�� 5) ȭ���� �ǳ���ǥ��(5)���� ũ�ų� ����
            //�̵������ �÷��̾ �ִ� ������ ������ ���̻� x�������� �̵����� ���ϰ� ����
            moveDir.x = 0;
        }
        if(Mathf.Abs(transform.position.y) >= screenSize.y && transform.position.y * moveDir.y > 0)
        {
            moveDir.y = 0;
        }
       Vector2 finalMove = moveDir * moveSpd * Time.deltaTime;
       rb.linearVelocity = finalMove ;
        


    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>(); //��ƽ�� ���� �о���ºκ�

    }

    public void Death()
    {
        GameManager.instance.weaponIndex = 0;
        Instantiate(explosionFx, transform.position, Quaternion.identity);
        if(GameManager.instance.Life > 0)
        {
            GameManager.instance.playerSpawner.SpawnPlayer(2f);
            GameManager.instance.LifeCountChange(-1);
        }
        else
        {
            GameManager.instance.uiManager.GameOver();
        }

        Destroy(gameObject);
    }
    bool isBlink = false; //������ ������ üũ�� bool
    public void Invincible(bool isOn)
    {    
        GetComponent<PlayerInput>().enabled = isOn;
        if (!isOn)
        {
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(BlinkSprite()); //������ �ڷ�ƾ ����
            
        }
        else
        {
            WeaponOnOff(true);
            StartCoroutine(EndBlink());
        }
    }

    IEnumerator EndBlink()
    {
        GetComponent<PlayerInput>().enabled = true;
        yield return new WaitForSeconds(2f);
        isBlink = false;
        GetComponent<Collider2D>().enabled = true;
    }

    IEnumerator BlinkSprite()
    {
        isBlink = true;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color c = renderer.color; //���� ������ ����
        while (true)
        {
            if (c.a == 1)   //���İ��� 0.3�� 1�� �ٲ㼭 �����̰�
                c.a = 0.3f;
            else
                c.a = 1;

            if(!isBlink) 
            {
                c.a = 1;
                renderer.color = c;
                break;
            }
            renderer.color = c; //�ٲ� ������ �������� �ֱ�
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Death();
        }
    }

    public void Boooom(InputAction.CallbackContext context) // ���� �� �ı�
    {      
        if (!context.performed || GameManager.instance.Bomb <= 0) return; 
        //�Է��� ���� ���°� �ƴϰų� ��ź������ 0���� ������ �Ʒ��� ���� ����
        Instantiate(bombPrefab); //��ź ����Ʈ ���
        GameManager.instance.bombCountChange(-1); //��ź ���� ����
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None); //��� �� ã��         
        StartCoroutine(BulletReset()); //�� ź�� ����
        foreach (var e in enemies) //��� �� ������� �ݺ��� ����
        {
            if(e != null)
            {
                if(e.isBoss) e.DamagedEnemy(400); // ��ź���¸�ŭ ������ �ֱ�
                else e.DamagedEnemy(e.maxHP); // �ִ�ü�¸�ŭ ������ �ֱ�
            }
        }

    }

   /* public void Boooom2(InputAction.CallbackContext context) //ź�˸� ����
    {
        if (!context.performed || GameManager.instance.Bomb <= 0) return;
        //�Է��� ���� ���°� �ƴϰų� ��ź������ 0���� ������ �Ʒ��� ���� ����
        Instantiate(bombPrefab); //��ź ����Ʈ ���
        GameManager.instance.bombCountChange(-1); //��ź ���� ����
        StartCoroutine(BulletReset());
    }*/
    IEnumerator BulletReset()
    {
        BulletParticle[] bullet = FindObjectsByType<BulletParticle>(FindObjectsSortMode.None); //��� �� ã��         
        foreach (var b in bullet) //��� �� ������� �ݺ��� ����
        {
            if (b.isEnemy)
            {
                b.ParticlePlay(false);
            }
        }

        yield return new WaitForSeconds(2); //2�� �ڿ�
        bullet = FindObjectsByType<BulletParticle>(FindObjectsSortMode.None);
        foreach (var b in bullet) //��� �� ������� �ݺ��� ����
        {
            if (b.isEnemy)
            {
                b.ParticlePlay(true);
            }
        }
    }

    public void WeaponChange(int index)
    {
        GameManager.instance.weaponIndex = index;
        for(int i = 0; i < weaponArray.Length; i++ )
        {
            if (weaponArray[i].activeInHierarchy && i != index) weaponArray[i].SetActive(false);
            //�̹� �����ִ� ����� ����
            if (i == index) weaponArray[i].SetActive(true);
            //index�� �ش��ϴ� ����� �ѱ�
        }
    }

    public void WeaponOnOff(bool isOn)
    {
        for (int i = 0; i < weaponArray.Length; i++)
        {
            if (isOn) weaponArray[i].GetComponent<ParticleSystem>().Play(); 
            else weaponArray[i].GetComponent<ParticleSystem>().Stop();
            //���� ��ƼŬ�� �÷����ϰų� ���߸� �ڽ������ִ� ��ƼŬ�� ���� �۵��Ѵ�

        }
    }

}
