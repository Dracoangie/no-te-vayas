using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[DisallowMultipleComponent]
public class SoundEvent : MonoBehaviour
{
    // Estas funciones se pueden seleccionar desde cualquier UnityEvent.
    public void Play()
    {
        AudioSource source = GetComponent<AudioSource>();
        if (source.clip == null)
            return;

        if (source.loop)
        {
            if (!source.isPlaying)
                source.Play();
        }
        else
            source.PlayOneShot(source.clip);
    }

    public void Stop()
    {
        GetComponent<AudioSource>().Stop();
    }

    private void Reset()
    {
        GetComponent<AudioSource>().playOnAwake = false;
    }
}
