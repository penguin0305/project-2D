using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private AudioSource audioSource;

	[Header("Clips")]
	[SerializeField] private AudioClip jumpClip;
	[SerializeField] private AudioClip landClip;
	[SerializeField] private AudioClip meleeClip;
	[SerializeField] private AudioClip footstepClip;

	private void PlayOnce(AudioClip clip)
	{
		if (clip == null || audioSource == null)
			return;
		audioSource.PlayOneShot(clip);
	}

	public void PlayJump()
	{
		PlayOnce(jumpClip);
	}

	public void PlayLand()
	{
		PlayOnce(landClip);
	}

	public void PlayMelee()
	{
		PlayOnce(meleeClip);
	}

	public void PlayFootstep()
	{
		PlayOnce(footstepClip);
	}
}
