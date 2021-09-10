using System.Collections.Generic;

using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;
namespace FrameWork
{
    public class InputSearch : EditorWindow
    {
        private string inputInfo = "";
        public enum searchType
        {
            只搜索Text,
            只搜索TMP_Text,
            全搜索
        }
        private searchType type = searchType.只搜索Text;
        private List<GameObject> allSearch = new List<GameObject>();
        private int currentSearch;
        public void OnGUI()
        {
            type = (searchType)EditorGUILayout.EnumPopup("搜索类型", type, GUILayout.Width(400));
            inputInfo = EditorGUI.TextField(new Rect(0, 30, 400, 30), "查找内容", inputInfo);
            if (GUI.Button(new Rect(0, 60, 400, 30), "GO"))
            {
                currentSearch = 0;
                allSearch = new List<GameObject>();
                //Transform [ ] transforms = Selection.transforms;
                Transform[] transforms = new Transform[] { GameObject.Find("Canvas").transform };
                foreach (Transform item in transforms)
                {
                    switch (type)
                    {
                        case searchType.只搜索Text:
                            search(item.GetComponentsInChildren<Text>(true));
                            break;
                        case searchType.只搜索TMP_Text:
                            search(item.GetComponentsInChildren<TMP_Text>(true));
                            break;
                        case searchType.全搜索:
                            search(item.GetComponentsInChildren<Text>(true));
                            search(item.GetComponentsInChildren<TMP_Text>(true));
                            break;
                    }
                }
                if (allSearch.Count >= 1)
                {
                    Selection.activeGameObject = allSearch[0];
                }
            }
            GUILayout.BeginHorizontal();
            if (GUI.Button(new Rect(0, 90, 200, 30), "上一个"))
            {
                currentSearch--;
                if (currentSearch < 0)
                    currentSearch = allSearch.Count - 1;
                Selection.activeGameObject = allSearch[currentSearch];
            }
            if (GUI.Button(new Rect(200, 90, 200, 30), "下一个"))
            {
                currentSearch++;
                if (currentSearch >= allSearch.Count)
                    currentSearch = 0;
                Selection.activeGameObject = allSearch[currentSearch];
            }
            GUILayout.EndHorizontal();

        }

        private void search(Component[] temp)
        {
            foreach (var texts in temp)
            {
                string textstr = "";
                if (texts is Text)
                    textstr = ((Text)texts).text;
                if (texts is TMP_Text)
                    textstr = ((TMP_Text)texts).text;
                string Lower = inputInfo.ToLower();
                if (textstr.Contains(inputInfo) || textstr.ToLower().Contains(Lower))
                {
                    DebugUtil.LogError("",texts.name);
                    allSearch.Add(texts.transform.gameObject);
                }
            }
        }
    }
}