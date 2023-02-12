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

	public Sound[] SFX;
	public Sound[] Music;

	Sound currentSong;

	void Start() {
		DontDestroyOnLoad(gameObject);
		InitializeSounds(SFX);
		InitializeSounds(Music);
		StartCoroutine(PlayMusic()); 
	}

	void InitializeSounds(Sound[] soundArray) {
		GameObject s = new GameObject(nameof(soundArray));
		s.transform.SetParent(transform);
		for(int i = 0; i < soundArray.Length; i++) {
			GameObject _go = new GameObject(soundArray[i].name);
			_go.transform.SetParent(s.transform);
			soundArray[i].SetSource(_go.AddComponent<AudioSource>());
		}
	}

	public void PlaySound(string name_) {
		for(int i = 0; i < SFX.Length; i++) {
			if(SFX[i].name == name_) {
                if(SFX[i].loop)
					Debug.LogWarning(name_ + " sound is looped, use PlayLoopedSound() instead.");
				SFX[i].Play();
				return;
			}
		}

		// no sound with _name
		Debug.LogWarning("AudioManager: Sound not found in list, " + name_);
	}

	public int PlayLoopedSound(string name_) {
		for(int i = 0; i < SFX.Length; i++) {
			if(SFX[i].name == name_) {
				SFX[i].PlayLooped();
				return i;
			}
		}

		// no sound with _name
		Debug.LogWarning("AudioManager: Sound not found in list, " + name_);
		return -1;
    }

	public void StopSound(int index) {
		SFX[index].Stop();
    }

	IEnumerator PlayMusic() {
		int musicIndex = Random.Range(0, Music.Length);

        while(true) {
			currentSong = Music[musicIndex];
			Music[musicIndex].Play();
			yield return new WaitForSecondsRealtime(Music[musicIndex].clip.length + 3f);
			musicIndex++;
			if(musicIndex >= Music.Length)
				musicIndex = 0;
        }
    }

	public void SetSFXVolume(float num) {
        foreach(var s in SFX) {
			s.volume = num;
        }
    }
	
	public void SetMusicVolume(float num) {
        foreach(var s in Music) {
			s.volume = num;
        }
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
