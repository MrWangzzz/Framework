/********************************************************************
	purpose:	打包工具面板
*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using UnityEditor;
using UnityEditor.Build.Reporting;

using UnityEngine;
namespace FrameWork
{
    public class BuildTools : EditorWindow
    {
        private bool isRelease
        {
            get { return bool.Parse(PlayerPrefs.GetString("IsRelease", "false")); }
            set { PlayerPrefs.SetString("IsRelease", value.ToString()); }
        }

        private bool isSetting;
        private bool isInitCode;
        private bool isOpenUpdateInfo;
        private string strCompanyName;
        private string strProductName;
        private string strApplicationIdentifier;
        private string strName;
        private string keyPass;
        private string MacroStr;
        private string updateInfo;
        private string buildPath { get { return Application.dataPath + "/../Build/"; } }
        private Vector2 scroll=Vector2.zero;

        private enum VersionType
        {
            One,
            Two,
            Three,
        }
        private VersionType type = VersionType.Three;

        private List<EditorBuildSettingsScene> levels = new List<EditorBuildSettingsScene>();
        private Color[] GetColors
        {
            get
            {
                return new Color[] { new Color(0, 0.75f, 1, 1), new Color(1, 1, 0.5f, 1), new Color() };
            }
        }
        private void OnEnable()
        {
            SetPass();
        }

        #region 设置密码
        public void SetPass()
        {
            if (PlayerSettings.Android.useCustomKeystore)
            {
                keyPass = PlayerSettings.Android.keyaliasName.Replace(".keystore", "");
                DebugUtil.Log("keyPass", keyPass);
                PlayerSettings.Android.keystorePass = keyPass;
                PlayerSettings.Android.keyaliasPass = keyPass;
            }
        }
        #endregion

        #region 设置打包平台
        private BuildTargetGroup TargetGroup
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsPlayer: return BuildTargetGroup.Standalone;
                    case RuntimePlatform.IPhonePlayer: return BuildTargetGroup.iOS;
                    case RuntimePlatform.Android: return BuildTargetGroup.Android;
                }
                return BuildTargetGroup.Android;
            }
        }
        #endregion

        private BuildTarget Target
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsPlayer: return BuildTarget.StandaloneWindows;
                    case RuntimePlatform.IPhonePlayer: return BuildTarget.iOS;
                    case RuntimePlatform.Android: return BuildTarget.Android;
                }
                return BuildTarget.Android;
            }
        }

        private void OnGUI()
        {
            GUI.backgroundColor = GetColors[0];
            GUI.contentColor = GetColors[1];
            MacroStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(TargetGroup);
            GUILayout.Label("包名：" + PlayerSettings.applicationIdentifier);
            GUILayout.Label("产品：" + PlayerSettings.productName);
            GUILayout.Label("版本：" + PlayerSettings.bundleVersion);
            GUILayout.Label("Code：" + PlayerSettings.Android.bundleVersionCode);
            GUILayout.Label("宏：" + MacroStr);
            isRelease = EditorGUILayout.Toggle("正式包", isRelease);
            if (isRelease && MacroStr.Contains("DEBUG_LOG"))
                PlayerSettings.SetScriptingDefineSymbolsForGroup(TargetGroup, MacroStr.Replace("DEBUG_LOG", ""));
            if (!isRelease)
            {
                if (!MacroStr.Contains("DEBUG_LOG"))
                {
                    string temp = "DEBUG_LOG;" + MacroStr;
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(TargetGroup, temp);
                }
            }
            type = (VersionType)EditorGUILayout.EnumPopup("升级版本类型", type);
            if (GUILayout.Button("设置初始物料"))
                isSetting = !isSetting;
            strCompanyName = isSetting ? EditorGUILayout.TextField("公司名", strCompanyName) : PlayerSettings.companyName;
            strProductName = isSetting ? EditorGUILayout.TextField("产品名", strProductName) : PlayerSettings.productName;
            strApplicationIdentifier = isSetting ? EditorGUILayout.TextField("包名", strApplicationIdentifier) : PlayerSettings.applicationIdentifier;

            if (isSetting)
            {
                isInitCode = EditorGUILayout.Toggle("初始化Code", isInitCode);
                if (GUILayout.Button("保存物料"))
                {
                    PlayerSettings.companyName = strCompanyName;
                    PlayerSettings.productName = strProductName;
                    PlayerSettings.applicationIdentifier = strApplicationIdentifier;
                    if (isInitCode)
                    {
                        PlayerSettings.bundleVersion = "1.0.0";
                        PlayerSettings.Android.bundleVersionCode = 100;
                    }
                }
            }
            GUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.white;
            GUI.contentColor = Color.cyan;
            if (GUILayout.Button("版本号 ↑↑↑"))
                UpVersion(true);
            GUI.contentColor = Color.green;
            if (GUILayout.Button("版本号 ↓↓↓"))
                UpVersion(false);
            GUILayout.EndHorizontal();
            GUI.backgroundColor = GetColors[0];
            GUI.contentColor = GetColors[1];
            if (GUILayout.Button("打开发布文件夹"))
                Application.OpenURL(buildPath);
            strName = EditorGUILayout.TextField("后缀名", strName);
            GUI.backgroundColor = Color.green;
            GUI.contentColor = Color.red;
            GUIStyle gUIStyle = new GUIStyle(GUI.skin.button);
            gUIStyle.fixedHeight = 40;
            gUIStyle.fontSize = 30;
            gUIStyle.fontStyle = FontStyle.Bold;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("打     包", gUIStyle))
                StartBuild();
            if (GUILayout.Button("运     行", gUIStyle))
                StartBuild(true);
            GUILayout.EndHorizontal();
            GUI.backgroundColor = GetColors[0];
            GUI.contentColor = GetColors[1];
            isOpenUpdateInfo = EditorGUILayout.Toggle("打开输入更新内容", isOpenUpdateInfo);
            if (isOpenUpdateInfo)
            {
                GUILayout.BeginScrollView(scroll);
                GUILayout.BeginHorizontal();
                GUILayout.Label("更新内容:");
                if (GUILayout.Button("导入上版本更新内容"))
                {
                    ArrayList list = ReadFile();
                    for (int i = 0; i < list.Count; i++)
                    {
                        updateInfo += $"{list[i]}\r\n";
                    }
                }
                GUILayout.EndHorizontal();
                updateInfo = EditorGUILayout.TextArea("", GUILayout.Height(200));
                GUILayout.EndScrollView();
            }
        }

        private void UpVersion(bool isadd)
        {
            string[] strVersions = PlayerSettings.bundleVersion.Split('.');
            string version = "";
            switch (type)
            {
                case VersionType.One:
                    version = (int.Parse(strVersions[0]) + (isadd ? 1 : -1)) + ".0.0";
                    break;
                case VersionType.Two:
                    version = strVersions[0] + "." + (int.Parse(strVersions[1]) + (isadd ? 1 : -1)) + ".0";
                    break;
                case VersionType.Three:
                    version = strVersions[0] + "." + strVersions[1] + "." + (int.Parse(strVersions[2]) + (isadd ? 1 : -1));
                    break;
            }
            PlayerSettings.bundleVersion = version;
            PlayerSettings.Android.bundleVersionCode = int.Parse(version.Replace(".", ""));
        }

        private void StartBuild(bool isPlay = false)
        {
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (!scene.enabled) continue;
                levels.Add(scene);
            }

            if (Directory.Exists(buildPath))
                Directory.CreateDirectory(buildPath);
            string APKName = buildPath + strProductName + (isRelease ? "_release_" : "_Debug_") + DateTime.Now.ToString("MMdd_HHmm") + "_"
                + PlayerSettings.Android.bundleVersionCode + "_" + PlayerSettings.bundleVersion +
                (string.IsNullOrEmpty(strName) ? "" : "_" + strName) + ".apk";
            BuildReport report = BuildPipeline.BuildPlayer(levels.ToArray(), APKName, Target, BuildOptions.CompressWithLz4);
            if (report.summary.result == BuildResult.Succeeded)
            {
                if (isOpenUpdateInfo)
                    CreateUpdateInfo();
                Application.OpenURL(buildPath);
                if (isPlay)
                {
                    Process proc = new Process();
                    proc.StartInfo.FileName = APKName;
                    proc.Start();
                }
            }
        }

        private void CreateUpdateInfo()
        {
            FileStream fileStream = new FileStream($"{buildPath}/{PlayerSettings.bundleVersion}_更新内容.txt", FileMode.Create, FileAccess.Write);
            if (!string.IsNullOrEmpty(updateInfo))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(updateInfo);
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
            }
        }

        private int GetLastCode(int curCode)
        {
            if (curCode <= 100)
                return -1;
            if (!File.Exists($"{buildPath}/{curCode}_更新内容.txt"))
                return GetLastCode(curCode - 1);
            return curCode;
        }


        public ArrayList ReadFile()
        {
            StreamReader streamReader;
            int tempcode = GetLastCode(PlayerSettings.Android.bundleVersionCode);
            if (tempcode == -1)
            {
                tempcode = PlayerSettings.Android.bundleVersionCode;
                DebugUtil.LogError("Tips", $"{tempcode}未找到！");
            }
            string path = $"{buildPath}/{tempcode}_更新内容.txt";
            streamReader = File.Exists(path) ? File.OpenText(path) : null;
            ArrayList arrayList = new ArrayList();
            if (streamReader != null)
            {
                string value;
                while ((value = streamReader.ReadLine()) != null)
                {
                    arrayList.Add(value);
                }
                streamReader.Dispose();
            }
            return arrayList;
        }

    }
}