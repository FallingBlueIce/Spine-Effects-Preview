using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#pragma warning disable CS0246 // 未能找到类型或命名空间名“Spine”(是否缺少 using 指令或程序集引用?)
using Spine.Unity;
#pragma warning restore CS0246 // 未能找到类型或命名空间名“Spine”(是否缺少 using 指令或程序集引用?)

/// <summary>
/// 动画组件
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
        //Spine播放动画
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
        foldout = EditorGUILayout.Foldout(foldout, "<color=#66FF66><size=12>" + "▼ " + comName + "</size></color>", true, style);
        base.OnGui();
        GUILayout.Space(30);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(5);
        if (foldout)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(35);
            dataBase.index = EditorGUILayout.Popup("动画", dataBase.index, playerEntity.animationName);
            skillName = playerEntity.animationName[dataBase.index];
            GUILayout.Space(30);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(185);
            dataBase.isLoop = GUILayout.Toggle(dataBase.isLoop, "循环播放");
            playByFrame = GUILayout.Toggle(playByFrame, "逐帧播放");
            if (playByFrame && !isPause)
            {
                setTimeScale(Time.deltaTime);
            }
            GUILayout.Space(30);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(185);
            if (GUILayout.Button("切换动画", GUILayout.MaxWidth(120)) && skillName != null)
            {
                isPause = false;
                playerEntity.skeletonGraphic.AnimationState.SetAnimation(0, skillName, dataBase.isLoop);
            }
            if (GUILayout.Button("暂停动画", GUILayout.MaxWidth(120)) && skillName != null)
            {
                setTimeScale(0.0f);
                isPause = true;
            }
            if (GUILayout.Button("恢复动画", GUILayout.MaxWidth(120)) && skillName != null)
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
