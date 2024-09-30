using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] int _giveDamage;
    private NavMeshAgent m_agent;
    private Animator m_animator;
    private Enemy m_enemy;
    private TakeDamage m_player;
    private bool m_dealingDamage;
    private Coroutine m_coroutine;
    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_enemy = GetComponent<Enemy>();
        m_agent = GetComponent<NavMeshAgent>();
        m_player = FindObjectOfType<TakeDamage>();
        m_dealingDamage = false;
    }

    private void Update()
    {
        if(!m_enemy.GetEnemyStatus())
        {
            return;
        }

        if (m_agent.remainingDistance <= m_agent.stoppingDistance)
        {
            if(!m_dealingDamage)
            {
                m_dealingDamage = true;
                m_coroutine = StartCoroutine(DealDamage());
            }
            m_animator.SetBool("attack", true);
        }
        else
        {
            if(m_coroutine != null)
            {
                m_dealingDamage = false;
                StopCoroutine(m_coroutine);
            }

            m_animator.SetBool("attack", false);
        }
    }

    IEnumerator DealDamage()
    {
        yield return new WaitForSeconds(1.5f);
        m_player.DealtDamage(_giveDamage);
        StartCoroutine(DealDamage());
    }

    public void EnemyDead()
    {
        m_animator.SetBool("attack", false);
        m_animator.SetBool("dead", true);
    }
}
