using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public static class Vector3Ext
    {

        public static Vector3 WithY(this Vector3 v, float y) =>
            new Vector3(v.x, y, v.z);
        
        public static Vector3 WithZ(this Vector3 v, float z) =>
            new Vector3(v.x, v.y, z);

        public static Vector3 Flat(this Vector3 v) =>
            v.WithY(0);

    }

}