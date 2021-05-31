using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using System.IO;

/// <summary>
/// 特效组件
/// </summary>
public class EffectComponent : ComponentBase
{
    //挂点
    private string point;
    private Transform pointTrans;
    private GameObject effect1;
    private GameObject effect2;
    public DateEffect dataBase;
    Transform tf;
    // 特效transform
    bool effectTF;
    Vector3 effectPos = Vector3.zero;
    Vector3 effectRt = Vector3.zero;
    Vector2 effectSize = Vector2.zero;

    public EffectComponent(PlayerEntity playerEntity, SkillEntity skillEntity, string comName, string effectName) : base(playerEntity, skillEntity, comName)
    {
        dataBase = new DateEffect(comName, ComType.CEffect, effectName);
    }
    public override void Init()
    {
        //point = "Point";
        pointTrans = playerEntity.transform;
        //playerEntity.transform.Find(point);
        if (dataBase.effectName != "")
            effect1 = Resources.Load<GameObject>(dataBase.effectName);
    }
    public override void Show(float time,bool timeplay)
    {
        if (effect1 != null)
        {
            if (effect2 != null)
            {
                GameObject.DestroyImmediate(effect2, true);
                effect2 = null;
            }
            GameObject temp = GameObject.Instantiate(effect1);
            temp.transform.SetParent(pointTrans);
            temp.transform.position = pointTrans.position;
            effect2 = temp;
            effect2.transform.localScale = new Vector3(1, 1, 1);
            if (!effect2.GetComponent<SortingGroup>())
            {

                SortingGroup sg = effect2.AddComponent<SortingGroup>();
                sg.sortingOrder = 1;
            }
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

            //这里写拖拽逻辑
            //effect1=拖拽API
            effect1 = (GameObject)EditorGUILayout.ObjectField("特效", effect1, typeof(GameObject), true);
            if (effect1 != null)
            {
                dataBase.effectName = effect1.name;
            }

            if (GUILayout.Button("保存") && effect2)
            {
                if (!Directory.Exists(Application.dataPath + "/Resources"))
                {
                    Directory.CreateDirectory(Application.dataPath + "/Resources");
                }
                string str = Application.dataPath + "/Resources/" + dataBase.effectName + ".Prefab";
                PrefabUtility.SaveAsPrefabAsset(effect2, str);
                AssetDatabase.Refresh();
            }

            GUILayout.Space(30);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            GUILayout.Space(35);
            EditorGUILayout.BeginVertical();
            effectTF = EditorGUILayout.Foldout(effectTF, "<color=#FFFFFF><size=12>▽ Effect Transform</size></color>", true, style);
            tf = Selection.activeTransform;
            if (effectTF  && tf)
            {
                Debug.LogError(tf.name);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(40);
                tf.localPosition = EditorGUILayout.Vector3Field("位置", tf.localPosition);
                GUILayout.Space(40);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(40);
                tf.localRotation = Quaternion.Euler(EditorGUILayout.Vector3Field("旋转", tf.localRotation.eulerAngles));
                GUILayout.Space(40);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(40);
                tf.localScale = EditorGUILayout.Vector3Field("大小", tf.localScale);
                GUILayout.Space(40);
                EditorGUILayout.EndHorizontal();

            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

    }

}
