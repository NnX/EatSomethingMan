/*
 * WCTools
 * by Vlad Bakholdin and Yevhen Suleimanov
 *
 * You may use these scripts under the terms of the MIT License 
 * See:
 * LICENSE.txt
 */

using UnityEngine;
using System;

namespace WCTools
{
    public partial class CoroutineInterpolator
    {
        public static Coroutine InterpolateUnmanaged(MonoBehaviour monoBehaviour, float startValue, float complateValue, float time, Action<float> onInterpolationProc, Action onInterpolationComplate = null, Interpolations.eInterpolation interpolation = Interpolations.eInterpolation.INTERPOLATE_TYPE_LINEAR)
        { return monoBehaviour.StartCoroutine(InterpolProcUnmanaged(time, onInterpolationComplate, interpolation, new InterpolatorFloat(startValue, complateValue, onInterpolationProc))); }

        public static Coroutine InterpolateUnmanaged(MonoBehaviour monoBehaviour, Vector2 startValue, Vector2 complateValue, float time, Action<Vector2> onInterpolationProc, Action onInterpolationComplate = null, Interpolations.eInterpolation interpolation = Interpolations.eInterpolation.INTERPOLATE_TYPE_LINEAR)
        { return monoBehaviour.StartCoroutine(InterpolProcUnmanaged(time, onInterpolationComplate, interpolation, new InterpolatorVector2(startValue, complateValue, onInterpolationProc))); }

        public static Coroutine InterpolateUnmanaged(MonoBehaviour monoBehaviour, Vector3 startValue, Vector3 complateValue, float time, Action<Vector3> onInterpolationProc, Action onInterpolationComplate = null, Interpolations.eInterpolation interpolation = Interpolations.eInterpolation.INTERPOLATE_TYPE_LINEAR)
        { return monoBehaviour.StartCoroutine(InterpolProcUnmanaged(time, onInterpolationComplate, interpolation, new InterpolatorVector3(startValue, complateValue, onInterpolationProc))); }

        public static Coroutine InterpolateUnmanaged(MonoBehaviour monoBehaviour, Vector4 startValue, Vector4 complateValue, float time, Action<Vector4> onInterpolationProc, Action onInterpolationComplate = null, Interpolations.eInterpolation interpolation = Interpolations.eInterpolation.INTERPOLATE_TYPE_LINEAR)
        { return monoBehaviour.StartCoroutine(InterpolProcUnmanaged(time, onInterpolationComplate, interpolation, new InterpolatorVector4(startValue, complateValue, onInterpolationProc))); }

        public static Coroutine InterpolateUnmanaged(MonoBehaviour monoBehaviour, Color startValue, Color complateValue, float time, Action<Color> onInterpolationProc, Action onInterpolationComplate = null, Interpolations.eInterpolation interpolation = Interpolations.eInterpolation.INTERPOLATE_TYPE_LINEAR)
        { return monoBehaviour.StartCoroutine(InterpolProcUnmanaged(time, onInterpolationComplate, interpolation, new InterpolatorColor(startValue, complateValue, onInterpolationProc))); }

        public Coroutine Interpolate(float startValue, float complateValue, float time, Action<float> onInterpolationProc, Action onInterpolationComplate = null, Interpolations.eInterpolation interpolation = Interpolations.eInterpolation.INTERPOLATE_TYPE_LINEAR)
        { return _monoBehaviour.StartCoroutine(InterpolProc(time, onInterpolationComplate, interpolation, new InterpolatorFloat(startValue, complateValue, onInterpolationProc))); }

        public Coroutine Interpolate(Vector2 startValue, Vector2 complateValue, float time, Action<Vector2> onInterpolationProc, Action onInterpolationComplate = null, Interpolations.eInterpolation interpolation = Interpolations.eInterpolation.INTERPOLATE_TYPE_LINEAR)
        { return _monoBehaviour.StartCoroutine(InterpolProc(time, onInterpolationComplate, interpolation, new InterpolatorVector2(startValue, complateValue, onInterpolationProc))); }

        public Coroutine Interpolate(Vector3 startValue, Vector3 complateValue, float time, Action<Vector3> onInterpolationProc, Action onInterpolationComplate = null, Interpolations.eInterpolation interpolation = Interpolations.eInterpolation.INTERPOLATE_TYPE_LINEAR)
        { return _monoBehaviour.StartCoroutine(InterpolProc(time, onInterpolationComplate, interpolation, new InterpolatorVector3(startValue, complateValue, onInterpolationProc))); }

        public Coroutine Interpolate(Vector4 startValue, Vector4 complateValue, float time, Action<Vector4> onInterpolationProc, Action onInterpolationComplate = null, Interpolations.eInterpolation interpolation = Interpolations.eInterpolation.INTERPOLATE_TYPE_LINEAR)
        { return _monoBehaviour.StartCoroutine(InterpolProc(time, onInterpolationComplate, interpolation, new InterpolatorVector4(startValue, complateValue, onInterpolationProc))); }

        public Coroutine Interpolate(Color startValue, Color complateValue, float time, Action<Color> onInterpolationProc, Action onInterpolationComplate = null, Interpolations.eInterpolation interpolation = Interpolations.eInterpolation.INTERPOLATE_TYPE_LINEAR)
        { return _monoBehaviour.StartCoroutine(InterpolProc(time, onInterpolationComplate, interpolation, new InterpolatorColor(startValue, complateValue, onInterpolationProc))); }

        public static Coroutine TimerUnmanaged(MonoBehaviour monoBehaviour, float time, Action onComplate)
        { return monoBehaviour.StartCoroutine(InterpolProcUnmanaged(time, onComplate, Interpolations.eInterpolation.INTERPOLATE_TYPE_LINEAR, new InterpolatorEmpty())); }

        public Coroutine Timer(float time, Action onComplate)
        { return _monoBehaviour.StartCoroutine(InterpolProc(time, onComplate, Interpolations.eInterpolation.INTERPOLATE_TYPE_LINEAR, new InterpolatorEmpty())); }

        public CoroutineInterpolator(MonoBehaviour monoBehaviour)
        { CoroutineInterpolator_w(monoBehaviour); }

        public void Skip()
        { Skip_w(); }

        public void ForcedComplete()
        { ForcedComplete_w(); }

        public void SetTimeScale(float timeScale)
        { SetTimeScale_w(timeScale); }

        public float GetTimeScale()
        { return GetTimeScale_w(); }

        public void SetUnscaledTime()
        { SetUnscaledTime_w(); }

        public void SetScaledTime()
        { SetScaledTime_w(); }

        public void SetFixedTime()
        { SetFixedTime_w(); }
    }
}
