1.将framework导入项目 在场景中创建Canvas 场景中如果有多个canvas，默认查找到第一个

2.点击LG_Tool > 一键添加文件夹  自动生成项目分类目录
	//Atlas    --->资源图片 文件夹
	//Scripts  --->项目脚本 文件夹
	//Scenes   --->项目场景 文件夹
	//Model    --->模型资源 文件夹
	//Plugins  --->导入插件 文件夹
	//Editor   --->编辑器用 文件夹
	//Prefabs  --->缓存预设 文件夹
	//Resources--->加载资源 文件夹
	//Thread   --->三方插件 文件夹

3.在场景中创建需要加载的UI预设体 。格式为（Panel_ 或Dialog_）+ name

4.做完物体后，选中物体后 点击菜单栏中LG_Tool > 一键制作框架预制体  自动生成需要加载的预设体及对应脚本

5.打开脚本如图，分三块区域
①界面加载中 都是继承自父类的方法， 现在自动创建完后基本不需要修改
②数据定义中 定义该面板所需变量，物体，等等
③逻辑中     各种实现功能的方法，逻辑  panelArgs 及dialogArgs 是调动这个面板时传的参数数组。取时按数组下标去取

注意：
_cache 关闭时 物体关闭时为销毁 打开时 物体关闭时为隐藏
[DynamicLoad] 在数据定义中， 添加该标签头，该字段会自动查找该面板中对应名称的物体

如果为 Dialog ，界面加载中 
		_alpha =0~1; 遮罩的透明度
        _showStyle=DialogMgr.DialogShowStyle.Nomal;//修改打开风格
        _maskStyle=DialogMgr.DialogMaskSytle.None;//修改遮罩方式

6.调用打开对应物体。
①打开一个Dialog ，DialogMgr.GetInstance.ShowDialog(名称,参数[]);没有参数则不用传
②打开一个Panel ，PanelMgr.GetInstance.SwitchingPanel(名称，参数[]);没有参数则不用传
③ 如需提示窗 ,   LogicMgr.GetInstance.GetLogic<LogicTips>().AddTips("添加一条提示");

7.事件注册使用
需要接收的脚本，实现 IMsgReceiver接口
需要发送的脚本，实现 IMsgSender接口
①发送事件 MsgDispatcher.SendMsg(this, "TestRegist",50);
②注册事件 MsgDispatcher.RegisterMsg(this, "TestRegist", TestRegist); 并实现TestRegist方法

8.Singleton脚本
Singleton类，不需要场景挂载，会在调用时自动创建
SingletonGetMono类，继承自mono, 需要挂载场景中的物体上
SingletonMono类，继承自mono， 不需要场景挂载，会在调用时自动创建到不销毁的物体上

9.宏定义管理面板
打开菜单中LG_Tool/宏定义
打开DEBUG_LOG宏定义，打印日志时用DebugUtil打印，当去掉该宏定义，DebugUtil会自动不打印，方便打正式包

10.打包工具面板
打开菜单中LG_Tool/打包工具
点击设置初始物料，可对项目进行物料设置

