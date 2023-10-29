using AnimationPro.RunTime;
using Codice.Client.BaseCommands;
using Core.Utility.UI;
using Core.Utility.UI.Navigation;
using System;
using TMPro;
using Unity.Plastic.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Screen.InGame
{
    public class InGameManager: UIManager
    {
        // hp
        [SerializeField] private TextMeshProUGUI currentHp;
        [SerializeField] private TextMeshProUGUI maxHp;
        [SerializeField] private Slider sliderHp;
        [SerializeField] private Image gaugeHpBackground;
        [SerializeField] private Image HandleBackground;
        private uint? maxHpValue;
        
        // progress
        [SerializeField] private TextMeshProUGUI currentProgress;
        
        // resource
        [SerializeField] private TextMeshProUGUI currentRes;
        [SerializeField] private TextMeshProUGUI maxRes;
        [SerializeField] private Image gaugeRes;
        private uint? maxResValue;

        // color
        // 最大HPの色　x:Hue, y:Satulation, z:Value
        private Vector3 maxHpHSV = new(109 / 360f, 1f, 1f);
        private Vector3 zeroHpHSV = new(0f, 1f, 0.74f);

        private const float StartOffsetAmount = 0.37f;
        private const float EndOffsetAmount = 0.63f;

        // workers
        [SerializeField] private TextMeshProUGUI currentWorkers;
        [SerializeField] private AnimateObject workersIcon;
        private uint? maxWorkersValue;
        [SerializeField] private float startOffsetY;// -205
        [SerializeField] private float endOffsetY; // -37

        public override void Select(Vector2 direction)
        {
            // NOP.
        }

        public override void Clicked()
        {
            // NOP.
        }

        public void SetHp(uint current, uint? max = null)
        {
            if (max.HasValue)
            {
                maxHpValue = max.Value;
                maxHp.text = max.Value.ToString();
                sliderHp.maxValue = max.Value;
                sliderHp.minValue = 0;
            }

            if (!maxHpValue.HasValue)
            {
                throw new MismatchedNotSetException();
            }

            if (current > maxHpValue.Value) return;

            currentHp.text = current.ToString();
            sliderHp.value = current;
            //gaugeHpBackground.color = current switch {
            //    _ when current > maxHpValue.Value * 0.3f => Color.green,
            //    _ => Color.red
            //};
            var t = (float)current / (float)maxHpValue;
            Func<Vector3, Vector3, float, Color> changeColor = (zeroHpHSV, maxHpHSV, t) =>
            {
                Vector3 lifeColorHSV = new(zeroHpHSV.x+((maxHpHSV.x - zeroHpHSV.x)*t), zeroHpHSV.y+((maxHpHSV.y - zeroHpHSV.y)*t), zeroHpHSV.z+((maxHpHSV.z - zeroHpHSV.z)*t));
                Color color = Color.HSVToRGB(lifeColorHSV.x, lifeColorHSV.y, lifeColorHSV.z);
                Debug.Log(color);
                return color;
            };
            gaugeHpBackground.color = changeColor(zeroHpHSV, maxHpHSV, t);
            HandleBackground.color = changeColor(zeroHpHSV, maxHpHSV, t);
        }

        public void SetStageProgress(uint value)
        {
            currentProgress.text = value.ToString();
        }

        public void SetResource(uint current, uint? max = null)
        {
            if (max.HasValue)
            {
                maxResValue = max.Value;
                maxRes.text = max.Value.ToString();
            }

            if (!maxResValue.HasValue)
            {
                throw new MismatchedNotSetException();
            }

            if (current > maxResValue.Value) return;

            currentRes.text = current.ToString();
            float rate = (float)current / maxResValue.Value;
            gaugeRes.fillAmount = StartOffsetAmount + (EndOffsetAmount - StartOffsetAmount) * rate;
        }

        public void SetWorkerCount(uint value, uint? max = null)
        {
            if (max.HasValue)
            {
                maxWorkersValue = max.Value;
            }
            
            if (!maxWorkersValue.HasValue)
            {
                throw new MismatchedNotSetException();
            }
            
            if (value > maxWorkersValue.Value) return;

            currentWorkers.text = value.ToString();
            

           UpdateWorkersMoviePosition(value);
        }
        
        // set position background raw image 
        private void UpdateWorkersMoviePosition(uint value)
        {
            if (!maxWorkersValue.HasValue || !workersIcon.rectTransform) return;
            Vector3 position = workersIcon.rectTransform.localPosition;
            float offsetRate = endOffsetY - startOffsetY;
            float updatePositionY = startOffsetY + offsetRate * ((float)value / maxWorkersValue.Value);
            
            workersIcon.OnCancel();
            workersIcon.Animation(
                workersIcon.SlideTo(updatePositionY - position.y, AnimationAPI.SlideDirection.Vertical, Easings.Default(1f)),
                new AnimationListener()
                {
                    OnFinished = () =>
                    {
                        position.y = updatePositionY;
                        workersIcon.rectTransform.localPosition = position;
                    }
                }
            );
        }
    }
}