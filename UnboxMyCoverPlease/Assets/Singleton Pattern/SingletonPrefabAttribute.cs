using System;

namespace FrostBit
{
    /// <summary>
    /// Attribute to give a name for a singleton
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SingletonPrefabAttribute : Attribute
    {
        public readonly string prefabName;

        public SingletonPrefabAttribute(string prefabName) {
            this.prefabName = prefabName;
        }

        public SingletonPrefabAttribute() {
            this.prefabName = null;
        }
    } 
}
