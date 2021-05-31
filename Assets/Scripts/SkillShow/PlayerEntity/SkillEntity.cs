using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// ����ʵ��
/// </summary>
public class SkillEntity
{
    public PlayerEntity playerEntity;
    public List<ComponentBase> comDic;
    public string SkillName;
    private string comName;
    public string[] Components = new string[] { "null", "Animator", "Effect" };
    public int index;
    bool foldout;
    bool playByFrame = false;
    bool isPaused = false;
    int timeLevel = 1;
    float timescale = 1;

    private ComponentBase com;
    public SkillEntity(string SkillName, PlayerEntity playerEntity, bool init, List<DataBase> datas = null)
    {
        this.playerEntity = playerEntity;
        this.SkillName = SkillName;
        comDic = new List<ComponentBase>();
        index = 0;
        comName = "";
        foldout = true;
        Init(init, datas);

    }

    private void Init(bool init, List<DataBase> datas = null)
    {
        if (init)
        {
            ComponentBase com1 = new AnimatorComponent(playerEntity, this, "����ʵ��1", 0, false);
            ComponentBase com2 = new EffectComponent(playerEntity, this, "��Чʵ��1", "");

            Add(com1);
            Add(com2);
        }
        else
        {
            ComponentBase com = null;
            for (int i = 0; i < datas.Count; i++)
            {
                if (datas[i].comType == ComType.CAnimator)
                {
                    DataAnimator data = (DataAnimator)datas[i];
                    com = new AnimatorComponent(playerEntity, this, data.comName, data.index, data.isLoop);
                }
                else if (datas[i].comType == ComType.CEffect)
                {
                    DateEffect data = (DateEffect)datas[i];
                    com = new EffectComponent(playerEntity, this, data.comName, data.effectName);
                }
                Add(com);
            }
        }


        for (int i = comDic.Count - 1; i >= 0; i--)
        {
            comDic[i].Init();
        }
    }

    public void Add(ComponentBase com)
    {
        int index;
        if (!CheckRepeate(com.comName, out index))
        {
            comDic.Add(com);
        }
    }
    public void Remove(ComponentBase com)
    {
        int index;
        if (CheckRepeate(com.comName, out index))
        {
            comDic.RemoveAt(index);
        }
    }

    public void OnGui()
    {

        GUIStyle style = new GUIStyle();
        style.richText = true;
        style.fontStyle = FontStyle.Bold;

        GUIStyle style2 = new GUIStyle();
        style2.richText = true;
        GUILayout.Space(30);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(10);
        foldout = EditorGUILayout.Foldout(foldout, "<color=#BBFFBB><size=14>" + "�� " + SkillName + "</size></color>", true, style);
        if (GUILayout.Button("ɾ������", GUILayout.Width(150)))
        {
            playerEntity.RemoveSkill(this);
        }
        GUILayout.Space(20);
        EditorGUILayout.EndHorizontal();
        if (foldout)
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Label("<color=#666666><size=12>    ___________________________________________________________________________________</size></color>", style2);
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("����������:                           ");
            comName = GUILayout.TextField(comName, GUILayout.Width(120));
            index = EditorGUILayout.Popup(index, Components);
            if (GUILayout.Button("���"))
            {
                if (comName != "" && index != 0)
                {
                    switch (index)
                    {
                        case 1:
                            com = new AnimatorComponent(playerEntity, this, comName, 0, false);
                            break;
                        case 2:
                            com = new EffectComponent(playerEntity, this, comName, "");
                            break;
                        default:
                            break;
                    }
                    Add(com);
                    comName = "";
                    index = 0;
                }

            }
            EditorGUILayout.Space(20);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);

            playByFrame = EditorGUILayout.ToggleLeft("��֡����", playByFrame);
            if (playByFrame && !isPaused)
            {
                timescale = Time.deltaTime;
            }


            GUILayout.Label("�����ٶ�");
            timescale = EditorGUILayout.Slider(timescale, 0, 2);

            GUILayout.Space(20);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            if (GUILayout.Button("����"))
            {
                isPaused = false;
                for (int i = comDic.Count - 1; i >= 0; i--)
                {
                    comDic[i].Show(timescale, true);
                }
                SetEffectSpeed(timescale);
            }
            if (GUILayout.Button("��ͣ"))
            {
                isPaused = true;
                for (int i = comDic.Count - 1; i >= 0; i--)
                {
                    comDic[i].Show(0.0f, false);
                }
                SetEffectSpeed(0);
            }
            if (GUILayout.Button("�ָ�"))
            {
                isPaused = false;
                for (int i = comDic.Count - 1; i >= 0; i--)
                {
                    comDic[i].Show(timescale, false);
                }
                SetEffectSpeed(timescale);
            }
            GUILayout.Space(20);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(10);



            EditorGUILayout.BeginVertical();
            GUILayout.Label("<color=#444444><size=12>           ____________________________________________________________________________</size></color>", style2);
            for (int i = comDic.Count - 1; i >= 0; i--)
            {
                EditorGUILayout.Space(10);
                comDic[i].OnGui();
                GUILayout.Label("<color=#444444><size=12>           ____________________________________________________________________________</size></color>", style2);
            }
            GUILayout.Space(10);
            EditorGUILayout.EndVertical();
            GUILayout.Label("<color=#666666><size=12>     ___________________________________________________________________________________</size></color>", style2);
            EditorGUILayout.EndVertical();
            GUILayout.Space(10);
        }

    }

    private void SetEffectSpeed(float timescale)
    {
        if(playerEntity.transform.childCount>0)
        {
            ParticleSystem[] pSlist = playerEntity.transform.GetChild(0).GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < pSlist.Length; i++)
            {
                if (timescale != 0)
                    pSlist[i].startSpeed = timescale;
                else
                    pSlist[i].Pause();
            }
        }
    }

    public bool CheckRepeate(string name, out int index)
    {
        for (int i = 0; i < comDic.Count; i++)
        {
            if (comDic[i].comName == name)
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
    }


}
