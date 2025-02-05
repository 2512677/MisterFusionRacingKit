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
        // Переменные окна редактора
        private static VertexPainter window;

        // Меню для создания окна Vertex Painter в Unity
        [MenuItem("MR Fusuin Engine/Инструменты/Vertex Painter")]
        private static void CreateWindow()
        {
            // Инициализация окна
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
        // Цветовые константы
        private static Color _r = new Color(1f, 0f, 0f, 0f); // Красный
        private static Color _g = new Color(0f, 1f, 0f, 0f); // Зелёный
        private static Color _b = new Color(0f, 0f, 1f, 0f); // Синий
        private static Color _a = new Color(0f, 0f, 0f, 1f); // Альфа
        private static Color _w = new Color(1f, 1f, 1f, 1f); // Белый

        // Ссылки на объекты и компоненты
        private GameObject go;
        private Collider coll;
        private MeshFilter mf;
        private Mesh mesh;
        private MeshRenderer mr;
        private Color[] originalColors, debugColors;
        private Shader[] originalShaders;

        // Переменные для GUI
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

        // Методы MonoBehaviour
        private void OnEnable()
        {
            // Подписка на событие отображения GUI сцены
            SceneView.duringSceneGui += OnSceneGUI;

            Setup();
        }

        private void OnDisable()
        {
            Initialize();

            // Отписка от события
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnSelectionChange()
        {
            // Обработка изменения выбора объекта
            Setup();
            this.Repaint();
        }

        private void OnProjectChange()
        {
            // Обработка изменения проекта
            Setup();
            this.Repaint();
        }

        private void OnInspectorUpdate()
        {
            // Обновление инспектора
            this.Repaint();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            // Предупреждения
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
                    // Изменение текста кнопки
                    str_Paint = "ПРЕКРАТИТЬ РИСОВАНИЕ";
                    // Изменение состояния другой кнопки
                    tgl_ShowVertexColors = true;
                    str_ShowVertexColors = "СКРЫТЬ ЦВЕТА";

                    // Установка отладочных шейдеров
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
                    // Изменение текста кнопки
                    str_ShowVertexColors = "СКРЫТЬ ЦВЕТА";

                    // Установка отладочных шейдеров
                    SetDebugShaders();
                }
                else
                {
                    // Изменение текста кнопки
                    str_ShowVertexColors = "ПОКАЗАТЬ ЦВЕТА";

                    // Восстановление оригинальных шейдеров
                    SetOriginalShaders();
                }
            }

            if (GUILayout.Button("?", GUILayout.Width(22)))
            {
                // Открытие документации в браузере
                Application.OpenURL("https://tozlab.com/documentation/toz-vertex-painter");
            }
            EditorGUILayout.EndHorizontal();

            if (tgl_Paint)
            {
                // Верхняя часть GUI
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical("box");

                EditorGUIUtility.labelWidth = 105f;
                gui_BrushSize = EditorGUILayout.Slider("Размер кисти:", gui_BrushSize, 0.1f, 10.0f);
                gui_BrushOpacity = EditorGUILayout.Slider("Непрозрачность кисти:", gui_BrushOpacity, 0.0f, 1.0f);
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Цвет кисти:");
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

                // Центральная часть GUI
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.PrefixLabel("Цвета вершин:");
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
                if (GUILayout.Button("СБРОС", GUILayout.Width(70)))
                {
                    SetOriginalColors();
                }
                EditorGUILayout.EndHorizontal();

                // Нижняя часть GUI
                EditorGUILayout.Space();
                if (GUILayout.Button("СОХРАНИТЬ НОВУЮ МЕШ"))
                {
                    string file = EditorUtility.SaveFilePanelInProject("Сохранить меш", "New Mesh", "mesh", "Пожалуйста, введите имя файла");
                    //string file = AssetDatabase.GenerateUniqueAssetPath("Assets/New Mesh.mesh");
                    if (file.Length != 0)
                    {
                        // Создание экземпляра и сохранение как новый меш
                        Mesh data = (Mesh)Instantiate(mesh);

                        if (EditorUtility.DisplayDialog("Оптимизировать?", "Оптимизировать меш и перестроить буферы вершин/индексов перед сохранением?", "Да", "Нет"))
                        {
                            MeshUtility.Optimize(data);
                        }

                        AssetDatabase.CreateAsset(data, file);
                        AssetDatabase.SaveAssets();
                        Debug.LogWarning("Меш сохранён по адресу: " + file);
                        // Восстановление оригинальных цветов меша
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

        // Методы
        private void OnSceneGUI(SceneView sceneView)
        {
            if (!tgl_Paint)
            {
                return;
            }

            Event current = Event.current;
            Ray ray = HandleUtility.GUIPointToWorldRay(current.mousePosition);
            RaycastHit hit;
            // События
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
                            // Можно добавить дополнительную логику при необходимости
                        }
                        if (current.GetTypeForControl(controlID) == EventType.MouseDrag && GUIUtility.hotControl != controlID)
                        {
                            return;
                        }
                        if (current.button != 0)
                        { // Проверка на левую кнопку мыши
                            return;
                        }

                        if (current.type == EventType.MouseDown)
                        {
                            GUIUtility.hotControl = controlID;
                        }
                        // Выполнение рисования
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
                        // Отрисовка кисти рисования
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
            // Сброс предыдущего объекта (если есть)
            if (go)
            {
                SetOriginalColors();
                SetOriginalShaders();
            }

            // Сброс переменных
            go = null;
            coll = null;
            mf = null;
            mesh = null;
            mr = null;
            originalColors = null;
            debugColors = null;
            originalShaders = null;

            // Сброс переменных GUI
            canPaint = false;
            gui_Notification = string.Empty;
            tgl_Paint = false;
            str_Paint = "НАЧАТЬ РИСОВАНИЕ";
            tgl_ShowVertexColors = false;
            str_ShowVertexColors = "ПОКАЗАТЬ ЦВЕТА";
            gui_BrushSize = 0.5f;
            gui_BrushOpacity = 0.5f;
            gui_BrushColor = _g;
        }

        private void Setup()
        {
            Initialize();

            // Установка выбранного объекта
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
                            // Получение оригинальных шейдеров
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
                                    gui_Notification = "Меш изначально содержит данные цветов вершин!!";
                                else
                                    gui_Notification = "Меш изначально НЕ содержит данных цветов вершин!!";

                                // Всё в порядке, можно рисовать
                                canPaint = true;
                            }
                            else
                                gui_Notification = "Объект имеет отсутствующие материалы/шейдеры!";
                        }
                        else
                            gui_Notification = "Объект не содержит меша!";
                    }
                    else
                        gui_Notification = "Объект не содержит MeshFilter!";
                }
                else
                    gui_Notification = "Объект не содержит MeshCollider!";
            }
            else
                gui_Notification = "Объект не выбран!";
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
