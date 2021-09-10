
//using UnityEngine;
//using ILRuntime.Runtime.Enviorment;
//using UnityEngine.Networking;
//using System.Collections;
//using FrameWork;
//using System.IO;
//using System;
//using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;
//using ILRuntime.CLR.TypeSystem;
//using ILRuntime.CLR.Method;
//using ILRuntimeDemo;

//public class Main : SingletonGetMono<Main>
//{
//    internal AppDomain appDomain;
//    MemoryStream htdll = null;
//    MemoryStream htpdb = null;
//    private void Start()
//    {
//        StartCoroutine(LoadHotFixAssembly());
//        //DialogMgr.Instance.ShowDialog("DialogLoad");

//    }

//    private IEnumerator LoadHotFixAssembly()
//    {
//        appDomain = new AppDomain();
//        string temp = "file:///";
//#if UNITY_ANDROID
//        temp = "";
//#endif
//        string Path = $"{temp}{Application.streamingAssetsPath}/HotFix_Project.";
//        UnityWebRequest request = UnityWebRequest.Get($"{Path}dll");
//        yield return request.SendWebRequest();
//        if( request.isNetworkError || request.isHttpError )
//        {
//            DebugUtil.Log("Tips", request.error);
//        }
//        else
//        {
//            htdll = new MemoryStream(request.downloadHandler.data);
//            request.Dispose();
//        }

//#if DEBUG_LOG
//        request = UnityWebRequest.Get($"{Path}pdb");
//        yield return request.SendWebRequest();
//        if( request.isNetworkError || request.isHttpError )
//        {
//            DebugUtil.Log("Tips", request.error);
//        }
//        else
//        {
//            htpdb = new MemoryStream(request.downloadHandler.data);
//            request.Dispose();
//        }
//#endif
//        try
//        {
//#if DEBUG_LOG
//            appDomain.LoadAssembly(htdll, htpdb, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
//#else
//            appDomain.LoadAssembly(htdll, htpdb, null);
//#endif
//        }
//        catch
//        {
//            DebugUtil.LogError("加载热更DLL失败，请确保已经通过VS打开Assets/Samples/ILRuntime/1.6/Demo/HotFix_Project/HotFix_Project.sln编译过热更DLL");
//        }
//        InitializeILRuntime();
//        OnHotFixLoaded();
//    }

//    private void InitializeILRuntime()
//    {
//#if DEBUG && ( UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE )
//        //由于Unity的Profiler接口只允许在主线程使用，为了避免出异常，需要告诉ILRuntime主线程的线程ID才能正确将函数运行耗时报告给Profiler
//        appDomain.UnityMainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
//#endif
//        //这里做一些ILRuntime的注册，HelloWorld示例暂时没有需要注册的
//    }

//    private void OnHotFixLoaded()
//    {
//        //appDomain.RegisterCrossBindingAdaptor(new DialogBaseAdapter());
//        //var db = appDomain.Instantiate<DialogBase>("FrameWork.DialogLoad");
//        //DebugUtil.Log("Tips",db);
//        // 
//        appDomain.RegisterCrossBindingAdaptor(new TestClassBaseAdapter());
//        //appDomain.RegisterCrossBindingAdaptor(new DialogBaseAdapter());
//        var tc = appDomain.Instantiate<TestClassBase>("HotFix_Project.TestInheritance");
//        DebugUtil.Log("Tips tc", tc);
//        tc.TestAbstract(1111);

//        //var db = appDomain.Instantiate<DialogBase>("FrameWork.DialogLoad");
//        //DebugUtil.Log("Tips db", db);
//        //db.OnInit();

//        //go.AddComponent(t);
//    }

//    private void OnDestroy()
//    {
//        if( htdll != null ) htdll.Close();
//        if( htpdb != null ) htpdb.Close();
//        htdll = null;
//        htpdb = null;
//    }
//}
