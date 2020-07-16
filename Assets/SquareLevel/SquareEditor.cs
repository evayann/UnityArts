using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[CustomEditor(typeof(Square))]
public class SquareEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Square square = (Square) target;
        if (GUILayout.Button("Random Seed"))
        {
            square.m_seed = Random.Range(0, Int32.MaxValue);
        }
        
        if (GUILayout.Button("Destroy"))
        {
            square.Clean();
        }
        
        if (GUILayout.Button("Regenerate"))
        {
            square.Regenerate();
        }
    }
}
