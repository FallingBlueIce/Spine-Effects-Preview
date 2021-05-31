using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#pragma warning disable CS0246 // δ���ҵ����ͻ������ռ�����Spine��(�Ƿ�ȱ�� using ָ����������?)
using Spine.Unity;
#pragma warning restore CS0246 // δ���ҵ����ͻ������ռ�����Spine��(�Ƿ�ȱ�� using ָ����������?)
using System;

/// <summary>
/// ��ɫʵ��
/// </summary>
public class PlayerEntity : MonoBehaviour
{
    /// <summary>
    /// �����б�
    /// </summary>
    public List<SkillEntity> skillDic;

    /// <summary>
    /// ��������
    /// </summary>
    public string skillName;

    /// <summary>
    /// �����б�
    /// </summary>
    public string[] animationName;

    /// <summary>
    /// ��ɫ�������
    /// </summary>
    public SkeletonGraphic skeletonGraphic;

    public PlayerEntity()
    {
        skillDic = new List<SkillEntity>();
        skillName = "";
    }

    /// <summary>
    /// ��Ӽ���
    /// </summary>
    /// <param name="skillName"></param>
    public void AddSkill(string skillName, bool init, List<DataBase> datas = null)
    {
        if (!CheckRepeate(skillName))
        {
            SkillEntity skillEntity = new SkillEntity(skillName, this, init, datas);
            skillDic.Add(skillEntity);
        }
        else
            Debug.LogError("���������ظ�����");
    }

    /// <summary>
    /// ɾ������
    /// </summary>
    /// <param name="skill"></param>
    public void RemoveSkill(SkillEntity skill)
    {
        if (CheckRepeate(skill.SkillName))
        {
            skillDic.Remove(skill);
        }
    }

    /// <summary>
    /// ��Ⱦ����ģ��UI
    /// </summary>
    public void OnGui()
    {

        GUIStyle style1 = new GUIStyle();
        style1.richText = true;
        style1.fontStyle = FontStyle.Bold;
        GUIStyle style2 = new GUIStyle();
        style2.richText = true;
        EditorGUILayout.BeginVertical();
        GUILayout.Space(15);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(10);
        GUILayout.Label("<color=#FFFFDF><size=14>��Ӽ��ܶ���</size></color>", style1);
        skillName = GUILayout.TextField(skillName);
        if (GUILayout.Button("�½����ܶ���"))
        {
            if (skillName != "")
            {
                AddSkill(skillName,true);
                skillName = "";
            }
        }
        GUILayout.Space(15);
        EditorGUILayout.EndHorizontal();
        GUILayout.Label("<color=#888888><size=12>  ______________________________________________________________________________________</size></color>", style2);
        GUILayout.Space(15);
        for (int i = skillDic.Count-1; i >=0 ; i--)
        {
            skillDic[i].OnGui();
        }
        EditorGUILayout.EndVertical();
    }

    /// <summary>
    /// ��鼼���Ƿ��ظ�
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool CheckRepeate(string name)
    {
        for (int i = 0; i < skillDic.Count; i++)
        {
            if (skillDic[i].SkillName == name)
                return true;
        }
        return false;
    }

    /// <summary>
    /// ��SkeletonData�ж�ȡ�����б�
    /// </summary>
    /// <param name="currentData">��ǰ��ɫ����</param>
#pragma warning disable CS0246 // δ���ҵ����ͻ������ռ�����Spine��(�Ƿ�ȱ�� using ָ����������?)
    public void ReadAnimationList(Spine.SkeletonData currentData)
#pragma warning restore CS0246 // δ���ҵ����ͻ������ռ�����Spine��(�Ƿ�ȱ�� using ָ����������?)
    {
        Spine.Animation[] animations = currentData.Animations.Items;
        animationName = new string[animations.Length];
        for (int id = 0; id < animations.Length; id++)
        {
            animationName[id] = animations[id].Name;
        }
    }
}
