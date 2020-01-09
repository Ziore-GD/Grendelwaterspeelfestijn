    using System.Collections.Generic;
    using System.Collections;
    using Unity.Collections;
    using UnityEngine;

    public class CameraShake : MonoBehaviour {
        private Vector3 _originalPos;
        private float _shakeAmount = .25f;

        public IEnumerator Shake () {
            _originalPos = transform.localPosition;
            float shakeTime = 3f;
            while (shakeTime > 0) {
                shakeTime -= Time.fixedDeltaTime;
                transform.localPosition = _originalPos + (Vector3) UnityEngine.Random.insideUnitCircle * _shakeAmount;
                yield return null;
            }

            transform.localPosition = _originalPos;
        }

        public void DoShake(){
            StartCoroutine(Shake());
        }
    }