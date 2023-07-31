using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace HexMap.Editor
{
    public class HexmapPanel : OdinEditorWindow
    {
        [MenuItem("Tools/Hexmap Panel")]
        private static void OpenPanel()
        {
            GetWindow<HexmapPanel>().Show();
        }
        
        public Vector2Int mapSize = new Vector2Int(16, 16);
        public GameObject hexMapPrefab;
        public GameObject hexCellPrefab;
        public GameObject hexTextPrefab;

        [OnInspectorInit]
        void Init()
        {
            hexMapPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/HexMap.prefab", typeof(GameObject)) as GameObject;
            hexCellPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/HexCell.prefab", typeof(GameObject)) as GameObject;
            hexTextPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/HexText.prefab", typeof(GameObject)) as GameObject;
        }
        
        [Button("Create Map")]
        void CreateMap()
        {
            var map = GameObject.Instantiate(hexMapPrefab);
            map.name = "HexMap";
            var hexGrid = map.GetComponent<HexGrid>();
            hexGrid.mapSize = mapSize;
            hexGrid.cells = new HexCell[mapSize.x * mapSize.y];
            Canvas gridCanvas = map.GetComponentInChildren<Canvas>();
            int id = 0;
            for (int z = 0; z < mapSize.x; z++)
            {
                for (int x = 0; x < mapSize.y; x++)
                {
                    #region create hex map

                    Vector3 position;
                    position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
                    position.y = 0f;
                    position.z = z * (HexMetrics.outerRadius * 1.5f);
            
                    HexCell cell  = GameObject.Instantiate(hexCellPrefab).GetComponent<HexCell>();
                    cell.hexCoordinates = HexCoordinates.FromOffsetCoordinates(x, z);
                    cell.name = cell.hexCoordinates.GetHexGameObjectName();
                    cell.transform.SetParent(map.transform, false);
                    cell.transform.localPosition = position;
                    hexGrid.cells[id] = cell;
                    id++;

                    #endregion

                    #region add neighbor

                    if (x > 0)
                    {
                        cell.SetNeighbor(HexDirection.W, hexGrid.cells[id-1]);
                    }

                    if (z > 0)
                    {
                        if ((z & 1) == 0) {
                            cell.SetNeighbor(HexDirection.SE, hexGrid.cells[id - mapSize.x]);
                            if (x > 0) {
                                cell.SetNeighbor(HexDirection.SW, hexGrid.cells[id - mapSize.x - 1]);
                            }
                        }
                        else {
                            cell.SetNeighbor(HexDirection.SW, hexGrid.cells[id - mapSize.x]);
                            if (x < mapSize.x - 1) {
                                cell.SetNeighbor(HexDirection.SE, hexGrid.cells[id - mapSize.x + 1]);
                            }
                        }
                    }

                    #endregion

                    #region show text info

                    GameObject labelObj = Instantiate(hexTextPrefab, gridCanvas.transform);
                    TextMeshProUGUI label = labelObj.GetComponent<TextMeshProUGUI>();
                    label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
                    label.text = cell.hexCoordinates.GetSeparateLineHexText();

                    #endregion
                }
            }


        }

    }
}