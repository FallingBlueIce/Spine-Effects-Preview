using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 组件枚举类型
/// </summary>
public enum ComType
{
    CBase,
    CAnimator,
    CEffect
}
/// <summary>
/// 预览数据基类
/// </summary>
public class DataBase 
{
    public string comName;
    public ComType comType;
    
    public DataBase(string comName, ComType comType)
    {
        this.comName = comName;
        this.comType = comType;
    }
}
/// <summary>
/// 特效数据类
/// </summary>
public class DateEffect:DataBase
{
    public string effectName;
    public DateEffect (string comName,ComType comType, string effectName):base(comName,comType)
    {
        this.effectName = effectName;
    }
}
/// <summary>
/// 动画组件数据类
/// </summary>
public class DataAnimator:DataBase
{
    public int index;
    public bool isLoop;
    public DataAnimator(string comName, ComType comType, int index, bool isLoop):base(comName, comType)
    {
        this.index = index;
        this.isLoop = isLoop;
    }
}