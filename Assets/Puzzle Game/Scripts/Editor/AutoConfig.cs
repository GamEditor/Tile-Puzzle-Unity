using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(LevelInformation))]
[CanEditMultipleObjects]

public class AutoConfig : Editor
{
    private LevelInformation m_InformationScript;
    private LevelInformation[] m_References;

    private int m_TempID;

    private void Awake()
    {
        m_References = FindObjectsOfType<LevelInformation>();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        m_InformationScript = (LevelInformation)target;
        GUILayout.BeginVertical();

        m_TempID = EditorGUILayout.IntField("ID", m_InformationScript.m_LevelID);

        for (int i = 0; i < m_References.Length; i++)
        {
            if(m_TempID != m_InformationScript.m_LevelID)
                if (m_References[i].m_LevelID == m_TempID)
                {
                    m_TempID = 0;
                    break;
                }
        }

        if (m_TempID > 0)
            m_InformationScript.m_LevelID = m_TempID;
        else
            Debug.LogWarning("This ID is already taken! Please try another value.");

        GUILayout.EndVertical();
    }
}