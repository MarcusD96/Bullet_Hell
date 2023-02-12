
using UnityEngine;
using UnityEditor;

public class AddAudioTool : EditorWindow {

    string soundName;
    AudioClip clipToUse;
    float volume, pitch, randVolume, randPitch;
    bool loop;
    AudioType audioType;

    [MenuItem("Tools/Add Audio")]
    public static void ShowWindow() {
        GetWindow(typeof(AddAudioTool));
    }

    private void OnGUI() {
        GUILayout.Space(10);
        GUILayout.Label("Add new sound to Soud Manager", EditorStyles.boldLabel);
        GUILayout.Space(10);

        audioType = (AudioType) EditorGUILayout.EnumPopup("Audio Array", audioType);
        soundName = EditorGUILayout.TextField("Name", soundName);
        clipToUse = (AudioClip) EditorGUILayout.ObjectField("Audio Clip", clipToUse, typeof(AudioClip), true);
        volume = EditorGUILayout.Slider("Volume", volume, 0f, 1f);
        pitch = EditorGUILayout.Slider("Pitch", pitch, 0f, 1f);
        randVolume = EditorGUILayout.Slider("Random Volume", randVolume, 0f, 1f);
        randPitch = EditorGUILayout.Slider("Random Pitch", randPitch, 0f, 1f);
        loop = EditorGUILayout.Toggle("Looped", loop);

        if(GUILayout.Button("Add New Sound")) {
            AddAudioToManager();
            Close();
        }
    }

    void AddAudioToManager() {
        //get sound manager asset to modify
        AudioManager manager = (AudioManager) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/UI/Audio Manager.prefab", typeof(AudioManager));

        Undo.RecordObject(manager, "Created New Audio");
        PrefabUtility.RecordPrefabInstancePropertyModifications(manager);


        //make a new array with an additional spot for thew new sound and copy the old array into the new one
        Sound[] newSounds, oldSounds;
        switch(audioType) {
            case AudioType.SFX:
                newSounds = new Sound[manager.SFX.Length + 1];
                oldSounds = manager.SFX;
                break;
            case AudioType.MUSIC:
                newSounds = new Sound[manager.Music.Length + 1];
                oldSounds = manager.Music;
                break;
            default:
                Debug.LogError("Invalid AudioType");
                return;
        }
        for(int i = 0; i < oldSounds.Length - 1; i++) {
            newSounds[i] = oldSounds[i];
        }

        var n = newSounds[newSounds.Length - 1] = new Sound();
        //add the new sound to the new spot
        n.name = soundName;
        n.clip = clipToUse;
        n.volume = volume;
        n.pitch = pitch;
        n.randomVolume = randVolume;
        n.randomPitch = randPitch;
        n.loop = loop;

        //replace the old sounds array with the new one
        switch(audioType) {
            case AudioType.SFX:
                manager.SFX = newSounds;
                break;
            case AudioType.MUSIC:
                manager.Music = newSounds;
                break;
            default:
                break;
        }
    }
}

public enum AudioType {
    SFX,
    MUSIC
}