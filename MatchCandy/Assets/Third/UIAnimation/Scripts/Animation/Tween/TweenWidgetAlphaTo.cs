using System.Collections.Generic;
using UIAnimation.Actions;
using SakashoUISystem;
using UnityEngine;
namespace UIAnimation.Tween
{
    [AddComponentMenu("UIAnimation/Tween/Tween Widget Alpha To")]
    public class TweenWidgetAlphaTo : TweenActionBase
    {
        [SerializeField]
        float toAlpha;
        UIWidget m_UIWidget = null;
        float fromAlpha;

        protected override void Awake()
        {
            base.Awake();
            m_UIWidget = transform.GetComponent<UIWidget>();
            fromAlpha = m_UIWidget.alpha;
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            m_UIWidget.alpha = fromAlpha;
        }

        public override void Prepare()
        {
            base.Prepare();
            m_UIWidget.alpha = fromAlpha;
        }

        protected override void Lerp(float normalizedTime)
        {
            m_UIWidget.alpha = Mathematics.LerpFloat(fromAlpha, ToAlpha, normalizedTime);
        }

        protected override void OnActionIsDone()
        {
            base.OnActionIsDone();
        }

        public float ToAlpha
        {
            get { return toAlpha; }
            set { toAlpha = value; }
        }
    }
}
