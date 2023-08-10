using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MinMaxFilter))]
public class MinMaxFilterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (Application.isPlaying)
        {
            MinMaxFilter filter = (MinMaxFilter) target;
            if (filter.voxelVar != null)
            {
                EditorGUILayout.LabelField("Min value: " + filter.minValue);
                EditorGUILayout.LabelField("Max value: " + filter.maxValue);
                EditorGUILayout.MinMaxSlider(ref filter.minValue, ref filter.maxValue, filter.voxelVar.Range.min, filter.voxelVar.Range.max);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(filter.voxelVar.Range.min.ToString());
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField(filter.voxelVar.Range.max.ToString());
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}