using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public static class LayerMaskExt
    {

        public static bool Contains(this LayerMask mask, int layer) => 
            mask == (mask | (1 << layer));

    }

}