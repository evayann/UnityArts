using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Serialization;

public class Animation : MonoBehaviour
{

    [Header("Camera")]
    public Camera m_camera;
    public bool   m_activeRotation = false;
    [Tooltip("Rotate angle by seconds")]
    public float  m_rotateSpeed    = 45f;
    
    [Header("Parameters")]
    [Header("Size")]
    [Range(10, 200)]
    public int     m_width          = 30;
    [Range(10, 200)]
    public int     m_height         = 30;
    public Vector3 m_waveCenter;
    
    [Header("Waves")]
    [Range(0, 2)]
    public float m_wavesIntensity = 1;
    [Range(0, 15)]
    public float m_speed          = 1;

    [Header("Colors")] 
    public bool     m_useColor = false;
    public Gradient m_color;

    [Header("Prefab")]
    public GameObject m_rectangle;

    private GameObject[,] m_grids;

    void Start()
    {
        float max = Mathf.Max(m_width, m_height);
        float halfMax = max / 2f;
        m_camera.orthographicSize = max;
        m_camera.transform.position = new Vector3(-halfMax, halfMax, -halfMax);
        CreateGrid();
    }

    void Update()
    {
        UpdateWaves();
        UpdateCamera();
    }

    private void UpdateWaves()
    {
        foreach (GameObject go in m_grids)
        {
            float distance = Vector3.Distance(go.transform.position, m_waveCenter);
            go.transform.localScale = new Vector3(1, 
                Remap(-1, 1, 
                    1, 10, 
                    Mathf.Cos(distance * m_wavesIntensity + Time.time * m_speed)),
                1);

            if (m_useColor)
            {
                Color color = m_color.Evaluate(Remap(1, 10, 0, 1, go.transform.localScale.y));
                go.GetComponent<Renderer>().material.color = color;
            }
        }
    }

    private void UpdateCamera()
    {
        if (m_activeRotation)
            m_camera.transform.RotateAround(Vector3.zero, Vector3.up, m_rotateSpeed * Time.deltaTime);
    }

    private void CreateGrid()
    {
        m_grids = new GameObject[m_width, m_height];
        int halfWidth  = m_width / 2, halfHeight = m_height / 2;
        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++)
            {
                m_grids[i, j] = CreateCube(i - halfWidth, j - halfHeight);
            }
        }
    }
    
    private GameObject CreateCube(int _x, int _z)
    {
        return Instantiate(m_rectangle,
            new Vector3(_x, 0, _z),
            m_rectangle.transform.rotation,
            transform);
    }

    private float Remap(float iMin, float iMax, float oMin, float oMax, float value)
    {
        return Mathf.Lerp(oMin, oMax, Mathf.InverseLerp(iMin, iMax, value));
    }

}
