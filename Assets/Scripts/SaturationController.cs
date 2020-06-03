using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace EyalPhoton.Game
{
    public class SaturationController : MonoBehaviour
    {
        [SerializeField] private float startingSaturation = -100f;
        [SerializeField] private float currSaturation = -100f;
        [SerializeField] private float maxSaturation = 0f;
        [SerializeField] private float saturationAddition = 25f;

        private ColorGrading cg = null;

        void Start()
        {
            currSaturation = startingSaturation;
            cg = GetComponent<PostProcessVolume>().profile.GetSetting<ColorGrading>();
            cg.saturation.value = currSaturation;
        }

        void Update()
        {
            cg.saturation.value = Mathf.Lerp(cg.saturation.value, currSaturation, 0.05f);
        }

        public void AddColor ()
        {
            if (currSaturation < maxSaturation)
            {
                currSaturation += saturationAddition;
            }
        }
    }
}