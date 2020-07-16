using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundResize : MonoBehaviour 
{
    public static BackgroundResize Instance;

    public Camera m_camera;
    public GameObject m_fixBackgroundContainer;
    
    public float m_originalScreenRatio = 1080f / 1920f;
    public Vector2 m_originalBGScale = new Vector2 (9f, 16f);
    public float m_originalCamSize = 8f;
    
    private void LateUpdate()
    {
        ComputeResize();
    }

    void ComputeResize()
    {
        if (m_camera == null) return;
        ResizeIso();
    }

    void ResizeIso()
    {
        float currentScreenRatio = (float) Screen.width / (float) Screen.height;
        float ratio = currentScreenRatio / m_originalScreenRatio;

        float currentCamSize = m_camera.orthographicSize;
        float camSizeRatio = currentCamSize / m_originalCamSize;

        // Background container
        float xScale = camSizeRatio * ratio * m_originalBGScale[0];
        float yScale = camSizeRatio * m_originalBGScale[1];
        float zScale = 1f;
        Vector3 scale = new Vector3(xScale, yScale, zScale);
        m_fixBackgroundContainer.transform.localScale = scale;
    }
}