using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestRenderImage : MonoBehaviour
{
    #region Variables
    public Shader curShader;
  //  public float grayScaleAmount = 1.0f;
    public float depthPower = 1.0f;

    private Material curMaterial;
    #endregion

    #region Properties
    Material material
    {
        get
        {
            if(curMaterial == null)
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
        if(!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            return;
        }

        if(!curShader && !curShader.isSupported)
        {
            enabled = false;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(curShader != null)
        {
          //  material.SetFloat("_LuminosityAmount", grayScaleAmount);
            material.SetFloat("_DepthPower", depthPower);

            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    private void Update()
    {
        //grayScaleAmount = Mathf.Clamp(grayScaleAmount, 0.0f, 1.0f);
        Camera.main.depthTextureMode = DepthTextureMode.Depth;
        depthPower = Mathf.Clamp(depthPower, 0, 5);
    }

    private void OnDisable()
    {
        if(curMaterial)
        {
            DestroyImmediate(curMaterial);
        }
    }

}
