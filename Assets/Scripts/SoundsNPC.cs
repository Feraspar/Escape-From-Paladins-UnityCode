using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsNPC : MonoBehaviour
{
    private AudioSource m_AudioSource;
    public AudioClip[] enemySounds;

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }
    public void PlayFootSteps()
    {
        FindObjectOfType<AudioManager>().Play("Player_Footsteps");
    }

    public void AttackSwing()
    {
        FindObjectOfType<AudioManager>().Play("Enemy_Attack");
    }

    public void EnemyWalkPlayFootSteps()
    {
        AudioClip clipToPlay = FindSoundByName("S_Rubber_Mono_14");
        m_AudioSource.clip = clipToPlay;
        m_AudioSource.Play();
    }

    public void EnemyRunPlayFootSteps()
    {
        AudioClip clipToPlay = FindSoundByName("S_Rubber_Mono_14");
        m_AudioSource.clip = clipToPlay;
        m_AudioSource.Play();
    }

    private AudioClip FindSoundByName(string soundName)
    {
        foreach (AudioClip clip in enemySounds)
        {
            if (clip.name == soundName)
            {
                return clip;
            }
        }

        return null;
    }
}
