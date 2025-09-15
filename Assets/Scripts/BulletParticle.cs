using UnityEngine;

public class BulletParticle : MonoBehaviour
{
    public ParticleSystem ps;
    public bool isEnemy;
    public int bulletDamage = 10;
    private void Awake()
    {
        
        ps = GetComponent<ParticleSystem>();
        isEnemy = transform.parent.CompareTag("Enemy");//부모오브젝트 태그 비교
    }

    public void ParticlePlay(bool isPlay)
    {

        if (isPlay)
            ps.Play();
        else
        {
            ps.Stop(); //파티클 재생정지
            ps.Clear(); //이미 생성된 파티클도 제거
        }

    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("Player") && isEnemy)
        {
            other.GetComponent<PlayerControl>().Death();
        }
        if(other.CompareTag("Enemy") && !isEnemy)
        {
            other.GetComponent<Enemy>().DamagedEnemy(bulletDamage);
        }
    }

    
}
