using System;
using System.Collections;
using System.IO;
using System.Text;

using UnityEngine;
namespace FrameWork
{
    public class IOperate : Singleton<IOperate>
    {
        internal string GetPath
        {
            get
            {
#if UNITY_EDITOR
                return Application.dataPath;
#elif UNITY_ANDROID || UNITY_IOS
                return Application.persistentDataPath;
#endif
            }
        }

        private string path;

        internal bool isExistsFile(string _name)
        {
            return File.Exists(path + "//" + _name);
        }

        #region 创建文件
        /// <summary>创建文件</summary>
        /// <param name="path">路径</param>
        /// <param name="name">文件名</param>
        /// <param name="Data">数据</param>
        public void CreateFile(string Data, string name = "", string _path = "", bool isAddAss = true)
        {
            try
            {
                path = (isAddAss ? GetPath : "") + _path;
                if (name != "")
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }
                FileStream fs = new FileStream(name == "" ? path : path + "//" + name, FileMode.Create, FileAccess.Write);
                byte[] bs = Encoding.UTF8.GetBytes(Data);
                fs.Write(bs, 0, bs.Length);
                fs.Close();
            }
            catch (Exception ex)
            {
                DebugUtil.Log("Tip", ex.Message);
            }
        }
        #endregion

        #region 读取文件
        /// <summary>读取文件</summary>
        /// <param name="path">路径</param>
        /// <param name="name">名称</param>
        public ArrayList ReadFile(string name = "", string _path = "", bool isAddAss = true)
        {
            StreamReader sr;
            path = (isAddAss ? GetPath : "") + _path;
            try
            {
                sr = File.OpenText(name == "" ? path : path + "//" + name);
            }
            catch (Exception ex)
            {
                DebugUtil.Log("Tip", ex.Message);
                return null;
            }
            string line;
            ArrayList al = new ArrayList();
            while ((line = sr.ReadLine()) != null)
            {
                al.Add(line);
            }

            sr.Dispose();
            return al;
        }
        /// <summary>
        /// 读取resources文件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ArrayList ResourcesFile(string name)
        {
            TextAsset text = Resources.Load(name) as TextAsset;
            ArrayList al = new ArrayList();
            string[] temp = text.ToString().Replace("\r\n", "!").Split('!');
            for (int i = 0; i < temp.Length; i++)
            {
                al.Add(temp[i]);
            }
            return al;
        }
        #endregion

        #region 替换文件中内容
        /// <summary>
        /// 替换文件中内容
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="oldstr">旧字符</param>
        /// <param name="newstr">新字符</param>
        /// <param name="_path">路径</param>
        public void ReplaceFile(string name, string oldstr, string newstr, string _path = "", bool isAddAss = true)
        {
            string con;
            path = (isAddAss ? GetPath : "") + _path;
            FileStream fs = new FileStream(name == "" ? path : path + "//" + name, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            con = sr.ReadToEnd();
            con = con.Replace(oldstr, newstr);
            sr.Close();
            fs.Close();
            FileStream fs2 = new FileStream(name == "" ? path : path + "//" + name, FileMode.Open, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs2);
            sw.WriteLine(con);
            sw.Close();
            fs2.Close();
        }
        #endregion

    }
}
