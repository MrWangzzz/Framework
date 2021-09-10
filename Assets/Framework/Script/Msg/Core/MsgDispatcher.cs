
using System.Collections.Generic;

using FrameWork;

using UnityEngine;

public static class MsgDispatcher
{

    /// <summary>消息捕捉器</summary>
    private class LogicMsgHandler
    {

        public IMsgReceiver receiver;
        public VoidDelegate.WithParams callback;
        public LogicMsgHandler(IMsgReceiver receiver, VoidDelegate.WithParams callback)
        {
            this.receiver = receiver;
            this.callback = callback;
        }
    }

    /// <summary>每个消息名字维护一组消息捕捉器</summary>
    private static Dictionary<string, List<LogicMsgHandler>> mMsgHandlerDict = new Dictionary<string, List<LogicMsgHandler>>();

    /// <summary>
    /// 注册消息,
    /// 注意第一个参数,使用了C# this的扩展,
    /// 所以只有实现IMsgReceiver的对象才能调用此方法
    /// </summary>
    public static void RegisterMsg(this IMsgReceiver self, string msgName, VoidDelegate.WithParams callback)
    {
        if (string.IsNullOrEmpty(msgName))
        {
            DebugUtil.Log("消息系统 【注册】：", "消息名称是 null或者empty");
            return;
        }

        if (null == callback)
        {
            DebugUtil.Log("消息系统 【注册】：", $"{msgName} callback是null");
            return;
        }

        if (!mMsgHandlerDict.ContainsKey(msgName))
        {
            mMsgHandlerDict[msgName] = new List<LogicMsgHandler>();
        }

        var handlers = mMsgHandlerDict[msgName];

        // 防止重复注册
        foreach (var handler in handlers)
        {
            if (handler.receiver == self && handler.callback == callback)
            {
                DebugUtil.Log("消息系统 【注册】：", $"{msgName} 已经注册！");
                return;
            }
        }
        handlers.Add(new LogicMsgHandler(self, callback));
    }
    /// <summary>
    /// 注销消息
    /// </summary>
    /// <param name="self">实例</param>
    /// <param name="msgName">消息名称</param>
    /// <param name="isAll">false=只注销当前实例的msgName true=注销全部实例的msgName </param>
    public static void CancelLogicMsg(this IMsgReceiver self, string msgName, bool isAll = false)
    {
        if (string.IsNullOrEmpty(msgName))
        {
            DebugUtil.Log("消息系统 【注册】：", "消息名称是 null或者empty");
            return;
        }

        if (!mMsgHandlerDict.ContainsKey(msgName))
        {
            DebugUtil.Log("消息系统 【注销】：", $"{msgName} 此消息没有注册！");
            return;
        }
        if (isAll == false)
        {
            if (mMsgHandlerDict.ContainsKey(msgName))
            {
                var handlers = mMsgHandlerDict[msgName];
                LogicMsgHandler mLogicMsgHandler = handlers.Find(s => s.receiver == self);
                if (mLogicMsgHandler != null)
                    handlers.Remove(mLogicMsgHandler);
                else
                    DebugUtil.Log("消息系统 【注销】：", $"{msgName} 消息没有找到");
            }
        }
        else
        {
            if (mMsgHandlerDict.ContainsKey(msgName))
                mMsgHandlerDict.Remove(msgName);
        }
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="sender">实例名</param>
    /// <param name="msgName">消息名称</param>
    /// <param name="paramList">消息参数</param>
    public static void SendMsg(this IMsgSender sender, string msgName, params object[] paramList)
    {
        if (string.IsNullOrEmpty(msgName))
        {
            DebugUtil.Log("消息系统 【注册】：", "消息名称是 null或者empty");
            return;
        }
        if (!mMsgHandlerDict.ContainsKey(msgName))
        {
            DebugUtil.Log("消息系统 【发送】：", $"{msgName} 此消息没有注册！");
            return;
        }

        var handlers = mMsgHandlerDict[msgName];
        var handlerCount = handlers.Count;

        // 之所以是从后向前遍历,是因为  从前向后遍历删除后索引值会不断变化
        for (int index = handlerCount - 1; index >= 0; index--)
        {
            var handler = handlers[index];
            if (handler.receiver != null)
            {
                DebugUtil.Log("消息系统 【发送】：", $"{sender} 开始发送 {msgName} 参数>>>> {(paramList.Length > 0 ? paramList.ListToString(">>>>") : "")}");
                handler.callback(paramList);
            }
            else
            {
                handlers.Remove(handler);
            }
        }
    }
}

