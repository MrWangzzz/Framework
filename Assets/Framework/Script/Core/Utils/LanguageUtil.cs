using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;

using Excel;

using TMPro;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public static class LanguageUtil
{
    internal static void LoadPic(this Image image, string name)
    {
        image.sprite = Resources.Load<Sprite>("Language/" + GetLanguage + "/" + name);
    }

    internal static void LoadStr(this Text text, string key, params object[] pars)
    {
        text.text = string.Format(GetLanguageDic[key], pars);
    }

    internal static void LoadStr(this TMP_Text text, string key, params object[] pars)
    {
        text.text = string.Format(GetLanguageDic[key], pars);
    }

    internal static SystemLanguage GetLanguage
    {
        get
        {
            SystemLanguage language = Application.systemLanguage;
            return language;
        }
    }

    public static DataSet mResultSet { get; private set; }

    private static Dictionary<string, string> GetLanguageDic = new Dictionary<string, string>();

    internal static IEnumerator InitData()
    {
#if UNITY_ANDROID || UNITY_IOS
        GetLanguageDic = new Dictionary<string, string>();
        string fileName = "/language.xlsx";
        string strpath = Application.streamingAssetsPath;
        string perpath = Application.persistentDataPath;

        if (!File.Exists(perpath + fileName))
        {
            UnityWebRequest webRequest = new UnityWebRequest(strpath + fileName);
            yield return webRequest;
            if (webRequest.error != null && !webRequest.isHttpError)
            {
                File.WriteAllBytes(perpath + fileName, webRequest.downloadHandler.data);
                ReadData();
            }
        }
        else
            ReadData();
#elif UNITY_EDITOR
        yield return new WaitForFixedUpdate();
        ReadData();
#endif


    }

    private static void ReadData()
    {
        FileStream mStream = new FileStream(Application.persistentDataPath + "/language.xlsx", FileMode.Open, FileAccess.Read);
        IExcelDataReader mExcelReader = ExcelReaderFactory.CreateOpenXmlReader(mStream);
        mResultSet = mExcelReader.AsDataSet();

        DataTable mSheet = mResultSet.Tables[0];

        int languageColums = 0;
        for (int j = 0; j < mSheet.Columns.Count; j++)
        {
            if (mSheet.Rows[0][j].ToString() == GetLanguage.ToString())
            {
                languageColums = j;
                break;
            }
            else
                languageColums = 0;
        }
        for (int i = 0; i < mSheet.Rows.Count; i++)
        {
            if (!GetLanguageDic.ContainsKey(mSheet.Rows[i][0].ToString()))
            {
                string value = mSheet.Rows[i][languageColums].ToString();
                value = value.Contains("#^#") ? value.Replace("#^#", "\r\n") : value;
                GetLanguageDic.Add(mSheet.Rows[i][0].ToString(), value);
            }
        }
    }
}
