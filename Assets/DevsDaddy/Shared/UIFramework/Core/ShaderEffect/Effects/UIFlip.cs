using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BaseMeshEffect = DevsDaddy.Shared.UIFramework.Core.ShaderEffect.Common.BaseMeshEffect;

namespace DevsDaddy.Shared.UIFramework.Core.ShaderEffect.Effects
{
    [DisallowMultipleComponent]
    [AddComponentMenu("UIFramework/Effects/Flip", 102)]
    public class UIFlip : BaseMeshEffect
    {
        [Tooltip("Flip horizontally.")] [SerializeField]
        private bool m_Horizontal = false;

        [Tooltip("Flip vertically.")] [SerializeField]
        private bool m_Veritical = false;
        
        public bool horizontal
        {
            get { return m_Horizontal; }
            set
            {
                if (m_Horizontal == value) return;
                m_Horizontal = value;
                SetEffectParamsDirty();
            }
        }
        
        public bool vertical
        {
            get { return m_Veritical; }
            set
            {
                if (m_Veritical == value) return;
                m_Veritical = value;
                SetEffectParamsDirty();
            }
        }
        
        public override void ModifyMesh(VertexHelper vh, Graphic graphic)
        {
            if (!isActiveAndEnabled) return;

            var vt = default(UIVertex);
            for (var i = 0; i < vh.currentVertCount; i++)
            {
                vh.PopulateUIVertex(ref vt, i);
                var pos = vt.position;
                vt.position = new Vector3(
                    m_Horizontal ? -pos.x : pos.x,
                    m_Veritical ? -pos.y : pos.y
                );
                vh.SetUIVertex(vt, i);
            }
        }
    }
}