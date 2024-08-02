using System;
using UnityEngine;

namespace Code.Digging.Garden
{
    [Serializable]
    public class GardenInfo 
    {
        [field: SerializeField] public Vector2Int Size{ get; private set; }
        [field: SerializeField] public int Cost { get; private set; }
        //Текущее количество грядок на складе данного типа.
        //Кмк излишняя логика, и должна быть в другом месте.
        [field: SerializeField] public int Count { get; private set; }
    }
}
