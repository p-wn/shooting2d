using UnityEngine;

public class BulletParticle : MonoBehaviour
{
    public ParticleSystem ps;
    public bool isEnemy;
    public int bulletDamage = 10;
    private void Awake()
    {
        
        ps = GetComponent<ParticleSystem>();
        isEnemy = transform.parent.CompareTag("Enemy");//�θ������Ʈ �±� ��
    }

    public void ParticlePlay(bool isPlay)
    {

        if (isPlay)
            ps.Play();
        else
        {
            ps.Stop(); //��ƼŬ �������
            ps.Clear(); //�̹� ������ ��ƼŬ�� ����
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
