////////////////////////////////////////////////////////////////////////////
// TabWindowControllerEditor
//
// Copyright (C) 2015-2016 Terry K. @ GIX Entertainment Inc. & Quasar Inc.
//
////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;

using Quasar.UI;

[CustomEditor(typeof(TabWindowController), true)]
public class TabWindowControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Show default inspector property editor
        DrawDefault();

        DrawButtonAndViewConnections();
    }

    void DrawDefault()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("activateMode"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultActivationIndex"));
        serializedObject.ApplyModifiedProperties();
    }

    static GUIContent removeButtonContent = new GUIContent("-", "remove");
    static GUILayoutOption miniButtonWidth = GUILayout.Width(20f);

    void DrawButtonAndViewConnections()
    {
        serializedObject.Update();
        {
            SerializedProperty tabViewsProperty = serializedObject.FindProperty("tabWindows");
            SerializedProperty tabbarButtonsProperty = serializedObject.FindProperty("tabButtons");

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Tabs", GUILayout.Width(40));

                DrawAddButton(tabViewsProperty, tabbarButtonsProperty, tabViewsProperty.arraySize);

                DrawResetButton(tabViewsProperty, tabbarButtonsProperty);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel += 1;
            {
                for (int index = 0; index < tabViewsProperty.arraySize; index++)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        DrawTabPropertyField("Root", index, tabViewsProperty);

                        DrawTabPropertyField("Button", index, tabbarButtonsProperty);

                        DrawRemoveButton(tabViewsProperty, tabbarButtonsProperty, index);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUI.indentLevel -= 1;
        }
        serializedObject.ApplyModifiedProperties();
    }

    void DrawResetButton(SerializedProperty tabViewsProperty, SerializedProperty tabbarButtonsProperty)
    {
        if (GUILayout.Button("Reset", GUILayout.Width(60f)))
        {
            tabViewsProperty.ClearArray();
            tabbarButtonsProperty.ClearArray();
        }
    }

    void DrawTabPropertyField(string text, int index, SerializedProperty property)
    {
        GUILayout.Label(text, GUILayout.Width(40));
        GUILayout.FlexibleSpace();
        EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(index), GUIContent.none, true, GUILayout.MinWidth(100));
    }

    void DrawRemoveButton(SerializedProperty tabViewsProperty, SerializedProperty tabbarButtonsProperty, int index)
    {
        if (GUILayout.Button(removeButtonContent, EditorStyles.miniButton, miniButtonWidth))
        {
            tabViewsProperty.DeleteArrayElementAtIndex(index);
            tabbarButtonsProperty.DeleteArrayElementAtIndex(index);
        }
    }

    void DrawAddButton(SerializedProperty tabViewsProperty, SerializedProperty tabbarButtonsProperty, int index)
    {
        if (GUILayout.Button("Add Tab Window Components", GUILayout.Width(200)))
        {
            tabViewsProperty.InsertArrayElementAtIndex(index);
            tabbarButtonsProperty.InsertArrayElementAtIndex(index);
        }
    }
}
