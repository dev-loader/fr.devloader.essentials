using Devloader.Effects;
using System.Collections.Generic;
using UnityEngine;

public class FadeMaterialProperties : AbstractEffect
{
    [System.Serializable]
    public struct HDRPColor
    {
        public Color color;
        public float intensity;

        public HDRPColor(Color color, float intensity)
        {
            this.color = color;
            this.intensity = intensity;
        }
    }

    [System.Serializable]
    public struct MaterialColorProperty
    {
        public string name;

        public HDRPColor initialColor;
        public HDRPColor finalColor;

        public MaterialColorProperty(string name, HDRPColor initialColor, HDRPColor finalColor)
        {
            this.name = name;

            this.initialColor = initialColor;
            this.finalColor = finalColor;
        }
    }

    [System.Serializable]
    public struct MaterialFloatProperty
    {
        public string name;

        public float initialValue;
        public float finalValue;

        public MaterialFloatProperty(string name, float initialValue, float finalValue)
        {
            this.name = name;

            this.initialValue = initialValue;
            this.finalValue = finalValue;
        }
    }

    [Header("Material settings")]
    [SerializeField] Material _material;

    [Header("Property settings")]
    [SerializeField] List<MaterialColorProperty> _colorProperties = new List<MaterialColorProperty>()
    {
        new MaterialColorProperty()
        {
            name = "_BaseColor",
            initialColor = new HDRPColor()
            {
                color = Color.white,
                intensity = 0f
            },
            finalColor = new HDRPColor()
            {
                color = Color.black,
                intensity = 0f
            }
        }
    };

    [SerializeField]
    List<MaterialFloatProperty> _floatProperties = new List<MaterialFloatProperty>();

    private void Awake() => processAction = value => {
        _colorProperties.ForEach(p =>
        {
            Color lerpedColor = Color.Lerp(p.initialColor.color, p.finalColor.color, value);
            float lerpedIntensity = Mathf.Lerp(p.initialColor.intensity + 1, p.finalColor.intensity + 1, value);

            Vector4 rgba = new Vector4(lerpedColor.r, lerpedColor.g, lerpedColor.b, lerpedColor.a);
            _material.SetVector(p.name, rgba * lerpedIntensity);
        });

        _floatProperties.ForEach(p => _material.SetFloat(p.name, Mathf.Lerp(p.initialValue, p.finalValue, value)));
    };
}
