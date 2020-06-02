/*
 * WCTools
 * by Vlad Bakholdin and Yevhen Suleimanov
 *
 * You may use these scripts under the terms of the MIT License 
 * See:
 * LICENSE.txt
 */

using UnityEngine;
using System.Collections;
using System;

namespace WCTools
{
    public delegate float UniversalFloatInterpolator(float from, float to);

    public interface IInterpolatorBase
    {
        void OnProc(UniversalFloatInterpolator interpolator);
    }

    public class InterpolatorEmpty : IInterpolatorBase
    {
        void IInterpolatorBase.OnProc(UniversalFloatInterpolator interpolator)
        { }
    }

    public class InterpolatorFloat : IInterpolatorBase
    {
        float _from;
        float _to;
        Action<float> _onInterpolationProc;

        public InterpolatorFloat(float from, float to, Action<float> onInterpolationProc)
        {
            _from = from;
            _to = to;
            _onInterpolationProc = onInterpolationProc;
        }

        void IInterpolatorBase.OnProc(UniversalFloatInterpolator interpolator)
        {
            if (_onInterpolationProc != null)
                _onInterpolationProc(interpolator(_from, _to));
        }
    }

    public class InterpolatorVector2 : IInterpolatorBase
    {
        Vector2 _from;
        Vector2 _to;
        Action<Vector2> _onInterpolationProc;

        public InterpolatorVector2(Vector2 from, Vector2 to, Action<Vector2> onInterpolationProc)
        {
            _from = from;
            _to = to;
            _onInterpolationProc = onInterpolationProc;
        }

        void IInterpolatorBase.OnProc(UniversalFloatInterpolator interpolator)
        {
            if (_onInterpolationProc != null)
                _onInterpolationProc(new Vector2(interpolator(_from.x, _to.x), interpolator(_from.y, _to.y)));
        }
    }

    public class InterpolatorVector3 : IInterpolatorBase
    {
        Vector3 _from;
        Vector3 _to;
        Action<Vector3> _onInterpolationProc;

        public InterpolatorVector3(Vector3 from, Vector3 to, Action<Vector3> onInterpolationProc)
        {
            _from = from;
            _to = to;
            _onInterpolationProc = onInterpolationProc;
        }

        void IInterpolatorBase.OnProc(UniversalFloatInterpolator interpolator)
        {
            if (_onInterpolationProc != null)
                _onInterpolationProc(new Vector3(interpolator(_from.x, _to.x), interpolator(_from.y, _to.y), interpolator(_from.z, _to.z)));
        }
    }

    public class InterpolatorVector4 : IInterpolatorBase
    {
        Vector4 _from;
        Vector4 _to;
        Action<Vector4> _onInterpolationProc;

        public InterpolatorVector4(Vector4 from, Vector4 to, Action<Vector4> onInterpolationProc)
        {
            _from = from;
            _to = to;
            _onInterpolationProc = onInterpolationProc;
        }

        void IInterpolatorBase.OnProc(UniversalFloatInterpolator interpolator)
        {
            if (_onInterpolationProc != null)
                _onInterpolationProc(new Vector4(interpolator(_from.x, _to.x), interpolator(_from.y, _to.y), interpolator(_from.z, _to.z), interpolator(_from.w, _to.w)));
        }
    }

    public class InterpolatorColor : IInterpolatorBase
    {
        Color _from;
        Color _to;
        Action<Color> _onInterpolationProc;

        public InterpolatorColor(Color from, Color to, Action<Color> onInterpolationProc)
        {
            _from = from;
            _to = to;
            _onInterpolationProc = onInterpolationProc;
        }

        void IInterpolatorBase.OnProc(UniversalFloatInterpolator interpolator)
        {
            if (_onInterpolationProc != null)
                _onInterpolationProc(new Color(interpolator(_from.r, _to.r), interpolator(_from.g, _to.g), interpolator(_from.b, _to.b), interpolator(_from.a, _to.a)));
        }
    }

    public class CoroutineInterpolatorInternal
    {
        IInterpolatorBase _interpolatorCore;
        float _epsilon = 0.0001f;
        Action _onInterpolationComplate;
        float _value;
        float _deltaValue;
        Interpolations.eInterpolation _interpolation;
        bool _isComplate;

