using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BlendMode_ImageEffect : MonoBehaviour {

    #region Variables
    public Shader curShader;
    public Texture2D blendTexture;
    public float blendOpacity = 1.0f;

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
            material.SetTexture("_BlendTex", blendTexture);
            material.SetFloat("_Opacity", blendOpacity);

            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    private void Update()
    {
        blendOpacity = Mathf.Clamp(blendOpacity, 0.0f, 1.0f);
    }

    private void OnDisable()
    {
        if (curMaterial)
        {
            DestroyImmediate(curMaterial);
        }
    }
}
