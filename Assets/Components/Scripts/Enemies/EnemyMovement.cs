using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Ease,
    Medium,
    Hard
}

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float _minWalkSpeed, _maxWalkSpeed;
    [SerializeField] float _minRunSpeed, _maxRunSpeed;

    public EnemyType _enemyType;
    private GameObject m_player;
    private NavMeshAgent m_agent;
    private Enemy m_enemy;
    private PlayerMovement m_playerMovement;

    private void OnEnable()
    {
        if(m_agent != null)
        {
            TriggerNPC();
        }
    }

    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_player = GameManager.Instance._player;
        m_enemy = GetComponent<Enemy>();
        m_playerMovement = m_player.GetComponent<PlayerMovement>();

        switch(_enemyType)
        {
            case EnemyType.Ease: 
                m_agent.speed = Random.Range(_minWalkSpeed, _maxWalkSpeed);
                break;
            case EnemyType.Medium: 
                m_agent.speed = Random.Range (_maxWalkSpeed, (_minRunSpeed + _maxRunSpeed) / 2);
                gameObject.transform.localScale = Vector3.one;
                break;
            case EnemyType.Hard: 
                m_agent.speed = Random.Range(_minRunSpeed, _maxRunSpeed);
                break;
        }

        TriggerNPC();
    }

    void Update()
    {
        if(!m_enemy.GetEnemyStatus())
        {
            return;
        }

        if(m_playerMovement._playerMoving)
        {
            TriggerNPC();
        }

        FaceTowardPlayer();
    }

    void TriggerNPC()
    {
        m_agent.SetDestination(m_player.transform.position);
    }

    void FaceTowardPlayer()
    {
        transform.forward = m_agent.steeringTarget - transform.position;
    }

    public bool NPCMovementStatus(bool status)
    {
        return status;
    }
}
