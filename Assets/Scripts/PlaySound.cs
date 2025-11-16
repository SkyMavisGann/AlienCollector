using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaySound : MonoBehaviour
{
    public float pitchMax = 1f;
    public float pitchMin = 1f;
    public float volume = 1f;

    public float imageSpeed = 1.0f;
    public float imageScale = 2;
    public int OrderLayer = 2;
    public float velocity = 2;
    public Color32 tintColor = new Color32(56, 56, 56, 255);
    public enum SoundType
    {
        AMBIENT,
        EFFECTS,
        CUSTOM
    }

    public SoundType type = SoundType.EFFECTS;
    public void playAudioClip(AudioClip clip)
    {
        GameObject obj = new GameObject("AudioSource");
        AudioSource newSource = obj.AddComponent<AudioSource>();
        switch (type)
        {
            case SoundType.AMBIENT:
                newSource.volume = PlayerPrefs.GetFloat("ambientVolume") * volume;
                break;
            case SoundType.EFFECTS:
                newSource.volume = PlayerPrefs.GetFloat("effectsVolume") * volume;
                break;
            default:
                newSource.volume = volume;
                break;
        }
        
        newSource.playOnAwake = false;
        obj.AddComponent<DestroyOnAudioSourceEnd>();
        obj.AddComponent<dontDestroyOnLoad>();
        if (pitchMax < pitchMin) pitchMin = pitchMax;
        newSource.pitch = Random.Range(pitchMin, pitchMax);
        newSource.clip = clip;
        newSource.Play();
    }

    public void createSoundImage(string word)
    {
        GameObject textTemplate = Instantiate(Resources.Load<GameObject>("SoundAnimation"), transform.position, transform.rotation, transform.parent);
        
        //foreach(Image img in new List<Image>(textTemplate.GetComponentsInChildren<Image>()))
        //{
        //    img.color = tintColor;
        //}
        //textTemplate.GetComponentInChildren<TMP_Text>().color = tintColor;

        Transform objParent = textTemplate.transform.parent;
        if (objParent != null)
        {
            for (int i = 0; i < OrderLayer; i++)
            {
                if (objParent.parent != null && objParent.GetComponent<Canvas>() == null)
                {
                    textTemplate.transform.SetParent(objParent.parent);
                    objParent = textTemplate.transform.parent;
                }
            }
        }
        
        textTemplate.transform.SetAsLastSibling();
        textTemplate.transform.localScale = Vector3.one * imageScale;
        textTemplate.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-velocity, velocity), Random.Range(-velocity, velocity));
        textTemplate.GetComponentInChildren<TMP_Text>().text = word;
        textTemplate.GetComponent<Animator>().speed = imageSpeed;
    }

    public void PlaySoundAndPlace(AudioClip clip)
    {
        playAudioClip(clip);
        createSoundImage(clip.name);
    }
}
