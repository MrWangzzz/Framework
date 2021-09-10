//-------------------------------------------
//作者：马超
//时间：2020-09-25 16:35
//作用：打印日志
//-------------------------------------------
using UnityEngine;
using System.Text;
namespace FrameWork
{
    public class DebugUtil
    {
        /// <summary>
        /// 是否打开log
        /// </summary>
        public static bool isOpenLog
        {
            get
            {
#if DEBUG_LOG
                return true;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="args"></param>
        public static void LogWarn(params object[] args)
        {
            if (isOpenLog)
                Debug.LogWarning($"{args.ListToString(">>>>")}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void LogError(params object[] args)
        {
            if (isOpenLog)
                Debug.LogError($"{args.ListToString(">>>>")}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="msg"></param>
        /// <param name="color"></param>
        public static void Log(string tag, object msg, string color = "white")
        {
            Log(color.GetColorByHax(), tag, msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="args"></param>
        public static void Log(Color color, params object[] args)
        {
            if (isOpenLog)
                Debug.Log($"<color={color}>{args.ListToString(">>>>")}</color>");
        }
    }
}