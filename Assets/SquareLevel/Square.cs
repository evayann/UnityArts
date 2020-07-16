using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Param
{
    RANDOM,
    PERLIN_NOISE
}

public class Square : MonoBehaviour
{

    [Header("Camera")] 
    public Camera m_camera;
    public float m_rotationBySeconds = 90;

    [Header("Parameters")] 
    public int   m_seed;
    public Param m_parameters = Param.RANDOM;
    public int   m_width      = 20; 
    public int   m_height     = 20;

    [Header("Random")]
    [Tooltip("Min and max value for hills height")]
    public Vector2 m_range = new Vector2(1, 20);

    [Header("Perlin Noise")] 
    public  float m_factor;
    private int   m_pnOfsset = 0;

    [Header("Prefab")] 
    public GameObject m_element;

    private GameObject[,] m_elements;
    
    // Start is called before the first frame update
    void Start()
    {
        m_elements = new GameObject[m_width, m_height];
        Generate();
    }
    
    void Update()
    {
        m_camera.transform.RotateAround(new Vector3(m_width / 2f, 0, m_height / 2f),
            Vector3.up, m_rotationBySeconds * Time.deltaTime);
    }

    public void Regenerate()
    {
        Clean();
        Start();
    }
    
    void Generate()
    {
        Random.InitState(m_seed);
        switch (m_parameters)
        {
            case Param.RANDOM:
                m_camera.orthographicSize = m_range.x + m_range.y;
                m_camera.transform.position += new Vector3(0,  (m_range.x + m_range.y) / 2f, 0);
                GenerateGrid(RandomGrid);
                break;
            case Param.PERLIN_NOISE:
                m_camera.orthographicSize = m_factor;
                m_camera.transform.position += new Vector3(0, m_factor / 2f, 0);
                m_pnOfsset = Random.Range(0, 10000);
                GenerateGrid(PerlinNoiseGrid);
                break;
            default:
                print("Parameters : " + m_parameters + " isn't supported.");
                break;
        }
    }
    
    private void RandomGrid(GameObject _go)
    {
        _go.transform.localScale = new Vector3(1, Random.Range(m_range.x, m_range.y), 1);
    }

    private void PerlinNoiseGrid(GameObject _go)
    {
        Vector3 pos = _go.transform.position;
        float x = pos.x + m_pnOfsset, y = pos.z + m_pnOfsset; // Correspond axe Z to Y and inflate each value by 10.
        _go.transform.localScale = new Vector3(1,
            Mathf.PerlinNoise(x / m_width , y / m_height) * m_factor ,
            1);
    }

    private void GenerateGrid(Action<GameObject> function)
    {
        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++)
            {
                GameObject go = CreateHill(i, j);
                function(go);
                m_elements[i, j] = go;
            }
        }
    }
    
    private GameObject CreateHill(int _x, int _z)
    {
        return Instantiate(m_element,
            new Vector3(_x, 0, _z),
            m_element.transform.rotation,
            transform);
    }

    public void Clean()
    {
        if (m_elements != null)
            foreach (GameObject go in m_elements)
            {
                DestroyImmediate(go); // DestroyImmediate for Editor
            }
    }
}