        public CoroutineInterpolatorInternal(Action onInterpolationComplate, Interpolations.eInterpolation interpolation, IInterpolatorBase interolatorCore)
        {
            _interpolation = interpolation;
            _onInterpolationComplate = onInterpolationComplate;
            _interpolatorCore = interolatorCore;

        }

        public void Init(float time)
        {
            if (time > _epsilon)
            {
                _deltaValue = 1.0f / time;
                _value = 0.0f;
                NotifyProc();
            }
            else
            {
                _isComplate = true;
                _value = 1.0f;
                NotifyProc();
                NotifyComplate();
            }
        }

        public bool IsActive()
        { return !_isComplate; }

        public void Update(float time)
        {
            _value += _deltaValue * time;

            if (_value >= 1.0f)
            {
                _value = 1.0f;
                _isComplate = true;
            }

            NotifyProc();

            if (_isComplate)
                NotifyComplate();
        }

        public void ForcedComplete()
        {
            if (_isComplate)
                return;

            _value = 1.0f;
            _isComplate = true;
            NotifyProc();
            NotifyComplate();
        }

        void NotifyProc()
        { _interpolatorCore.OnProc(UniversalFloatInterpolator); }

        void NotifyComplate()
        {
            if (_onInterpolationComplate != null)
                _onInterpolationComplate();
        }

        float UniversalFloatInterpolator(float from, float to)
        { return Interpolations.Interpolate(from, to, _value, _interpolation); }
    }

    public partial class CoroutineInterpolator
    {
        static IEnumerator InterpolProcUnmanaged(float time, Action onInterpolationComplate, Interpolations.eInterpolation interpolation, IInterpolatorBase interolatorCore)
        {
            CoroutineInterpolatorInternal interpolator = new CoroutineInterpolatorInternal(onInterpolationComplate, interpolation, interolatorCore);
            interpolator.Init(time);

            while (interpolator.IsActive())
            {
                yield return new WaitForEndOfFrame();
                interpolator.Update(Time.deltaTime);
            }
        }

        // ---

        MonoBehaviour _monoBehaviour;
        CoroutineInterpolatorInternal _coroutineInterpolatorInternal = null;
        float _timeScale = 1.0f;

        enum eTime
        {
            TIME_SCALED,
            TIME_UNSCALED,
            TIME_FIXED,
        }

        eTime _timeType = eTime.TIME_SCALED;

        void CoroutineInterpolator_w(MonoBehaviour monoBehaviour)
        { _monoBehaviour = monoBehaviour; }

        void Skip_w()
        { _coroutineInterpolatorInternal = null; }

        void ForcedComplete_w()
        {
            if (_coroutineInterpolatorInternal != null)
            {
                _coroutineInterpolatorInternal.ForcedComplete();
                _coroutineInterpolatorInternal = null;
            }
        }

        void SetTimeScale_w(float timeScale)
        { _timeScale = timeScale; }

        float GetTimeScale_w()
        { return _timeScale; }

        void SetUnscaledTime_w()
        { _timeType = eTime.TIME_UNSCALED; }

        void SetScaledTime_w()
        { _timeType = eTime.TIME_SCALED; }

        void SetFixedTime_w()
        { _timeType = eTime.TIME_FIXED; }

        IEnumerator InterpolProc(float time, Action onInterpolationComplate, Interpolations.eInterpolation interpolation, IInterpolatorBase interolatorCore)
        {
            CoroutineInterpolatorInternal interpolator = _coroutineInterpolatorInternal = new CoroutineInterpolatorInternal(onInterpolationComplate, interpolation, interolatorCore);
            interpolator.Init(time);

            while (interpolator.IsActive())
            {
                if (_timeType != eTime.TIME_FIXED)
                    yield return new WaitForEndOfFrame();
                else
                    yield return new WaitForFixedUpdate();

                if (_coroutineInterpolatorInternal != interpolator)
                    yield break;

                float time_ = 0.0f;
                switch (_timeType)
                {
                    case eTime.TIME_SCALED:
                        time_ = Time.deltaTime;
                        break;
                    case eTime.TIME_UNSCALED:
                        time_ = Time.unscaledDeltaTime;
                        break;
                    case eTime.TIME_FIXED:
                        time_ = Time.fixedDeltaTime;
                        break;
                    default:
                        time_ = 0.0f;
                        break;
                }

                interpolator.Update(time_ * _timeScale);
            }
        }
    }
}