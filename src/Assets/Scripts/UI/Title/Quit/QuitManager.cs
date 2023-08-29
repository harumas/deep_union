﻿using System;
using Core.Utility.UI;
using Core.Utility.UI.Cursor;
using UnityEngine;

namespace UI.Title.Quit
{
    internal class QuitManager : MonoBehaviour, IUIManager
    {
        public Action<bool> onClick;
        
        public enum Nav
        {
            Yes,
            No
        }

        [SerializeField] private CursorController<Nav> cursor;
        [SerializeField] private RectTransform yes;
        [SerializeField] private RectTransform no;

        private Nav? current;

        private void Start()
        {
            cursor.AddPoint(Nav.Yes, yes);
            cursor.AddPoint(Nav.No, no);
            current = null;
        }


        private void SetState(Nav setNav)
        {
            current = setNav;
            cursor.SetPoint(setNav);
        }

        public void Initialized()
        {     
            gameObject.SetActive(true);
            SetState(Nav.Yes);
        }

        public void Clicked()
        {
            if (!current.HasValue) return;
            switch (current.Value)
            {
                case Nav.Yes:
                    onClick?.Invoke(true);
                    break;
                case Nav.No:
                    onClick?.Invoke(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Select(Vector2 direction)
        {
            if (!current.HasValue)
            {
                SetState(Nav.Yes);
                return;
            }

            if (direction.y != 0)
            {
                var nextNav = current == Nav.No ? Nav.Yes : Nav.No;
                SetState(nextNav);
            }
        }

        public void Finished()
        {
            gameObject.SetActive(false);
        }
    }
}