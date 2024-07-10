using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Mechanics.RoboticFlows
{
    public class RoboticFlowBuilder : MonoBehaviour
    {
        [SerializeField] private Cell cell;
        [SerializeField] private Node node;
        [SerializeField] private Grid grid;
        [SerializeField] private FlowDrawer drawer;
        [SerializeField] private RoboticFlowDrawer flowDrawer;
        [SerializeField] private RoboticFlowConfig config;
        [SerializeField] private RoboticFlowHint hint;
        [SerializeField] private float minOrthoSize = 3;
        [SerializeField, HideInInspector] private string json;
        
        [Space]
        #if UNITY_EDITOR
        [OnValueChanged(nameof(OnSizeChanged))]
        #endif
        [SerializeField] private Vector2Int size;
        
        #if UNITY_EDITOR
        [OnValueChanged(nameof(OnMapValueChanged))]
        [TableMatrix(DrawElementMethod = nameof(DrawCell), SquareCells = true), ShowInInspector]
        #endif 
        public int[,] map;
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (map == null)
            {
                var cells = GetComponentsInChildren<Cell>();
                map = string.IsNullOrWhiteSpace(json) ? null : JsonConvert.DeserializeObject<int[,]>(json);

                if (map == null || map.Length == 0)
                {
                    map = new int[size.x, size.y];

                    foreach (var c in cells)
                    {
                        if (c.x < size.x && c.y < size.y)
                        {
                            var value = c.node ? c.node.Id : -1;
                            map[c.x, size.y - c.y - 1] = value;
                        }
                    }

                    json = JsonConvert.SerializeObject(map);
                }
                else
                {
                    var width = map.GetLength(0);
                    var height = map.Length / width;

                    if (width != size.x || height != size.y)
                    {
                        map = new int[size.x, size.y];

                        foreach (var c in cells)
                        {
                            if (c.x < size.x && c.y < size.y)
                            {
                                var value = c.node ? c.node.Id : -1;
                                map[c.x, size.y - c.y - 1] = value;
                            }
                        }

                        json = JsonConvert.SerializeObject(map);
                    }
                }
            }
        }

        [Button(ButtonSizes.Large)]
        [ContextMenu("Construct")]
        private void Construct()
        {
            Destruct();
            var gridSize = CalculateGridSizeAndOffset(out var offset);
            SetCameraOrthoSizeFromGrid(gridSize);
            var cells = CreateCells(offset);
            CreateDrawers();
            CreateHints(cells);
        }

        [Button(ButtonSizes.Large)]
        [ContextMenu("Destruct")]
        private void Destruct()
        {
            var cells = GetComponentsInChildren<Cell>();

            foreach (var c in cells)
            {
                if (UnityEditor.EditorApplication.isPlaying) Destroy(c.gameObject);
                else DestroyImmediate(c.gameObject);
            }

            var drawersRoot = flowDrawer.transform.Find("Drawers");

            if (drawersRoot)
            {
                if (UnityEditor.EditorApplication.isPlaying) Destroy(drawersRoot.gameObject);
                else DestroyImmediate(drawersRoot.gameObject);
            }
            
            flowDrawer.SetDrawers(Array.Empty<FlowDrawer>());
        }

        private List<Cell> CreateCells(Vector3 offset)
        {
            var cells = new List<Cell>();
            
            for (int i = 0; i < size.y; i++)
            {
                for (int j = 0; j < size.x; j++)
                {
                    var xCoord = j;
                    var yCoord = size.y - i - 1;
                    var position = grid.GetCellCenterWorld(new Vector3Int(xCoord, 0, yCoord));
                    var value = map[j, i];

                    if (UnityEditor.EditorApplication.isPlaying)
                    {
                        var instance = Instantiate(cell, position + offset, Quaternion.identity, grid.transform);

                        instance.x = xCoord;
                        instance.y = yCoord;
                        instance.SetColor(config.cellColor);
                        instance.SetOccupiedColor(config.occupiedCellColor);

                        if (value > -1 && value < config.colors.Count)
                        {
                            instance.node = Instantiate(node, position + offset, Quaternion.identity, instance.transform);
                            instance.node.SetColor(config.colors[value]);
                            instance.node.SetId(value);
                        }
                        
                        cells.Add(instance);
                    }
                    else
                    {
                        var instance = UnityEditor.PrefabUtility.InstantiatePrefab(cell) as Cell;

                        if (instance)
                        {
                            instance.x = xCoord;
                            instance.y = yCoord;
                            instance.SetColor(config.cellColor);
                            instance.SetOccupiedColor(config.occupiedCellColor);
                            instance.transform.position = position + offset;
                            instance.transform.SetParent(grid.transform, true);

                            if (value > -1 && value < config.colors.Count)
                            {
                                instance.node = UnityEditor.PrefabUtility.InstantiatePrefab(node) as Node;
                                instance.node.transform.position = instance.transform.position;
                                instance.node.transform.rotation = instance.transform.rotation;
                                instance.node.transform.SetParent(instance.transform, true);
                                instance.node.SetColor(config.colors[value]);
                                instance.node.SetId(value);
                            }
                        
                            cells.Add(instance);
                        }
                    }
                }
            }

            return cells;
        }

        private void CreateDrawers()
        {
            var drawersTransform = transform.Find("Drawers");

            if (!drawersTransform)
            {
                drawersTransform = new GameObject("Drawers").transform;
                var localPos = drawersTransform.localPosition;
                localPos.y = 0.5f;
                drawersTransform.localPosition = localPos;
            }
            
            drawersTransform.SetParent(flowDrawer.transform);

            var list = new List<FlowDrawer>();

            for (int i = 0; i < size.y; i++)
            {
                for (int j = 0; j < size.x; j++)
                {
                    var value = map[j, i];

                    if (value < 0 || value >= config.colors.Count) 
                        continue;

                    if (list.Any(d => d.Id == value))
                        continue;
                    
                    if (UnityEditor.EditorApplication.isPlaying)
                    {
                        var instance = Instantiate(drawer, drawersTransform);
                        
                        instance.SetColor(config.colors[value]);
                        instance.SetId(value);
                        
                        list.Add(instance);
                    }
                    else
                    {
                        var instance = UnityEditor.PrefabUtility.InstantiatePrefab(drawer) as FlowDrawer;
                        
                        instance.transform.SetParent(drawersTransform.transform);
                        instance.transform.localPosition = Vector3.zero;
                        instance.SetColor(config.colors[value]);
                        instance.SetId(value);
                        
                        list.Add(instance);
                    }
                }
            }
            
            flowDrawer.SetDrawers(list);
        }

        private void CreateHints(List<Cell> cells)
        {
            var hints = new Dictionary<int, RoboticFlowHint.Hint>();

            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    var value = map[j, i];

                    if (value < 0) 
                        continue;

                    value %= 1000;

                    if (!hints.TryGetValue(value, out var hint))
                    {
                        hint = new RoboticFlowHint.Hint();
                        hint.id = value;
                        var color = config.colors[value];
                        color.a = 0.2f;
                        hint.color = color;
                        hint.cells = new List<Cell>();
                        hints.Add(value, hint);
                    }
                    
                    var xCoord = j;
                    var yCoord = size.y - i - 1;
                    
                    hint.cells.Add(cells.First(c => c.x == xCoord && c.y == yCoord));
                }
            }
            
            hint.SetHints(hints.Values.ToList());
        }

        private Vector3 CalculateGridSizeAndOffset(out Vector3 offset)
        {
            var gridSize = new Vector3(
                size.x * grid.cellSize.x + (size.x - 1) * grid.cellGap.x,
                0,
                size.y * grid.cellSize.z + (size.y - 1) * grid.cellGap.z
            );

            offset = -0.5f * gridSize;
            return gridSize;
        }

        private void SetCameraOrthoSizeFromGrid(Vector3 gridSize)
        {
            var mainCamera = Camera.main;
            var orthographicSize = gridSize.x / (2 * mainCamera.aspect);

            orthographicSize = Mathf.Max(minOrthoSize, orthographicSize);

            if (mainCamera)
            {
                mainCamera.orthographicSize = orthographicSize;
            }

            if (grid.TryGetComponent(out FlowGrid flowGrid))
            {
                var serialized = new UnityEditor.SerializedObject(flowGrid);
                var orthoSize = serialized.FindProperty("orthoSize");
                
                orthoSize.floatValue = orthographicSize;

                serialized.ApplyModifiedProperties();
            }
        }

        private int DrawCell(Rect rect, int value)
        {
            if (!config) return value;

            var rowCount = Mathf.CeilToInt(config.colors.Count / 3f);
            var drawRect = rect.Padding(5);

            if (value < 0)
            {
                var width = drawRect.width / 3;
                var height = drawRect.height / rowCount;

                for (int i = 0; i < config.colors.Count; i++)
                {
                    var rectPosition = drawRect.position;
                    var rectSize = new Vector2(width, height);

                    rectPosition.x += (i % 3) * width;
                    rectPosition.y += (i / 3) * height;
                
                    var r = new Rect(rectPosition, rectSize);

                    if (CheckMouse(0, r))
                    {
                        value = Count(i) > 1 ? i + 1000 : i;
                        GUI.changed = true;
                        Event.current.Use();
                    }
                
                    UnityEditor.EditorGUI.DrawRect(r.Padding(1), config.colors[i]);
                }
            }
            else
            {
                var colorValue = value % 1000;

                if (CheckMouse(0, drawRect))
                {
                    value = -1;
                    GUI.changed = true;
                    Event.current.Use();
                }

                if (value >= 1000)
                {
                    UnityEditor.EditorGUI.DrawRect(drawRect.Padding(15), config.colors[colorValue]);
                }
                else
                {
                    UnityEditor.EditorGUI.DrawRect(drawRect, config.colors[colorValue]);
                }
            }

            return value;
        }

        private void OnMapValueChanged()
        {
            json = JsonConvert.SerializeObject(map);
        }
        
        private void OnSizeChanged()
        {
            map = new int[size.x, size.y];

            for (int i = 0; i < size.y; i++)
            {
                for (int j = 0; j < size.x; j++)
                {
                    map[j, i] = -1;
                }
            }
        }

        private int Count(int reference)
        {
            var c = 0;
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    var value = map[i, j];
                    if (value == reference)
                        ++c;
                }
            }
            return c;
        }

        private bool CheckMouse(int mouseButton, Rect rect)
        {
            return Event.current.type == EventType.MouseDown &&
                   Event.current.button == 0 &&
                   rect.Contains(Event.current.mousePosition);
        }
        #endif
    }
}