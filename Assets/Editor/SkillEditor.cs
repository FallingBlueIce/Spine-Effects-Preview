using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine.UI;
using Spine.Unity;
using System.IO;
using Newtonsoft.Json;
public class SkillEditor : EditorWindow
{
    // properties

    /// <summary>
    /// 实例化角色
    /// </summary>
    private GameObject player;

    /// <summary>
    /// 实例化场景
    /// </summary>
    private GameObject scene;

    /// <summary>
    /// 场景预制体
    /// </summary>
    Sprite scene_obj = null;

    /// <summary>
    /// 角色data
    /// </summary>
    GameObject m_Role = null;

    /// <summary>
    /// 角色实体
    /// </summary>
    PlayerEntity entity;

    /// <summary>
    /// 版本号
    /// </summary>
    static string version = "SkillEditor V03";

    static bool foldout;
    static int index;
    string path;
    AnimBool versionInfo;
    const string versionFile = "/Scripts/SkillShow/version.txt";


    /// <summary>
    /// 角色名
    /// </summary>
    public static List<string> RoleNames;
    private Vector2 scrolView = Vector2.zero;

    [MenuItem("Kit/ShowSkillEditor")]
    public static void Open()
    {
        foldout = false;
        var editorPlatform = GetWindow<SkillEditor>();
        editorPlatform.titleContent = new GUIContent(version);
        index = 0;
        editorPlatform.position = new Rect(
            Screen.width / 2,
            Screen.height * 2 / 3,
            450,
            800
        );
        RoleNames = new List<string>();
        GetAllNames();
        editorPlatform.Show();

        Init();
    }

    private void OnEnable()
    {
        versionInfo = new AnimBool(false);
        versionInfo.valueChanged.AddListener(Repaint);
    }

    /// <summary>
    /// 初始化编辑器
    /// </summary>
    private static void Init()
    {
        GameObject.FindObjectOfType<Camera>().orthographic = true;
    }

    /// <summary>
    /// 获取角色名字
    /// </summary>
    private static void GetAllNames()
    {
        RoleNames.Add("1");
        RoleNames.Add("2");
        RoleNames.Add("3");
    }

    /// <summary>
    /// 渲染UI
    /// </summary>
    public void OnGUI()
    {
        scrolView= EditorGUILayout.BeginScrollView(scrolView);
        EditorGUILayout.BeginVertical();
        GUILayout.Space(10);


        GUIStyle style1 = new GUIStyle();
        GUIStyle style2 = new GUIStyle();
        style1.richText = true;
        style2.richText = true;
        style1.fontStyle = FontStyle.Bold;
        GUILayout.Label("<color=#FFFFCC><size=14>    注意事项：\n        "+
                        "1. 先运行再打开插件！\n        "+
                        "2. 特效需要先从Hierarchy拖到Project中生成预制体再使用！\n        "+
                        "3. 先加载场景载加载角色</size></color> \n \n",
                        style1);
        versionInfo.target = EditorGUILayout.ToggleLeft("版本信息", versionInfo.target);

        string versionContent = File.ReadAllText(Application.dataPath + versionFile);
        if (EditorGUILayout.BeginFadeGroup(versionInfo.faded))
        {
            GUILayout.Label(versionContent, style1);
        }
        EditorGUILayout.EndFadeGroup();
        GUILayout.Label("<color=#888888><size=12>  ______________________________________________________________________________________</size></color>", style2);
        GUILayout.Space(10);

        // 1. 场景
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(10);
        GUILayout.Label("<color=#FFFFDF><size=14>场景： </size></color>", style1);
        GUILayout.Space(32);
        scene_obj = (Sprite)EditorGUILayout.ObjectField("",scene_obj, typeof(Sprite), false);
        if (GUILayout.Button("新建场景") && scene_obj != null)
        {
            if (scene != null)
            {
                GameObject.DestroyImmediate(scene_obj, true);
            }
            CreateScene();
        }
        GUILayout.Space(15);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);

