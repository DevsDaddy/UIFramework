using System.Collections.Generic;
using DevsDaddy.Shared.UIFramework.Core.ShaderEffect.Effects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DevsDaddy.Shared.UIFramework.Core.ShaderEffect.Common
{
    [RequireComponent(typeof(Graphic))]
    [RequireComponent(typeof(RectTransform))]
    [ExecuteAlways]
    public abstract class BaseMeshEffect : UIBehaviour, IMeshModifier
    {
        RectTransform _rectTransform;
        Graphic _graphic;
        GraphicConnector _connector;
        
        protected GraphicConnector connector
        {
            get { return _connector ?? (_connector = GraphicConnector.FindConnector(graphic)); }
        }
        
        public Graphic graphic
        {
            get { return _graphic ? _graphic : _graphic = GetComponent<Graphic>(); }
        }
        
        protected RectTransform rectTransform
        {
            get { return _rectTransform ? _rectTransform : _rectTransform = GetComponent<RectTransform>(); }
        }

        internal readonly List<UISyncEffect> syncEffects = new List<UISyncEffect>(0);
        
        public virtual void ModifyMesh(Mesh mesh)
        {
        }
        
        public virtual void ModifyMesh(VertexHelper vh)
        {
            ModifyMesh(vh, graphic);
        }

        public virtual void ModifyMesh(VertexHelper vh, Graphic graphic)
        {
        }
        
        protected virtual void SetVerticesDirty()
        {
            connector.SetVerticesDirty(graphic);

            foreach (var effect in syncEffects)
            {
                effect.SetVerticesDirty();
            }
        }
        
        protected override void OnEnable()
        {
            connector.OnEnable(graphic);
            SetVerticesDirty();
        }
        
        protected override void OnDisable()
        {
            connector.OnDisable(graphic);
            SetVerticesDirty();
        }
        
        protected virtual void SetEffectParamsDirty()
        {
            if (!isActiveAndEnabled) return;
            SetVerticesDirty();
        }
        
        protected override void OnDidApplyAnimationProperties()
        {
            if (!isActiveAndEnabled) return;
            SetEffectParamsDirty();
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            if (!isActiveAndEnabled) return;
            SetVerticesDirty();
        }
        
        protected override void OnValidate()
        {
            if (!isActiveAndEnabled) return;
            SetEffectParamsDirty();
        }
#endif
    }
}