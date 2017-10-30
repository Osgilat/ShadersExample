using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BSC_ImageEffect : MonoBehaviour {

    #region Variables
    public Shader curShader;
    public float brightnessAmount = 1.0f;
    public float saturationAmount = 1.0f;
    public float contrastAmount = 1.0f;

    private Material curMaterial;
    #endregion

    #region Properties
    Material material
    {
        get
        {
            if (curMaterial == null)
            {
                curMaterial = new Material(curShader);
                curMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return curMaterial;
        }
    }
    #endregion

    private void Start()
    {
        if (!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            return;
        }

        if (!curShader && !curShader.isSupported)
        {
            enabled = false;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (curShader != null)
        {
            material.SetFloat("_BrightnessAmount", brightnessAmount);
            material.SetFloat("_satAmount", saturationAmount);
            material.SetFloat("_conAmount", contrastAmount);

            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    private void Update()
    {
        brightnessAmount = Mathf.Clamp(brightnessAmount, 0.0f, 2.0f);
        saturationAmount = Mathf.Clamp(saturationAmount, 0.0f, 2.0f);
        contrastAmount = Mathf.Clamp(contrastAmount, 0.0f, 3.0f);
    }

    private void OnDisable()
    {
        if (curMaterial)
        {
            DestroyImmediate(curMaterial);
        }
    }

}
