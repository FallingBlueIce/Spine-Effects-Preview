using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#pragma warning disable CS0246 // δ���ҵ����ͻ������ռ�����Spine��(�Ƿ�ȱ�� using ָ����������?)
using Spine.Unity;
#pragma warning restore CS0246 // δ���ҵ����ͻ������ռ�����Spine��(�Ƿ�ȱ�� using ָ����������?)

/// <summary>
/// �������
/// </summary>
public class AnimatorComponent : ComponentBase
{
    public string skillName;
    private AnimatorStateInfo info;
    public DataAnimator dataBase;
    bool playByFrame;
    bool isPause = false;

    public AnimatorComponent(PlayerEntity playerEntity, SkillEntity skillEntity, string comName, int index, bool isLoop) : base(playerEntity, skillEntity, comName)
    {
        dataBase = new DataAnimator(comName, ComType.CAnimator, index, isLoop);
    }

    public override void Init()
    {

    }

    public override void Show(float time, bool play)
    {
        //Spine���Ŷ���
        dataBase.isLoop = false;
        if (skillName != null)
        {
            if (play)
            {
                playerEntity.skeletonGraphic.AnimationState.SetAnimation(0, skillName, dataBase.isLoop);
            }
            setTimeScale(time);
        }

    }

    public override void OnGui()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(30);
        GUIStyle style = new GUIStyle();
        style.richText = true;
        style.fontStyle = FontStyle.Bold;
        foldout = EditorGUILayout.Foldout(foldout, "<color=#66FF66><size=12>" + "�� " + comName + "</size></color>", true, style);
        base.OnGui();
        GUILayout.Space(30);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(5);
        if (foldout)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(35);
            dataBase.index = EditorGUILayout.Popup("����", dataBase.index, playerEntity.animationName);
            skillName = playerEntity.animationName[dataBase.index];
            GUILayout.Space(30);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(185);
            dataBase.isLoop = GUILayout.Toggle(dataBase.isLoop, "ѭ������");
            playByFrame = GUILayout.Toggle(playByFrame, "��֡����");
            if (playByFrame && !isPause)
            {
                setTimeScale(Time.deltaTime);
            }
            GUILayout.Space(30);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(185);
            if (GUILayout.Button("�л�����", GUILayout.MaxWidth(120)) && skillName != null)
            {
                isPause = false;
                playerEntity.skeletonGraphic.AnimationState.SetAnimation(0, skillName, dataBase.isLoop);
            }
            if (GUILayout.Button("��ͣ����", GUILayout.MaxWidth(120)) && skillName != null)
            {
                setTimeScale(0.0f);
                isPause = true;
            }
            if (GUILayout.Button("�ָ�����", GUILayout.MaxWidth(120)) && skillName != null)
            {
                setTimeScale(1.0f);
                isPause = false;
            }
            GUILayout.Space(30);
            EditorGUILayout.EndHorizontal();
        }

    }
    void setTimeScale(float scale)
    {
        if (playerEntity.skeletonGraphic.AnimationState.GetCurrent(0) != null)
            playerEntity.skeletonGraphic.AnimationState.GetCurrent(0).TimeScale = scale;
    }
}
