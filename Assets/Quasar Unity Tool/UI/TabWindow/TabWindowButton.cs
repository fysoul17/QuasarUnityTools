////////////////////////////////////////////////////////////////////////////
// TabWindowButton.cs
//
// Copyright (C) 2015 Terry K. @ GIX Entertainment Inc. & Quasar Inc.
//
////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.UI;
using System;

namespace Quasar.UI
{
    public class TabWindowButton : MonoBehaviour
    {
        public enum TabWindowButtonTransition
        {
            None = 0,
            ColorTint,
            SpriteSwap
        }

        public Action<TabWindowButton> ButtonTapEvent;

        public Image background;
        public Text title;
        public TabWindowButtonTransition transition = TabWindowButtonTransition.None;

        public Color normalBackgroundColor;
        public Color selectedBackgroundColor;
        public Color normalTextColor;
        public Color selectedTextColor;

        public Sprite normalSprite;
        public Sprite selectedSprite;

        public bool interactable = true;

        void SetButtonAppearance(bool isSelected)
        {
            if (transition == TabWindowButtonTransition.None) return;

            if (transition == TabWindowButtonTransition.SpriteSwap)
            {
                background.sprite = isSelected ? selectedSprite : normalSprite;
            }

            background.color = isSelected ? selectedBackgroundColor : normalBackgroundColor;

            if (title) title.color = isSelected ? selectedTextColor : normalTextColor;
        }

        public bool IsSelected
        {
            get;
            protected set;
        }

        public void Pressed()
        {
            if (!interactable) return;

            ButtonTapEvent(this);
        }

        public void SetSelected(bool setSelected)
        {
            IsSelected = setSelected;

            SetButtonAppearance(isSelected: IsSelected);
        }
    }

}