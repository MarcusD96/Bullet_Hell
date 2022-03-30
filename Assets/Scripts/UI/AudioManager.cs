using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    #region Singleton
    public static AudioManager Instance;

    private void Awake() {
        if(Instance) {
			Destroy(this);
			return;
        }
		Instance = this;
    }
    #endregion

	public Sound[] sounds;

	void Start() {
		DontDestroyOnLoad(gameObject);
		GameObject s = new GameObject("Sounds");
		s.transform.SetParent(transform);
		for(int i = 0; i < sounds.Length; i++) {
			GameObject _go = new GameObject(sounds[i].name);
			_go.transform.SetParent(s.transform);
			sounds[i].SetSource(_go.AddComponent<AudioSource>());
		}
	}

	public void PlaySound(string name_) {
		for(int i = 0; i < sounds.Length; i++) {
			if(sounds[i].name == name_) {
                if(sounds[i].loop)
					Debug.LogWarning(name_ + " sound is looped, use PlayLoopedSound() instead.");
				sounds[i].Play();
				return;
			}
		}

		// no sound with _name
		Debug.LogWarning("AudioManager: Sound not found in list, " + name_);
	}

	public int PlayLoopedSound(string name_) {
		for(int i = 0; i < sounds.Length; i++) {
			if(sounds[i].name == name_) {
				sounds[i].PlayLooped();
				return i;
			}
		}

		// no sound with _name
		Debug.LogWarning("AudioManager: Sound not found in list, " + name_);
		return -1;
    }

	public void StopSound(int index) {
		sounds[index].Stop();
    }
}

[System.Serializable]
public class Sound {

	public string name;
	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume = 0.5f;
	[Range(0.5f, 1.5f)]
	public float pitch = 1f;

	[Range(0f, 0.5f)]
	public float randomVolume = 0;
	[Range(0f, 0.5f)]
	public float randomPitch = 0;

	public bool loop = false;

	private AudioSource source;

	public void SetSource(AudioSource _source) {
		source = _source;
		source.volume = volume;
		source.pitch = pitch;
		source.loop = loop;
		source.clip = clip;
	}

	public void Play() {
		source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
		source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
		source.PlayOneShot(source.clip);
	}
	
	public void PlayLooped() {
		source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
		source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
		source.Play();
	}

	public void Stop() {
		source.Stop();
    }

}
