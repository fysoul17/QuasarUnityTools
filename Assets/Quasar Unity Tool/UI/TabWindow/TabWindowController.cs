////////////////////////////////////////////////////////////////////////////
// TabWindowController - This controls tab windows.
//
// Copyright (C) 2015-2016 Terry K. @ GIX Entertainment Inc. & Quasar Inc.
//
////////////////////////////////////////////////////////////////////////////

using UnityEngine;

namespace Quasar.UI
{
    public class TabWindowController : MonoBehaviour
    {
        public GameObject[] tabWindows;
        public TabWindowButton[] tabButtons;

        public int defaultActivationIndex = 0;

        public TabActivateMode activateMode = TabActivateMode.SetActive;

        [SerializeField] Vector3 windowActivePosition = Vector3.zero;
        Vector3 offPosition = new Vector3(5000f, 5000f);

        ITabWindowDelegate tabWindowDelegate;

        public enum TabActivateMode
        {
            SetActive,
            Position
        }

        public GameObject SelectedView
        {
            get;
            protected set;
        }

        public int SelectedIndex
        {
            get;
            protected set;
        }

        void Awake()
        {
            Validate();

            InitialiseValues();

            InitialiseTabButtonsEvents();
        }

		void Start() 
		{
			ActivateTabAtIndex(defaultActivationIndex);
		}

        void Validate()
        {
            if (tabWindows == null || tabWindows.Length < 1)
            {
                Debug.LogError("Tab window is initialised, but no views are allocated.");
                return;
            }

            if (tabButtons == null || tabButtons.Length < 1)
            {
                Debug.LogError("Tab window is initialised, but no buttons are allocated.");
                return;
            }

            if (tabWindows.Length != tabButtons.Length)
            {
                Debug.LogError(string.Format("{0} of tab windows are allocated, but {1} of tab buttons are exist. (Something is missing)", tabWindows.Length, tabButtons.Length));
                return;
            }

            int numberOfWindows = tabWindows.Length;
            for (int index = 0; index < numberOfWindows; index++)
            {
                if (tabWindows[index] == null) Debug.LogError(string.Format("index of {0}'s tab window is not allocated.", index));
                if (tabButtons[index] == null) Debug.LogError(string.Format("index of {0}'s tab button is not allocated.", index));
            }
        }

        void InitialiseValues()
        {
			windowActivePosition = tabWindows[0].transform.localPosition;

            SelectedIndex = -1;
            SelectedView = null;
        }

        public void SetTabWindowDelegate(ITabWindowDelegate aDelegate)
        {
            tabWindowDelegate = aDelegate;
        }

        void InitialiseTabButtonsEvents()
        {
            int numberOfButtons = tabButtons.Length;
            for (int index = 0; index < numberOfButtons; index++)
            {
                if (tabButtons[index] == null) return;

                tabButtons[index].ButtonTapEvent = ButtonTapped;
            }
        }

        void ButtonTapped(TabWindowButton button)
        {
            int numberOfTabbarButtons = tabButtons.Length;
            for (int index = 0; index < numberOfTabbarButtons; index++)
            {
                if (tabButtons[index] == button)
                {
                    ActivateTabAtIndex(index);
                    break;
                }
            }
        }

        public void ActivateTabAtIndex(int index)
        {
            if (tabWindowDelegate != null)
            {
                tabWindowDelegate.DidSelectTabAtIndex(tabWindows[index], index);
            }

            if (SelectedIndex == index) return;

            SelectedView = tabWindows[index];
            SelectedIndex = index;

            bool isTarget = false;
            int numberOfTabs = tabWindows.Length;
            for (int tabIndex = 0; tabIndex < numberOfTabs; tabIndex++)
            {
                isTarget = index == tabIndex;

                tabButtons[tabIndex].SetSelected(isTarget);

                // SetActive mode.
                if (activateMode == TabActivateMode.SetActive)
                {
                    tabWindows[tabIndex].SetActive(isTarget);
                }
                // Position mode.
                else
                {
                    tabWindows[tabIndex].transform.localPosition = isTarget ? windowActivePosition : offPosition;
                }
            }

            if (tabWindowDelegate != null)
            {
                tabWindowDelegate.DidActivateViewAtIndex(tabWindows[index], index);
            }
        }
    }
}
