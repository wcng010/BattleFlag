using UnityEngine;

namespace C_Script.Data
{
    [CreateAssetMenu(menuName = "Data",fileName = "Data/EntityData")]
    public class EntityDataSo:ScriptableObject
    {
        [field:SerializeField]public int MaxHealth { get; set; }
        [field:SerializeField]public int CurrentHealth { get; set; }
        [field:SerializeField]public int Defense { get; set; }
        [field:SerializeField]public int Attack { get; set; }
        [field: SerializeField] public int Speed { get; set; }
        [field:SerializeField]public int MoveRange { get; set; }//范围值直接定成格子数
        [field: SerializeField] public int AttackRange { get; set; }
    }
}