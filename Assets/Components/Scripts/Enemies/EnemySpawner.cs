using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    EnemyPooling _pool;

    [Header("Transforms")]
    [SerializeField]
    List<Transform> _enemySpawnLocations;

    [Header("Settings")]
    [SerializeField]
    float[] _easySpawnTime = new float[2];
    [SerializeField]
    float[] _mediumSpawnTime = new float[2];
    [SerializeField]
    float[] _hardSpawnTime = new float[2];

    private List<Transform> m_enemySpawnLocations;

    private void Start()
    {
        m_enemySpawnLocations = new List<Transform>();
        SpawnEasyEnemy();
        SpawnMediumEnemy();
        SpawnHardEnemy();
        ReAssignSpawnLocations();
    }

    public void SpawnEasyEnemy()
    {
        Invoke(nameof(_SpawnEasyEnemy), Random.Range(_easySpawnTime[0], _easySpawnTime[1]));
    }

    private void _SpawnEasyEnemy()
    {
        if (!GameManager.Instance.GetGameStatus())
            return;

        GameObject easyEnemy = _pool.GetPooledEnemy(0);

        Vector3 spawnTransform = GetSpawnPosition();
        easyEnemy.transform.position = spawnTransform;

        easyEnemy.SetActive(true);
        SpawnEasyEnemy();
    }
    
    public void SpawnHardEnemy()
    {
        Invoke(nameof(_SpawnHardEnemy), Random.Range(_hardSpawnTime[0], _hardSpawnTime[1]));
    }

    private void _SpawnHardEnemy()
    {
        if (!GameManager.Instance.GetGameStatus())
            return;

        GameObject hardEnemy = _pool.GetPooledEnemy(2);

        Vector3 spawnTransform = GetSpawnPosition();
        hardEnemy.transform.position = spawnTransform;

        hardEnemy.SetActive(true);
        SpawnHardEnemy();
    }
    public void SpawnMediumEnemy()
    {
        Invoke(nameof(_SpawnMediumEnemy), Random.Range(_mediumSpawnTime[0], _mediumSpawnTime[1]));
    }

    private void _SpawnMediumEnemy()
    {
        if (!GameManager.Instance.GetGameStatus())
            return;

        GameObject mediumEnemy = _pool.GetPooledEnemy(1);

        Vector3 spawnTransform = GetSpawnPosition();
        mediumEnemy.transform.position = spawnTransform;

        mediumEnemy.SetActive(true);
        SpawnHardEnemy();
    }

    public Vector3 GetSpawnPosition()
    {
        if(m_enemySpawnLocations.Count > 0)
        {
            int result = Random.Range(0, m_enemySpawnLocations.Count);
            Vector3 resultPosition = m_enemySpawnLocations[result].position;
            m_enemySpawnLocations.Remove(m_enemySpawnLocations[result]);

            if(m_enemySpawnLocations.Count <= 0)
            {
                ReAssignSpawnLocations();
            }

            return resultPosition;
        }

        return Vector3.zero;
    }

    private void ReAssignSpawnLocations()
    {
        foreach (var item in _enemySpawnLocations)
        {
            m_enemySpawnLocations.Add(item);
        }
    }
}
