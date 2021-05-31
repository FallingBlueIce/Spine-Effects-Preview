using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/// <summary>
/// 技能组件效果基类
/// </summary>
public class ComponentBase : MonoBehaviour
{
    protected PlayerEntity playerEntity;
    public string comName;
    public SkillEntity skillEntity;
    public bool foldout;
    public ComponentBase(PlayerEntity playerEntity, SkillEntity skillEntity, string comName)
    {
        foldout = true;
        this.playerEntity = playerEntity;
        this.comName = comName;
        this.skillEntity = skillEntity;
    }
    //初始化组件
    public virtual void Init() { }

    //组件在Editor显示
    public virtual void OnGui()
    {
        if (GUILayout.Button("删除", GUILayout.Width(220)))
            skillEntity.Remove(this);
    }

    //组件具体实现逻辑
    public virtual void Show() { }
    public virtual void Show(float time=0,bool play=false) { }
}
