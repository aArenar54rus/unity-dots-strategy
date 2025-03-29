using UnityEditor;
using UnityEngine;


namespace Arenar.PocketFantasyWar
{
    [CustomEditor(typeof(UnitData))]
    public class UnitDataEditor : Editor
    {
        private UnitData unitData;


        private void OnEnable()
        {
            unitData = target as UnitData;
        }
        
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            
            EditorGUILayout.LabelField("Редактировать данные юнита", EditorStyles.boldLabel);
            unitData.unitName = EditorGUILayout.TextField("Unit Name", unitData.unitName);
            unitData.unitIconPath = EditorGUILayout.TextField("Unit Icon Path", unitData.unitIconPath);

            Sprite icon = unitData.UnitIcon;
            if (icon != null)
            {
                GUILayout.Label("Unit Icon Preview:");
                GUILayout.Box(AssetPreview.GetAssetPreview(icon.texture), GUILayout.Width(100), GUILayout.Height(100));
            }
            else
            {
                EditorGUILayout.HelpBox("Не удалось загрузить иконку. Проверьте путь.", MessageType.Warning);
            }

            unitData.unitPrefabPath = EditorGUILayout.TextField("Unit Prefab Path", unitData.unitPrefabPath);
            GameObject prefab = unitData.UnitPrefab;
            if (prefab != null)
            {
                GUI.enabled = false;
                EditorGUILayout.ObjectField("Prefab Preview", prefab, typeof(GameObject), false);
                GUI.enabled = true;
            }
            else
            {
                EditorGUILayout.HelpBox("Не удалось загрузить префаб. Проверьте путь.", MessageType.Warning);
            }
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Характеристики юнита", EditorStyles.boldLabel);
            unitData.unitHealth = EditorGUILayout.IntField("Unit Health", unitData.unitHealth);
            unitData.unitSpeed = EditorGUILayout.FloatField("Unit Speed", unitData.unitSpeed);
            unitData.unitDamage = EditorGUILayout.IntField("Unit Damage", unitData.unitDamage);
            
            EditorGUILayout.Space();
            if (GUILayout.Button("Сохранить изменения"))
            {
                EditorUtility.SetDirty(unitData);
                AssetDatabase.SaveAssets();
                Debug.Log($"UnitData {unitData.unitName} сохранено!");
            }
        }
    }
}