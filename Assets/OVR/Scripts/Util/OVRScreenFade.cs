/************************************************************************************

Copyright   :   Copyright 2014 Oculus VR, LLC. All Rights reserved.

Licensed under the Oculus VR Rift SDK License Version 3.3 (the "License");
you may not use the Oculus VR Rift SDK except in compliance with the License,
which is provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

You may obtain a copy of the License at

http://www.oculus.com/licenses/LICENSE-3.3

Unless required by applicable law or agreed to in writing, the Oculus VR SDK
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

************************************************************************************/

using UnityEngine;
using System.Collections; // required for Coroutines
using UnityEngine.UI;

/// <summary>
/// Fades the screen from black after a new scene is loaded.
/// </summary>
public class OVRScreenFade : MonoBehaviour
{
	/// <summary>
	/// How long it takes to fade.
	/// </summary>
	public float fadeTime = 2.0f;
    [SerializeField]
    private float _waitLogoTime = 4.0f;

	/// <summary>
	/// The initial screen color.
	/// </summary>
	public Color fadeColor = new Color(0.01f, 0.01f, 0.01f, 1.0f);

	private Material fadeMaterial = null;
	private bool isFading = false;
	private YieldInstruction fadeInstruction = new WaitForEndOfFrame();
    private YieldInstruction waitLogoInstruction;

    [SerializeField]
    private Texture _logoTexture;

    private bool _displayedLogo = false;

	/// <summary>
	/// Initialize.
	/// </summary>
	void Awake()
	{
		// create the fade material
		fadeMaterial = new Material(Shader.Find("Oculus/Unlit Transparent Color"));
        if (_logoTexture == null)
        {
            Debug.LogError("ATTACH LOGO!");
        }
        //_logoMat = new Material(Shader.Find("Transparent/Diffuse"));
        
        waitLogoInstruction = new WaitForSeconds(_waitLogoTime);
    }

	/// <summary>
	/// Starts the fade in
	/// </summary>
	void OnEnable()
	{
		StartCoroutine(FadeIn());
	}

	/// <summary>
	/// Starts a fade in when a new level is loaded
	/// </summary>
	void OnLevelWasLoaded(int level)
	{
		StartCoroutine(FadeIn());
	}

	/// <summary>
	/// Cleans up the fade material
	/// </summary>
	void OnDestroy()
	{
		if (fadeMaterial != null)
		{
			Destroy(fadeMaterial);
		}
    }

	/// <summary>
	/// Fades alpha from 1.0 to 0.0
	/// </summary>
	IEnumerator FadeIn()
	{
        
        yield return waitLogoInstruction;
        _displayedLogo = true;
        isFading = true;
        fadeMaterial.color = fadeColor;
        Color color = fadeColor;
        isFading = true;
        
        //yield return waitLogoInstruction;
        
        float elapsedTime = 0.0f;
        while (elapsedTime < fadeTime)
        {
            //Graphics.DrawTexture(new Rect(0, 0, 3000, 3000), _logoTexture, 0, 2, 0, 2, null);
            //_logoImage.gameObject.SetActive(true);
            //_logoImage.enabled = true;
            yield return fadeInstruction;
        	elapsedTime += Time.deltaTime;
        	color.a = 1.0f - Mathf.Clamp01(elapsedTime / fadeTime);
        	fadeMaterial.color = color;
        }
        Debug.Log("Fading done");
        isFading = false;
    }

	/// <summary>
	/// Renders the fade overlay when attached to a camera object
	/// </summary>
	void OnPostRender()
	{
        if(!_displayedLogo)
        {
            GL.PushMatrix();
            GL.LoadOrtho();
            Graphics.DrawTexture(new Rect(0, 0, 1, 1), _logoTexture);
            GL.End();
            GL.PopMatrix();
        }
		else if (isFading)
		{
            fadeMaterial.SetPass(0);
            GL.Color(fadeMaterial.color);
            GL.PushMatrix();
			GL.LoadOrtho();

            GL.Begin(GL.QUADS);
            GL.Vertex3(0f, 0f, -12f);
            GL.Vertex3(0f, 1f, -12f);
            GL.Vertex3(1f, 1f, -12f);
            GL.Vertex3(1f, 0f, -12f);
            GL.End();

            GL.PopMatrix();
		}
    }
}
