////////////////////////////////////////////////////////////////////////////
// TabWindowButtonEditor
//
// Copyright (C) 2015-2016 Terry K. @ GIX Entertainment Inc. & Quasar Inc.
//
////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;

using Quasar.UI;

[CanEditMultipleObjects]
[CustomEditor(typeof(TabWindowButton), true)]
public class TabWindowButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefault();

        SerializedProperty transitionProperty = serializedObject.FindProperty("transition");
        if (transitionProperty.enumValueIndex == 1) // Color tint
        {
            DrawColorSettings();
        }
        else if (transitionProperty.enumValueIndex == 2)    // Sprite swap
        {
            DrawSpriteSettings();
        }
    }

    void DrawDefault()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("background"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("title"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("transition"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("interactable"));
        serializedObject.ApplyModifiedProperties();
    }

    void DrawColorSettings()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("normalBackgroundColor"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("selectedBackgroundColor"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("normalTextColor"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("selectedTextColor"));
        serializedObject.ApplyModifiedProperties();
    }

    void DrawSpriteSettings()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("normalSprite"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("selectedSprite"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("normalBackgroundColor"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("selectedBackgroundColor"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("normalTextColor"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("selectedTextColor"));
        serializedObject.ApplyModifiedProperties();
    }
}
