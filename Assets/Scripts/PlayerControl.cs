
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public GameObject[] weaponArray; //무기들
    public GameObject bombPrefab; //폭탄이펙트 프리팹
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
        Vector3 spriteSize = gameObject.GetComponent<SpriteRenderer>().localBounds.size; //플레이어 스프라이트 크기계산
        float height = Camera.main.orthographicSize ; //화면 높이 계산
        float width = (height * ((float)Screen.width / Screen.height)); //화면 가로 계산 * 화면 비율)
        screenSize = new Vector2(width - (spriteSize.x / 2), height - (spriteSize.y / 2)); //플레이어의 크기의 절반만큼 빼기

        print($"{spriteSize} / screensize: {screenSize}");
    }
    private void FixedUpdate()
    {
        Movement();
    }
    void Movement()
    {
        if (Mathf.Abs(transform.position.x) >= screenSize.x && transform.position.x * moveDir.x > 0)
        {//플레이어의 x좌표의 절대값(-5든 +5든 5) 화면의 맨끝좌표값(5)보다 크거나 같고
            //이동방향과 플레이어가 있는 방향이 같을때 더이상 x방향으로 이동하지 못하게 막음
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
        moveDir = context.ReadValue<Vector2>(); //스틱의 값을 읽어오는부분

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
    bool isBlink = false; //깜박임 중인지 체크용 bool
    public void Invincible(bool isOn)
    {    
        GetComponent<PlayerInput>().enabled = isOn;
        if (!isOn)
        {
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(BlinkSprite()); //깜박임 코루틴 시작
            
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
        Color c = renderer.color; //원래 색상값을 저장
        while (true)
        {
            if (c.a == 1)   //알파값을 0.3과 1을 바꿔서 깜박이게
                c.a = 0.3f;
            else
                c.a = 1;

            if(!isBlink) 
            {
                c.a = 1;
                renderer.color = c;
                break;
            }
            renderer.color = c; //바꾼 색상을 렌더러에 넣기
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

    public void Boooom(InputAction.CallbackContext context) // 적들 다 파괴
    {      
        if (!context.performed || GameManager.instance.Bomb <= 0) return; 
        //입력이 퍼폼 상태가 아니거나 폭탄갯수가 0보다 작으면 아랫줄 실행 안함
        Instantiate(bombPrefab); //폭탄 이펙트 재생
        GameManager.instance.bombCountChange(-1); //폭탄 갯수 감소
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None); //모든 적 찾기         
        StartCoroutine(BulletReset()); //적 탄알 제거
        foreach (var e in enemies) //모든 적 대상으로 반복문 실행
        {
            if(e != null)
            {
                if(e.isBoss) e.DamagedEnemy(400); // 폭탄위력만큼 데미지 주기
                else e.DamagedEnemy(e.maxHP); // 최대체력만큼 데미지 주기
            }
        }

    }

   /* public void Boooom2(InputAction.CallbackContext context) //탄알만 제거
    {
        if (!context.performed || GameManager.instance.Bomb <= 0) return;
        //입력이 퍼폼 상태가 아니거나 폭탄갯수가 0보다 작으면 아랫줄 실행 안함
        Instantiate(bombPrefab); //폭탄 이펙트 재생
        GameManager.instance.bombCountChange(-1); //폭탄 갯수 감소
        StartCoroutine(BulletReset());
    }*/
    IEnumerator BulletReset()
    {
        BulletParticle[] bullet = FindObjectsByType<BulletParticle>(FindObjectsSortMode.None); //모든 적 찾기         
        foreach (var b in bullet) //모든 적 대상으로 반복문 실행
        {
            if (b.isEnemy)
            {
                b.ParticlePlay(false);
            }
        }

        yield return new WaitForSeconds(2); //2초 뒤에
        bullet = FindObjectsByType<BulletParticle>(FindObjectsSortMode.None);
        foreach (var b in bullet) //모든 적 대상으로 반복문 실행
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
            //이미 켜져있는 무기는 끄기
            if (i == index) weaponArray[i].SetActive(true);
            //index에 해당하는 무기는 켜기
        }
    }

    public void WeaponOnOff(bool isOn)
    {
        for (int i = 0; i < weaponArray.Length; i++)
        {
            if (isOn) weaponArray[i].GetComponent<ParticleSystem>().Play(); 
            else weaponArray[i].GetComponent<ParticleSystem>().Stop();
            //상위 파티클을 플레이하거나 멈추면 자식으로있는 파티클도 같이 작동한다

        }
    }

}