        // 2. 角色
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(10);
        GUILayout.Label("<color=#FFFFDF><size=14>Spine 角色：</size></color>", style1);
        GUILayout.Space(90);
        m_Role = (GameObject)EditorGUILayout.ObjectField(m_Role, typeof(GameObject), false);
        if (GUILayout.Button("新建角色") && m_Role != null)
        {
            if (player != null)
            {
                GameObject.DestroyImmediate(player, true);
            }
            player = Instantiate(m_Role);
            entity = player.AddComponent<PlayerEntity>();
            entity.skeletonGraphic = player.GetComponent<SkeletonGraphic>();
            entity.ReadAnimationList(entity.skeletonGraphic.skeletonDataAsset.GetSkeletonData(false));

            //读Json
            ReadJson();

            CheckStage();
            player.transform.SetParent(GameObject.Find("Stage").transform);
            player.transform.localPosition = new Vector3(0, 0, 0);
            player.transform.localScale = new Vector3(1, 1, 1);
            player.SetActive(true);
        }
        GUILayout.Space(15);
        EditorGUILayout.EndHorizontal();
        //GUILayout.Label("<color=#888888><size=12>  ______________________________________________________________________________________</size></color>", style2);
        GUILayout.Space(5);

        // 3. 技能
        EditorGUILayout.BeginHorizontal();
        EditorShowSkill();
        EditorGUILayout.EndHorizontal();
        
        //写Json
        WriteJson();
        GUILayout.Space(20);
        // End
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
    }

    // 技能
    private void EditorShowSkill()
    {
        if (entity != null)
            entity.OnGui();
    }

    // 检查场景是否加载
    private void CheckStage()
    {
        GameObject stage;
        if ((stage = GameObject.Find("Stage")) == null)
        {
            stage = new GameObject("Stage", typeof(Canvas), typeof(GraphicRaycaster), typeof(CanvasScaler));
            stage.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
            stage.GetComponent<Canvas>().worldCamera = GameObject.FindObjectOfType<Camera>();
        }
    }

    // 创建场景，调整场景渲染属性
    private void CreateScene()
    {
        scene = new GameObject("背景");
        scene.transform.position = new Vector3(0, 0, 10);
        CheckStage();
        scene.transform.SetParent(GameObject.Find("Stage").transform, false);
        scene.AddComponent<Image>().sprite = scene_obj;
        scene.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        scene.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        scene.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
    }

    private void ReadJson()
    {
        path = Application.dataPath + "/Output";
        string readPath = path + "/" + entity.transform.name;
        readPath = readPath.Split('(')[0];
        if (!File.Exists(readPath))
            entity.AddSkill("当前技能", true);
        else
        {
            string js = File.ReadAllText(readPath);
            Dictionary<string, List<DataBase>> dic = JsonConvert.DeserializeObject<Dictionary<string, List<DataBase>>>(js, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            foreach (var item in dic)
            {
                entity.AddSkill(item.Key, false, item.Value);
            }
        }
    }

    private void WriteJson()
    {
        path = Application.dataPath + "/Output";
        if (entity != null)
            if (GUILayout.Button("保存当前设置"))
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileName = path + "/" + entity.transform.name;
                Dictionary<string, List<DataBase>> dic = new Dictionary<string, List<DataBase>>();
                for (int i = 0; i < entity.skillDic.Count; i++)
                {
                    List<ComponentBase> comList = entity.skillDic[i].comDic;
                    string skiName = entity.skillDic[i].SkillName;
                    dic.Add(skiName, new List<DataBase>());
                    for (int j = 0; j < comList.Count; j++)
                    {
                        if (comList[j] is AnimatorComponent)
                            dic[skiName].Add(((AnimatorComponent)comList[j]).dataBase);
                        else if (comList[j] is EffectComponent)
                            dic[skiName].Add(((EffectComponent)comList[j]).dataBase);
                    }
                }
                string str = JsonConvert.SerializeObject(dic, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                File.WriteAllText(fileName, str);
                AssetDatabase.Refresh();
            }
    }

}
