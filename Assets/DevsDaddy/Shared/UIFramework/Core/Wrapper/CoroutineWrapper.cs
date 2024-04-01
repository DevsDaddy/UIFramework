using System;
using System.Collections;
using System.Collections.Generic;
using DevsDaddy.Shared.UIFramework.Core.Constants;
using UnityEngine;

namespace DevsDaddy.Shared.UIFramework.Core.Wrapper
{
    /// <summary>
    /// Coroutine Wrapper
    /// </summary>
    public class CoroutineWrapper : MonoBehaviour
    {
        private readonly Dictionary<string, IEnumerator> routines = new Dictionary<string, IEnumerator>();

        /// <summary>
        /// Coroutine Worker Start
        /// </summary>
        private void Start() {
            gameObject.name = GeneralConstants.ROUTINE_WRAPPER_NAME;
            transform.SetParent(null);
            transform.SetAsLastSibling();
            DontDestroyOnLoad(this);
        }

        /// <summary>
        /// Start Coroutine
        /// </summary>
        /// <param name="routineName"></param>
        /// <param name="routine"></param>
        public void StartRoutine(string routineName, IEnumerator routine) {
            if (routines.ContainsKey(routineName)) {
                StopCoroutine(routineName);
                routines.Remove(routineName);
            }
            
            routines.Add(routineName, routine);
            StartCoroutine(routine);
        }

        /// <summary>
        /// Stop Coroutine
        /// </summary>
        /// <param name="routineName"></param>
        public void StopRoutine(string routineName) {
            if(!routines.ContainsKey(routineName))
                return;
            
            StopCoroutine(routines[routineName]);
        }
    }
}