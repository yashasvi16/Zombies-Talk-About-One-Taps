using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private bool m_enemyStatus;

    private Animator m_animator;
    private NavMeshAgent m_agent;

    private void OnEnable()
    {
        PutEnemyStatus(true);
    }

    public void DeadEnemy()
    {
        PutEnemyStatus(false);

        m_agent = GetComponent<NavMeshAgent>();
        m_agent.isStopped = true;

        GetComponent<EnemyAnimation>().EnemyDead();

        Invoke(nameof(DeSpawn), 2f);
    }

    private void DeSpawn()
    {
        gameObject.SetActive(false);
    }

    public bool GetEnemyStatus()
    {
        return m_enemyStatus;
    }

    public void PutEnemyStatus(bool status)
    {
        m_enemyStatus = status;
    }
}
