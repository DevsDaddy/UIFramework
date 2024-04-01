using System.Linq;
using DevsDaddy.Shared.UIFramework.Core.ShaderEffect.Common;
using UnityEditor;
using UnityEngine;

namespace DevsDaddy.Shared.UIFramework.Core.ShaderEffect.Editor
{
    internal class MaterialDirtyScope : EditorGUI.ChangeCheckScope
    {
        readonly Object[] targets;

        public MaterialDirtyScope(Object[] targets)
        {
            this.targets = targets;
        }

        protected override void CloseScope()
        {
            if (changed)
            {
                foreach (var effect in targets.OfType<BaseMaterialEffect>())
                {
                    effect.SetMaterialDirty();
                }
            }

            base.CloseScope();
        }
    }
}