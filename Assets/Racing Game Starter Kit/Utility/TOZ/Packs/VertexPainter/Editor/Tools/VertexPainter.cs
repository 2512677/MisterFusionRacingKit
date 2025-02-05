using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//using TOZ;

namespace TOZEditor
{

    public class VertexPainter : EditorWindow
    {
        #region Editor Window
        // ���������� ���� ���������
        private static VertexPainter window;

        // ���� ��� �������� ���� Vertex Painter � Unity
        [MenuItem("MR Fusuin Engine/�����������/Vertex Painter")]
        private static void CreateWindow()
        {
            // ������������� ����
            int x = 40, y = 40, w = 312, h = 198;
            window = (VertexPainter)GetWindow(typeof(VertexPainter), true);
            window.position = new Rect(x, y, w, h);
            window.minSize = new Vector2(w, h);
            window.maxSize = new Vector2(w, h);
            window.titleContent = new GUIContent("Vertex Painter");
            window.Show();
        }
        #endregion Editor Window

        #region Variables
        // �������� ���������
        private static Color _r = new Color(1f, 0f, 0f, 0f); // �������
        private static Color _g = new Color(0f, 1f, 0f, 0f); // ������
        private static Color _b = new Color(0f, 0f, 1f, 0f); // �����
        private static Color _a = new Color(0f, 0f, 0f, 1f); // �����
        private static Color _w = new Color(1f, 1f, 1f, 1f); // �����

        // ������ �� ������� � ����������
        private GameObject go;
        private Collider coll;
        private MeshFilter mf;
        private Mesh mesh;
        private MeshRenderer mr;
        private Color[] originalColors, debugColors;
        private Shader[] originalShaders;

        // ���������� ��� GUI
        private bool canPaint;
        private string gui_Notification;
        private bool tgl_Paint;
        private string str_Paint;
        private bool tgl_ShowVertexColors;
        private string str_ShowVertexColors;
        private float gui_BrushSize;
        private float gui_BrushOpacity;
        private Color gui_BrushColor;
        #endregion Variables

        // ������ MonoBehaviour
        private void OnEnable()
        {
            // �������� �� ������� ����������� GUI �����
            SceneView.duringSceneGui += OnSceneGUI;

            Setup();
        }

