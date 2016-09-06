using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour 

{
    // CLIPS
    [SerializeField]
    private AudioClip Tap;
    [SerializeField]
    private float _tapSoundPercentage = 1;
    [SerializeField]
    private AudioClip ClickMove;
    [SerializeField]
    private float _clickSoundPercentage = 1;
    [SerializeField]
    private AudioClip Twirl;
    [SerializeField]
    private float _twirlSoundPercentage = 1;


    // AUDIOSOURCE
    private AudioSource audioSource;

	void Start () 
	{
		audioSource = GetComponent<AudioSource> ();
	}
	
	public void TapAudio()
	{
		audioSource.clip = Tap;
        audioSource.volume = _tapSoundPercentage;
		audioSource.Play ();
	}

	public void ClickMoveAudio()
	{
		audioSource.clip = ClickMove;
        audioSource.volume = _clickSoundPercentage;
		audioSource.Play ();
	}

	public void TwirlAudio()
	{
		audioSource.clip = Twirl;
        audioSource.volume = _twirlSoundPercentage;
		audioSource.Play ();
	}

	// TEST PURPOSES

	//void Update()
	//{
	//	if (Input.GetKeyDown(KeyCode.A))
	//	{
	//		TapAudio ();
	//	}
    //
	//	if (Input.GetKeyDown(KeyCode.S))
	//	{
	//		ClickMoveAudio ();
	//	}
    //
	//	if (Input.GetKeyDown(KeyCode.D))
	//	{
	//		TwirlAudio ();
	//	}
	//}


}
