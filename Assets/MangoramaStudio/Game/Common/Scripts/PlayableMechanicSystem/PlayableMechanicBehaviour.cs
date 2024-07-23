using System;
using System.Collections;
using UnityEngine;

namespace Mechanics.Scripts
{
    public abstract class PlayableMechanicBehaviour : MonoBehaviour
    {
        public event Action Success;
        public event Action Failure;
        public event Action Attempt;
        public event Action Warn;
        
        private bool _raiseSuccessOnEnable;
        private bool _raiseFailureOnEnable;
        private Coroutine _successRoutine;
        private Coroutine _failureRoutine;

        private void OnEnable()
        {
            if (_raiseSuccessOnEnable)
            {
                RaiseSuccess();
            }
            
            if (_raiseFailureOnEnable)
            {
                RaiseFailure();
            }
            
            
        }

        private void OnDisable()
        {
            if (_successRoutine != null)
            {
                _raiseSuccessOnEnable = true;
                StopCoroutine(_successRoutine);
                _successRoutine = null;
            }
            
            if (_failureRoutine != null)
            {
                _raiseFailureOnEnable = true;
                StopCoroutine(_failureRoutine);
                _failureRoutine = null;
            }
        }

        public abstract void Initialize();
        public abstract void Prepare();

        public virtual void Enable()
        {
            
        }

        public virtual void Disable()
        {
            
        }

    
        public abstract void Dispose();
        
        public virtual void Clear()
        {
            if (_successRoutine != null) StopCoroutine(_successRoutine);
            if (_failureRoutine != null) StopCoroutine(_failureRoutine);

            _successRoutine = null;
            _failureRoutine = null;
            _raiseSuccessOnEnable = false;
            _raiseFailureOnEnable = false;
        }

        protected void RaiseSuccess()
        {
            Success?.Invoke();
        }

        protected void RaiseWarning()
        {
            Warn?.Invoke();
        }

        protected void RaiseFailure()
        {
            Failure?.Invoke();
        }
        
        public virtual void RaiseRestart()
        {
            Clear();
            Prepare();
        }

        protected virtual void RaiseUndo() { }

        protected void RaiseSuccess(float delay)
        {
            _successRoutine = StartCoroutine(DelayedCall(delay, () => Success?.Invoke()));
        }

        protected void RaiseFailure(float delay)
        {
            _failureRoutine = StartCoroutine(DelayedCall(delay, () => Failure?.Invoke()));
        }

        protected void RaiseAttempt()
        {
            Attempt?.Invoke();
        }

        private IEnumerator DelayedCall(float delay, Action callback)
        {
            yield return new WaitForSeconds(delay);
            callback?.Invoke();
        }
    }
}