using UnityEngine;


namespace Arenar.PocketFantasyWar
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "PocketFantasyWar/CharacterData")]
    public class UnitData : ScriptableObject
    {
        public string unitName;
        public string unitIconPath;
        public string unitPrefabPath;
        
        public int unitHealth;
        public float unitSpeed;
        public int unitDamage;


        public string UnitName => unitName;
        public Sprite UnitIcon => string.IsNullOrEmpty(unitIconPath) ? null : Resources.Load<Sprite>(unitIconPath);
        public string UnitPrefabPath => unitPrefabPath;
        public GameObject UnitPrefab => Resources.Load<GameObject>(unitPrefabPath);
        public int UnitHealth => unitHealth;
        public float UnitSpeed => unitSpeed;
        public int UnitDamage => unitDamage;
    }
}