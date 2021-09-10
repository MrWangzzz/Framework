using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RectToDrawInfo
{
    public Vector2 startPoint;
    public Vector2 endPoint;

    public RectToDrawInfo(Vector2 startPoint, Vector2 endPoint)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
    }


}


/// <summary>
/// 亲测 GL的API在项目使用urp的情况下 会绘制不出来
/// </summary>
public class DrawRect : MonoBehaviour
{
    private Vector2 mMouseStart, mMouseEnd;
    private bool mBDrawMouseRect;

    //画线的材质不设定系统会用当前材质画线 结果不可控
    //一般选择defualt-line 材质
    [SerializeField]
    private Material rectMat = null;


    List<RectToDrawInfo> rectToDrawInfos;


    private static DrawRect instance;
    public static DrawRect Instance
    {
        get
        {
            return instance;
        }
    }


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

        mBDrawMouseRect = false;

        rectMat.hideFlags = HideFlags.HideAndDontSave;
        rectMat.shader.hideFlags = HideFlags.HideAndDontSave;
    }

    public void SetDrawInfo(List<RectToDrawInfo> toDrawInfos)
    {
        rectToDrawInfos = toDrawInfos;
    }


    void OnGUI()
    {
        if( rectToDrawInfos != null )
        {
            foreach( var item in rectToDrawInfos )
            {
                Draw(item.startPoint, item.endPoint);

            }
        }
    }

    //渲染2D框
    void Draw(Vector2 start, Vector2 end)
    {
        rectMat.SetPass(0);

        GL.PushMatrix();//保存摄像机变换矩阵

        Color clr = Color.green;
        clr.a = 0.1f;

        //设置在屏幕坐标绘图
        //此时传入的点是在屏幕坐标系下
        GL.LoadPixelMatrix();

        //半透明框  
        //根据矩形的左下角和右上角的点绘制
        GL.Begin(GL.QUADS);
        GL.Color(clr);
        GL.Vertex3(start.x, start.y, 0);
        GL.Vertex3(end.x, start.y, 0);
        GL.Vertex3(end.x, end.y, 0);
        GL.Vertex3(start.x, end.y, 0);
        GL.End();

        //线
        //上
        GL.Begin(GL.LINES);
        GL.Color(Color.green);
        GL.Vertex3(start.x, start.y, 0);
        GL.Vertex3(end.x, start.y, 0);
        GL.End();

        //下
        GL.Begin(GL.LINES);
        GL.Color(Color.green);
        GL.Vertex3(start.x, end.y, 0);
        GL.Vertex3(end.x, end.y, 0);
        GL.End();

        //左
        GL.Begin(GL.LINES);
        GL.Color(Color.green);
        GL.Vertex3(start.x, start.y, 0);
        GL.Vertex3(start.x, end.y, 0);
        GL.End();

        //右
        GL.Begin(GL.LINES);
        GL.Color(Color.green);
        GL.Vertex3(end.x, start.y, 0);
        GL.Vertex3(end.x, end.y, 0);
        GL.End();

        GL.PopMatrix();//还原
    }
}