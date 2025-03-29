using UnityEngine;


namespace Arenar.PocketFantasyWar
{
    public class UnitDataContainer : MonoBehaviour
    {
        [SerializeField]
        private UnitData unitData;


        public UnitData UnitData => unitData;
    }
}