using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject m_ObjectPrefab;
    [SerializeField] private int m_PooledAmount = 2;
    [SerializeField] private bool m_WillGrow = false;

    private List<GameObject> m_PooledObjects;

    private void Start()
    {
        m_PooledObjects = new List<GameObject>();
        for (int i = 0; i < m_PooledAmount; i++)
            CreateNewObject();
    }

    private GameObject CreateNewObject()
    {
        GameObject go = (GameObject)Instantiate(m_ObjectPrefab);
        go.SetActive(false);
        m_PooledObjects.Add(go);
        return go;
    }

    public GameObject NextObject()
    {
        for (int i = 0; i < m_PooledAmount; i++)
        {
            if (!m_PooledObjects[i].activeInHierarchy)
                return m_PooledObjects[i];
        }

        if (m_WillGrow)
        {
            m_PooledAmount++;
            return CreateNewObject();
        }

        return null;
    }
}