using System;
using System.Collections.Generic;
using UnityEngine;

public enum RotateType
{
    NORMAL,
    ROUND
}

public class Polygon: MonoBehaviour
{
    
    [Header("Camera")]
    public Camera m_camera;

    [Header("Parameters")]
    [Range(3, 15)]   
    public int        m_nbArret = 3;
    public int        m_nbElOnSegment = 5;
    [Range(0, 100)]
    public float      m_size = 2;
    public float      m_rotatePerSecond = 45;

    [Header("Prefab")]
    public GameObject m_element;
    
    private float            m_rotation = 0;
    private GameObject[]     m_coins; // List of each coins fo polygon
    private List<GameObject> m_elements;
    
    // Start is called before the first frame update
    void Start()
    {
        // Initialize values 
        m_coins    = new GameObject[m_nbArret];
        m_elements = new List<GameObject>();
        m_camera.orthographicSize = m_size * 1.5f;
        
        m_nbElOnSegment -= 1; // Need to remove coins of segment
        
        // Create segment with circle
        float part = ((2 * Mathf.PI) / m_nbArret);
        for (int i = 0; i < m_nbArret; i++)
        {
            float trigo = part * i;
            m_coins[i] = CreateCircle(Mathf.Cos(trigo) * m_size,  Mathf.Sin(trigo) * m_size);
        }
        
        // Make segment
        Vector3 prevCoin = m_coins[0].transform.position;
        for (int i = 1; i <= m_nbArret; i++)
        {
            Vector3 curr = m_coins[i % m_nbArret].transform.position;
            Vector3 diff = (prevCoin - curr) / m_nbElOnSegment;
            
            m_elements.Add(m_coins[i - 1]);
            
            // Don't do first and last because already existant
            for (int j = 1; j < m_nbElOnSegment; j++)
            {
                Vector3 pos = curr + (diff * j);
                GameObject go = CreateCircle(pos.x, pos.z);
                m_elements.Add(go);
            }
            prevCoin = curr;
        }
    }

    private GameObject CreateCircle(float _x, float _z)
    {
        GameObject go = Instantiate(m_element,
            new Vector3(_x, 0, _z),
            m_element.transform.rotation,
            transform);
       
        go.transform.localScale *= m_size;
        TrailRenderer tr = go.GetComponent<TrailRenderer>();
        if (tr) tr.widthMultiplier = m_size;
        
        return go;
    }
    
    // Update is called once per frame
    void Update()
    {
        // Make rotation
        m_rotation += m_rotatePerSecond * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, m_rotation, 0);
    }
}