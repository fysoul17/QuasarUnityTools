////////////////////////////////////////////////////////////////////////////
// LoadingIndicator.cs
//
// DEPENDENCY: iTween - Free (http://www.pixelplacement.com/itween/index.php)
//
// Author: Terry Keetae Kwon
// 
// Copyright (C) 2015 Terry K. @ GIX Entertainment Inc. & Quasar Inc.
//
////////////////////////////////////////////////////////////////////////////

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Quasar.UI
{
    public class LoadingIndicator : MonoBehaviour
    {
        public bool isPlaying;
        public LoadingIndicatorMode animationMode;

        [SerializeField] Text text;

        [SerializeField] Image[] indicatorImageObjects;

        /////// Rotation related values.
        [HideInInspector] public bool rotateClockwise = true;       // Will be re-drawn in editor script.
        Hashtable rotateHash;
        //////////////////////////

        /////// Swap related values.
        [HideInInspector] public float swapDelay = 1f;              // Will be re-drawn in editor script.
        WaitForSeconds waitForSeconds;
        //////////////////////////

        void Awake()
        {
            SetRotateHashtable();
        }

        public void Display(string displayText = "")
        {
            if (isPlaying) return;
            isPlaying = true;

            text.text = displayText;

            gameObject.SetActive(true);

            StartAnimation();
        }

        public void Hide()
        {
            gameObject.SetActive(false);

            isPlaying = false;
        }

        void StartAnimation()
        {
            if (animationMode == LoadingIndicatorMode.Rotation)
            {
                // Must use only one image for rotating. Disable all other images if exist.
                int numberOfImages = indicatorImageObjects.Length;
                for (int index = 0; index < numberOfImages; index++)
                {
                    indicatorImageObjects[index].gameObject.SetActive(index == 0);
                }

                iTween.RotateBy(indicatorImageObjects[0].gameObject, rotateHash);
            }
            else if (animationMode == LoadingIndicatorMode.ImageSwap)
            {
                StopCoroutine("SwapImages");    // Prevent starting multiple corutine for 'swap images'.
                StartCoroutine("SwapImages");
            }
        }

        #region Rotation Mode settings
        void SetRotateHashtable()
        {
            rotateHash = rotateHash ?? new Hashtable();
            rotateHash.Clear();

            rotateHash.Add("amount", new Vector3(0f, 0f, rotateClockwise ? -1f : 1f));
            rotateHash.Add("easetype", iTween.EaseType.linear);
            rotateHash.Add("time", 3f);
            rotateHash.Add("looptype", iTween.LoopType.loop);
        }
        #endregion

        #region Swap Mode settings
        IEnumerator SwapImages()
        {
            int numberOfImages = indicatorImageObjects.Length;
            if (numberOfImages < 2)
            {
                Debug.LogError("At least 2 images should be presented in order to swap.");
                yield break;
            }

            waitForSeconds = new WaitForSeconds(swapDelay);
            int currentDisplayingIndex = 0;
            while (true)
            {
                for (int index = 0; index < numberOfImages; index++)
                {
                    indicatorImageObjects[index].gameObject.SetActive(index == currentDisplayingIndex);
                }

                yield return waitForSeconds;

                if (++currentDisplayingIndex >= numberOfImages) currentDisplayingIndex = 0;
                // NOTE: Can use below code if you want. Personally prefer not to use % or / if I can. 
                // currentDisplayingIndex = (currentDisplayingIndex + 1) % numberOfImages.
            }
        }
        #endregion
    }

    public enum LoadingIndicatorMode
    {
        Rotation,
        ImageSwap
        // AnimationClip   // Not supported yet.
    }
}