        private void OnDisable()
        {
            Initialize();

            // ������� �� �������
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnSelectionChange()
        {
            // ��������� ��������� ������ �������
            Setup();
            this.Repaint();
        }

        private void OnProjectChange()
        {
            // ��������� ��������� �������
            Setup();
            this.Repaint();
        }

        private void OnInspectorUpdate()
        {
            // ���������� ����������
            this.Repaint();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            // ��������������
            if (!canPaint)
            {
                EditorGUILayout.HelpBox(gui_Notification, MessageType.Warning);
                return;
            }

            EditorGUILayout.BeginHorizontal("box");
            if (GUILayout.Button(str_Paint, GUILayout.Width(136)))
            {
                tgl_Paint = !tgl_Paint;
                if (tgl_Paint)
                {
                    // ��������� ������ ������
                    str_Paint = "���������� ���������";
                    // ��������� ��������� ������ ������
                    tgl_ShowVertexColors = true;
                    str_ShowVertexColors = "������ �����";

                    // ��������� ���������� ��������
                    SetDebugShaders();
                }
                else
                {
                    Setup();
                }
            }

            if (GUILayout.Button(str_ShowVertexColors, GUILayout.Width(136)))
            {
                tgl_ShowVertexColors = !tgl_ShowVertexColors;
                if (tgl_ShowVertexColors)
                {
                    // ��������� ������ ������
                    str_ShowVertexColors = "������ �����";

                    // ��������� ���������� ��������
                    SetDebugShaders();
                }
                else
                {
                    // ��������� ������ ������
                    str_ShowVertexColors = "�������� �����";

                    // �������������� ������������ ��������
                    SetOriginalShaders();
                }
            }

            if (GUILayout.Button("?", GUILayout.Width(22)))
            {
                // �������� ������������ � ��������
                Application.OpenURL("https://tozlab.com/documentation/toz-vertex-painter");
            }
            EditorGUILayout.EndHorizontal();

            if (tgl_Paint)
            {
                // ������� ����� GUI
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical("box");

                EditorGUIUtility.labelWidth = 105f;
                gui_BrushSize = EditorGUILayout.Slider("������ �����:", gui_BrushSize, 0.1f, 10.0f);
                gui_BrushOpacity = EditorGUILayout.Slider("�������������� �����:", gui_BrushOpacity, 0.0f, 1.0f);
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("���� �����:");
                if (GUILayout.Button("R", GUILayout.Width(22)))
                {
                    gui_BrushColor = _r;
                }
                if (GUILayout.Button("G", GUILayout.Width(22)))
                {
                    gui_BrushColor = _g;
                }
                if (GUILayout.Button("B", GUILayout.Width(22)))
                {
                    gui_BrushColor = _b;
                }
                if (GUILayout.Button("A", GUILayout.Width(22)))
                {
                    gui_BrushColor = _a;
                }
                if (GUILayout.Button("W", GUILayout.Width(22)))
                {
                    gui_BrushColor = _w;
                }
                gui_BrushColor = EditorGUILayout.ColorField(gui_BrushColor, GUILayout.Height(20));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();

                // ����������� ����� GUI
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.PrefixLabel("����� ������:");
                EditorGUIUtility.labelWidth = 0f;
                if (GUILayout.Button("R", GUILayout.Width(22)))
                {
                    SetDebugColors(_r);
                }
                if (GUILayout.Button("G", GUILayout.Width(22)))
                {
                    SetDebugColors(_g);
                }
                if (GUILayout.Button("B", GUILayout.Width(22)))
                {
                    SetDebugColors(_b);
                }
                if (GUILayout.Button("A", GUILayout.Width(22)))
                {
                    SetDebugColors(_a);
                }
                if (GUILayout.Button("W", GUILayout.Width(22)))
                {
                    SetDebugColors(_w);
                }
                if (GUILayout.Button("�����", GUILayout.Width(70)))
                {
                    SetOriginalColors();
                }
                EditorGUILayout.EndHorizontal();

                // ������ ����� GUI
                EditorGUILayout.Space();
                if (GUILayout.Button("��������� ����� ���"))
                {
                    string file = EditorUtility.SaveFilePanelInProject("��������� ���", "New Mesh", "mesh", "����������, ������� ��� �����");
                    //string file = AssetDatabase.GenerateUniqueAssetPath("Assets/New Mesh.mesh");
                    if (file.Length != 0)
                    {
                        // �������� ���������� � ���������� ��� ����� ���
                        Mesh data = (Mesh)Instantiate(mesh);

                        if (EditorUtility.DisplayDialog("��������������?", "�������������� ��� � ����������� ������ ������/�������� ����� �����������?", "��", "���"))
                        {
                            MeshUtility.Optimize(data);
                        }

                        AssetDatabase.CreateAsset(data, file);
                        AssetDatabase.SaveAssets();
                        Debug.LogWarning("��� ������� �� ������: " + file);
                        // �������������� ������������ ������ ����
                        Initialize();
                        EditorUtility.FocusProjectWindow();
                        Selection.activeObject = data;
                    }
                }
            }
            else
            {
                EditorGUILayout.HelpBox(gui_Notification, MessageType.Info);
            }

            EditorGUILayout.EndVertical();
        }

        // ������
        private void OnSceneGUI(SceneView sceneView)
        {
            if (!tgl_Paint)
            {
                return;
            }

            Event current = Event.current;
            Ray ray = HandleUtility.GUIPointToWorldRay(current.mousePosition);
            RaycastHit hit;
            // �������
            int controlID = GUIUtility.GetControlID(sceneView.GetHashCode(), FocusType.Passive);
            switch (current.GetTypeForControl(controlID))
            {
                case EventType.Layout:
                    {
                        if (!tgl_Paint)
                        {
                            return;
                        }
                        HandleUtility.AddDefaultControl(controlID);
                    }
                    break;
                case EventType.MouseDown:
                case EventType.MouseDrag:
                    {
                        if (!tgl_Paint)
                        {
                            return;
                        }
                        if (current.alt || current.control)
                        {
                            return;
                        }
                        if (HandleUtility.nearestControl != controlID)
                        {
                            // ����� �������� �������������� ������ ��� �������������
                        }
                        if (current.GetTypeForControl(controlID) == EventType.MouseDrag && GUIUtility.hotControl != controlID)
                        {
                            return;
                        }
                        if (current.button != 0)
                        { // �������� �� ����� ������ ����
                            return;
                        }

                        if (current.type == EventType.MouseDown)
                        {
                            GUIUtility.hotControl = controlID;
                        }
                        // ���������� ���������
                        if (Physics.Raycast(ray, out hit, float.MaxValue))
                        {
                            if (hit.transform == go.transform)
                            {
                                Vector3 hitPos = Vector3.Scale(go.transform.InverseTransformPoint(hit.point), go.transform.lossyScale);
                                for (int i = 0; i < mesh.vertices.Length; i++)
                                {
                                    Vector3 vertPos = Vector3.Scale(mesh.vertices[i], go.transform.lossyScale);
                                    float mag = (vertPos - hitPos).magnitude;
                                    if (mag > gui_BrushSize)
                                        continue;
                                    debugColors[i] = Color.Lerp(debugColors[i], gui_BrushColor, gui_BrushOpacity);
                                }
                                SetDebugColors();
                            }
                        }
                        current.Use();
                    }
                    break;
                case EventType.MouseUp:
                    {
                        if (!tgl_Paint)
                        {
                            return;
                        }
                        if (GUIUtility.hotControl != controlID)
                        {
                            return;
                        }
                        GUIUtility.hotControl = 0;
                        current.Use();
                    }
                    break;
                case EventType.Repaint:
                    {
                        // ��������� ����� ���������
                        if (Physics.Raycast(ray, out hit, float.MaxValue))
                        {
                            if (hit.transform == go.transform)
                            {
                                Handles.color = new Color(gui_BrushColor.r, gui_BrushColor.g, gui_BrushColor.b, 1.0f);
                                Handles.DrawWireDisc(hit.point, hit.normal, gui_BrushSize, 2f);
                            }
                        }
                        HandleUtility.Repaint();
                    }
                    break;
            }
        }

        private void Initialize()
        {
            // ����� ����������� ������� (���� ����)
            if (go)
            {
                SetOriginalColors();
                SetOriginalShaders();
            }

            // ����� ����������
            go = null;
            coll = null;
            mf = null;
            mesh = null;
            mr = null;
            originalColors = null;
            debugColors = null;
            originalShaders = null;

            // ����� ���������� GUI
            canPaint = false;
            gui_Notification = string.Empty;
            tgl_Paint = false;
            str_Paint = "������ ���������";
            tgl_ShowVertexColors = false;
            str_ShowVertexColors = "�������� �����";
            gui_BrushSize = 0.5f;
            gui_BrushOpacity = 0.5f;
            gui_BrushColor = _g;
        }

        private void Setup()
        {
            Initialize();

            // ��������� ���������� �������
            go = Selection.activeGameObject;
            if (go != null)
            {
                coll = go.GetComponent<Collider>();
                if (coll != null)
                {
                    mf = go.GetComponent<MeshFilter>();
                    if (mf != null)
                    {
                        mesh = mf.sharedMesh;
                        if (mesh != null)
                        {
                            mr = go.GetComponent<MeshRenderer>();
                            // ��������� ������������ ��������
                            GetOriginalShaders();

                            bool isNull = false;
                            for (int i = 0; i < originalShaders.Length; i++)
                            {
                                if (originalShaders[i] == null)
                                    isNull = true;
                            }
                            if (!isNull)
                            {
                                GetOriginalColors();

                                if (originalColors.Length > 0)
                                    gui_Notification = "��� ���������� �������� ������ ������ ������!!";
                                else
                                    gui_Notification = "��� ���������� �� �������� ������ ������ ������!!";

                                // �� � �������, ����� ��������
                                canPaint = true;
                            }
                            else
                                gui_Notification = "������ ����� ������������� ���������/�������!";
                        }
                        else
                            gui_Notification = "������ �� �������� ����!";
                    }
                    else
                        gui_Notification = "������ �� �������� MeshFilter!";
                }
                else
                    gui_Notification = "������ �� �������� MeshCollider!";
            }
            else
                gui_Notification = "������ �� ������!";
        }

        private void SetDebugShaders()
        {
            if (mr == null)
                return;

            Shader debugShader = Shader.Find("TOZ/Debug/VertexColors");

            for (int i = 0; i < mr.sharedMaterials.Length; i++)
            {
                if (mr.sharedMaterials[i] == null)
                    continue;

                mr.sharedMaterials[i].shader = debugShader;
            }
        }

        private void SetOriginalShaders()
        {
            if (mr == null)
                return;

            for (int i = 0; i < mr.sharedMaterials.Length; i++)
            {
                if (mr.sharedMaterials[i] == null)
                    continue;

                mr.sharedMaterials[i].shader = originalShaders[i];
            }
        }

        private void GetOriginalShaders()
        {
            if (mr == null)
                return;

            originalShaders = new Shader[mr.sharedMaterials.Length];

            for (int i = 0; i < originalShaders.Length; i++)
            {
                if (mr.sharedMaterials[i] == null)
                    continue;

                originalShaders[i] = mr.sharedMaterials[i].shader;
            }
        }

        private void SetDebugColors(Color col)
        {
            for (int i = 0; i < debugColors.Length; i++)
            {
                debugColors[i] = col;
            }

            SetDebugColors();
        }

        private void SetDebugColors()
        {
            if (mesh == null)
                return;

            mesh.colors = debugColors;
            EditorUtility.SetDirty(go);
        }

        private void SetOriginalColors()
        {
            if (mesh == null)
                return;

            mesh.colors = originalColors;
            EditorUtility.SetDirty(go);
        }

        private void GetOriginalColors()
        {
            if (mesh.colors != null && mesh.colors.Length > 0)
            {
                originalColors = new Color[mesh.colors.Length];
                debugColors = new Color[originalColors.Length];
                for (int i = 0; i < originalColors.Length; i++)
                {
                    originalColors[i] = mesh.colors[i];
                    debugColors[i] = originalColors[i];
                }
            }
            else
            {
                originalColors = mesh.colors;
                debugColors = new Color[mesh.vertices.Length];
                for (int i = 0; i < debugColors.Length; i++)
                {
                    // originalColors[i] = _w;
                    debugColors[i] = _w;
                }
            }
        }
    }

}
