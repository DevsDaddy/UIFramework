using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevsDaddy.Shared.UIFramework.Core.ShaderEffect.Common
{
    public interface IParameterTexture
    {
        int parameterIndex { get; set; }

        ParameterTexture paramTex { get; }
    }
    
    [System.Serializable]
    public class ParameterTexture
    {
        Texture2D _texture;
        bool _needUpload;
        int _propertyId;
        readonly string _propertyName;
        readonly int _channels;
        readonly int _instanceLimit;
        readonly byte[] _data;
        readonly Stack<int> _stack;
        static List<Action> updates;
        
        public ParameterTexture(int channels, int instanceLimit, string propertyName)
        {
            _propertyName = propertyName;
            _channels = ((channels - 1) / 4 + 1) * 4;
            _instanceLimit = ((instanceLimit - 1) / 2 + 1) * 2;
            _data = new byte[_channels * _instanceLimit];

            _stack = new Stack<int>(_instanceLimit);
            for (int i = 1; i < _instanceLimit + 1; i++)
            {
                _stack.Push(i);
            }
        }
        
        public void Register(IParameterTexture target)
        {
            Initialize();
            if (target.parameterIndex <= 0 && 0 < _stack.Count)
            {
                target.parameterIndex = _stack.Pop();
            }
        }
        
        public void Unregister(IParameterTexture target)
        {
            if (0 < target.parameterIndex)
            {
                _stack.Push(target.parameterIndex);
                target.parameterIndex = 0;
            }
        }
        
        public void SetData(IParameterTexture target, int channelId, byte value)
        {
            int index = (target.parameterIndex - 1) * _channels + channelId;
            if (0 < target.parameterIndex && _data[index] != value)
            {
                _data[index] = value;
                _needUpload = true;
            }
        }
        
        public void SetData(IParameterTexture target, int channelId, float value)
        {
            SetData(target, channelId, (byte) (Mathf.Clamp01(value) * 255));
        }
        
        public void RegisterMaterial(Material mat)
        {
            if (_propertyId == 0)
            {
                _propertyId = Shader.PropertyToID(_propertyName);
            }

            if (mat)
            {
                mat.SetTexture(_propertyId, _texture);
            }
        }
        
        public float GetNormalizedIndex(IParameterTexture target)
        {
            return ((float) target.parameterIndex - 0.5f) / _instanceLimit;
        }
        
        void Initialize()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying && UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }
#endif
            if (updates == null)
            {
                updates = new List<Action>();
                Canvas.willRenderCanvases += () =>
                {
                    var count = updates.Count;
                    for (int i = 0; i < count; i++)
                    {
                        updates[i].Invoke();
                    }
                };
            }

            if (!_texture)
            {
                bool isLinear = QualitySettings.activeColorSpace == ColorSpace.Linear;
                _texture = new Texture2D(_channels / 4, _instanceLimit, TextureFormat.RGBA32, false, isLinear);
                _texture.filterMode = FilterMode.Point;
                _texture.wrapMode = TextureWrapMode.Clamp;

                updates.Add(UpdateParameterTexture);
                _needUpload = true;
            }
        }
        
        void UpdateParameterTexture()
        {
            if (_needUpload && _texture)
            {
                _needUpload = false;
                _texture.LoadRawTextureData(_data);
                _texture.Apply(false, false);
            }
        }
    }
}