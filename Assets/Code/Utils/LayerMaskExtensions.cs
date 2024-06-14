using System.Runtime.CompilerServices;
using UnityEngine;

namespace Utils
{
    public static class LayerMaskExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this LayerMask mask, int layer)
        {
            return (mask & (1 << layer)) != 0;
        }
    }
}