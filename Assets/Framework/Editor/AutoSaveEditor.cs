using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEditor;
using UnityEditor.SceneManagement;

using UnityEngine;

namespace FrameWork
{
    public class AutoSaveEditor : EditorWindow
    {
        private bool autoSaveScene = true;
        private bool autoUndoSave = true;
        private bool isLastSaveTime = true;
        private int intervalScene;
        private int minutes;
        private float tempTime;
        private int UndoIndex;
        private float tempUndo;
        private DateTime lastSaveTime = DateTime.Now;

        private void OnGUI()
        {
            autoSaveScene = EditorGUILayout.BeginToggleGroup("间隔保存", autoSaveScene);
            if (autoSaveScene)
                intervalScene = EditorGUILayout.IntSlider("间隔 (分钟)", intervalScene, 1, 10);
            EditorGUILayout.EndToggleGroup();
            autoUndoSave = EditorGUILayout.BeginToggleGroup("操作保存", autoUndoSave);
            if (autoUndoSave)
                UndoIndex = EditorGUILayout.IntSlider("操作 (次数)", UndoIndex, 5, 20);
            EditorGUILayout.EndToggleGroup();
            minutes = intervalScene * 180;
            if (isLastSaveTime)
            {
                lastSaveTime = DateTime.Now;
                isLastSaveTime = false;
            }
            EditorGUILayout.LabelField("最后保存:", "" + lastSaveTime);
        }

        private void Update()
        {
            if (autoSaveScene)
            {
                tempTime += Time.fixedDeltaTime;
                if (tempTime >= minutes)
                {
                    tempTime = 0;
                    SaveScene();
                }
            }
        }

        private void OnHierarchyChange()
        {
            tempUndo++;
            if (autoSaveScene && tempUndo > UndoIndex)
            {
                SaveScene();
            }
        }

        private void SaveScene()
        {
            tempUndo = 0;
            isLastSaveTime = true;
            if (!EditorApplication.isPlaying)
            {
                EditorSceneManager.SaveOpenScenes();
                AssetDatabase.SaveAssets();
            }
        }
    }
}