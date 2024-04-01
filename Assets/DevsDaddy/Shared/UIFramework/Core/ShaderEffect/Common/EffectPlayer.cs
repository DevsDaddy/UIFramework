using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevsDaddy.Shared.UIFramework.Core.ShaderEffect.Common
{
    [Serializable]
    public class EffectPlayer
    {
        [Header("Effect Player")] [Tooltip("Playing.")]
        public bool play = false;
        
        [Tooltip("Initial play delay.")] [Range(0f, 10f)]
        public float initialPlayDelay = 0;
        
        [Tooltip("Duration.")] [Range(0.01f, 10f)]
        public float duration = 1;
        
        [Tooltip("Loop.")] public bool loop = false;
        
        [Tooltip("Delay before looping.")] [Range(0f, 10f)]
        public float loopDelay = 0;
        
        [Tooltip("Update mode")] public AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal;
        
        static List<Action> s_UpdateActions;
        float _time = 0;
        Action<float> _callback;
        
        public void OnEnable(Action<float> callback = null)
        {
            if (s_UpdateActions == null)
            {
                s_UpdateActions = new List<Action>();
                Canvas.willRenderCanvases += () =>
                {
                    var count = s_UpdateActions.Count;
                    for (int i = 0; i < count; i++)
                    {
                        s_UpdateActions[i].Invoke();
                    }
                };
            }

            s_UpdateActions.Add(OnWillRenderCanvases);

            if (play)
            {
                _time = -initialPlayDelay;
            }
            else
            {
                _time = 0;
            }

            _callback = callback;
        }
        
        public void OnDisable()
        {
            _callback = null;
            s_UpdateActions.Remove(OnWillRenderCanvases);
        }
        
        public void Play(bool reset, Action<float> callback = null)
        {
            if (reset)
            {
                _time = 0;
            }

            play = true;
            if (callback != null)
            {
                _callback = callback;
            }
        }
        
        public void Stop(bool reset)
        {
            if (reset)
            {
                _time = 0;
                if (_callback != null)
                {
                    _callback(_time);
                }
            }

            play = false;
        }
        
        void OnWillRenderCanvases()
        {
            if (!play || !Application.isPlaying || _callback == null)
            {
                return;
            }

            _time += updateMode == AnimatorUpdateMode.UnscaledTime
                ? Time.unscaledDeltaTime
                : Time.deltaTime;
            var current = _time / duration;

            if (duration <= _time)
            {
                play = loop;
                _time = loop ? -loopDelay : 0;
            }

            _callback(current);
        }
    }
}