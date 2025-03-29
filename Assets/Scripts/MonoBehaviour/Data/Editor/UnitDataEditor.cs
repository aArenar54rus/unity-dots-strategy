using UnityEditor;
using UnityEngine;


namespace Arenar.PocketFantasyWar
{
    [CustomEditor(typeof(UnitData))]
    public class UnitDataEditor : Editor
    {
        private UnitData unitData;
        
        private string[] allowedValuesLabels;


        private void OnEnable()
        {
            allowedValuesLabels = new string[SquadSizes.AllowedValues.Length];
            for (int i = 0; i < SquadSizes.AllowedValues.Length; i++)
                allowedValuesLabels[i] = SquadSizes.AllowedValues[i].ToString();
            
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
            EditorGUILayout.LabelField("Характеристики отряда", EditorStyles.boldLabel);
            int currentIndex = System.Array.IndexOf(SquadSizes.AllowedValues, unitData.unitCountInSquad);
            if (currentIndex == -1)
                currentIndex = 0;
            
            int selectedIndex = EditorGUILayout.Popup("Unit Count In Squad", currentIndex, allowedValuesLabels);
            unitData.unitCountInSquad = SquadSizes.AllowedValues[selectedIndex];
            
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
    
    
    public class IntSliderExample : MonoBehaviour
    {
        [SerializeField]
        private int selectedValue = 1; // Ваше поле для выбора слайдером

        // Список возможных значений для выбора
        public int[] allowedValues = { 1, 4, 9, 16, 25, 36, 49 };

        public int SelectedValue
        {
            get => selectedValue;
            set => selectedValue = Mathf.Clamp(value, allowedValues[0], allowedValues[allowedValues.Length - 1]);
        }
    }
}