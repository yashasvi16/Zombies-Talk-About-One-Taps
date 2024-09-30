using System.Collections.Generic;
using UnityEngine;

public class EnemyPooling : MonoBehaviour
{
    [Header("Enemy Types")]
    [SerializeField]
    GameObject _easyEnemy;
    [SerializeField]
    GameObject _mediumEnemy;
    [SerializeField]
    GameObject _hardEnemy;

    [Header("Pool Parents")]
    [SerializeField]
    Transform _easyEnemyPoolParent;
    [SerializeField]
    Transform _mediumEnemyPoolParent;
    [SerializeField]
    Transform _hardEnemyPoolParent;

    private List<GameObject> m_easyEnemyPool;
    private List<GameObject> m_mediumEnemyPool;
    private List<GameObject> m_hardEnemyPool;

    private void Start()
    {
        m_easyEnemyPool = new List<GameObject>();
        m_mediumEnemyPool = new List<GameObject>();
        m_hardEnemyPool = new List<GameObject>();

        for (int i = 0; i < _easyEnemyPoolParent.childCount; i++)
        {
            m_easyEnemyPool.Add(_easyEnemyPoolParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < _mediumEnemyPoolParent.childCount; i++)
        {
            m_mediumEnemyPool.Add(_mediumEnemyPoolParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < _hardEnemyPoolParent.childCount; i++)
        {
            m_hardEnemyPool.Add(_hardEnemyPoolParent.GetChild(i).gameObject);
        }
    }

    public GameObject GetPooledEnemy(int indexOfEnemyType)
    {
        List<GameObject> duplicateEnemyPool = new List<GameObject>();
        GameObject duplicateEnemy = null;

        switch (indexOfEnemyType)
        {
            case 0:
                duplicateEnemyPool = m_easyEnemyPool;
                duplicateEnemy = _easyEnemy;
                break;
            case 1:
                duplicateEnemyPool = m_mediumEnemyPool;
                duplicateEnemy = _mediumEnemy;
                break;
            case 2:
                duplicateEnemyPool = m_hardEnemyPool;
                duplicateEnemy = _hardEnemy;
                break;
        }

        foreach (var enemy in duplicateEnemyPool)
        {
            if (!enemy.activeInHierarchy)
            {
                return enemy;
            }
        }

        GameObject newEnemy = Instantiate(duplicateEnemy);
        newEnemy.SetActive(false);

        switch (indexOfEnemyType)
        {
            case 0:
                m_easyEnemyPool.Add(newEnemy);
                break;
            case 1:
                m_mediumEnemyPool.Add(newEnemy);
                break;
            case 2:
                m_hardEnemyPool.Add(newEnemy);
                break;
        }

        return newEnemy;
    }

}
