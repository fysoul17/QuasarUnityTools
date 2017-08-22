////////////////////////////////////////////////////////////////////////////
// LoadingIndicatorEditor
//
// Copyright (C) 2015 Terry K. @ GIX Entertainment Inc. & Quasar Inc.
//
////////////////////////////////////////////////////////////////////////////

using UnityEditor;

using Quasar.UI;

[CustomEditor(typeof(LoadingIndicator), true)]
public class LoadingIndicatorEditor : Editor
{
    LoadingIndicator loadingIndicatorScript;

    void OnEnable()
    {
        loadingIndicatorScript = target as LoadingIndicator;
    }

    public override void OnInspectorGUI()
    {
        // Show default inspector property editor
        DrawDefaultInspector();

        if (loadingIndicatorScript.animationMode == LoadingIndicatorMode.Rotation)
        {
            loadingIndicatorScript.rotateClockwise = EditorGUILayout.Toggle("Rotate Clockwise", loadingIndicatorScript.rotateClockwise);
        }
        else if (loadingIndicatorScript.animationMode == LoadingIndicatorMode.ImageSwap)
        {
            loadingIndicatorScript.swapDelay = EditorGUILayout.FloatField("Swap Delay", loadingIndicatorScript.swapDelay);
        }
    }
}