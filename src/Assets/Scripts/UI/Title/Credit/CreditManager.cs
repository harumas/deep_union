﻿using System;
using AnimationPro.RunTime;
using Core.Utility.UI.Navigation;
using UnityEngine;

namespace UI.Title.Credit
{
    internal class CreditManager : UIManager
    {
        public void Initialized(ContentTransform content)
        {
            gameObject.SetActive(true);
            OnCancel();
            Animation(content);
        }

        public void Clicked()
        {
        }

        public void Select(Vector2 direction)
        {
        }

        public void Finished(ContentTransform content, Action onFinished)
        {
            Animation(
                content,
                new AnimationListener
                {
                    OnFinished = () =>
                    {
                        gameObject.SetActive(false);
                        onFinished?.Invoke();
                    }
                }
            );
        }

        public AnimationBehaviour GetContext()
        {
            return this;
        }
    }
}