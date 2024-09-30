using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimation : MonoBehaviour
{
    private NavMeshAgent m_agent;
    private Animator m_animator;
    private Enemy m_enemy;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_enemy = GetComponent<Enemy>();
        m_agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(!m_enemy.GetEnemyStatus())
        {
            return;
        }

        if (m_agent.remainingDistance <= m_agent.stoppingDistance)
        {
            m_animator.SetBool("attack", true);
        }
        else
        {
            m_animator.SetBool("attack", false);
        }
    }

    public void EnemyDead()
    {
        m_animator.SetBool("attack", false);
        m_animator.SetBool("dead", true);
    }
}
