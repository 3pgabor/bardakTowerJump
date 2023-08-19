using UnityEngine;
using System.Collections;

public class musicBG : MonoBehaviour
{

    private static musicBG instance = null;
    public AudioClip[] musicbg;
    public static AudioSource source;
    public static bool isMuted;
    private int i;


    public static musicBG Instance
    {
        get { return instance; }
    }


    void Awake()
    {
        source = GetComponent<AudioSource>();
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        i = Random.Range(0, musicbg.Length);
        StartCoroutine("Playlist");
    }

    public static void StopMusic()
    {
       source.Pause();
    }

    public static void PlayMusic()
    {
        source.Play();
    }

    IEnumerator Playlist()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            if (!GetComponent<AudioSource>().isPlaying && !isMuted)
            {
                if (i != (musicbg.Length - 1))
                {
                    i++;
                    GetComponent<AudioSource>().clip = musicbg[i];
                    GetComponent<AudioSource>().Play();
                }
                else
                {
                    i = 0;
                    GetComponent<AudioSource>().clip = musicbg[i];
                    GetComponent<AudioSource>().Play();
                }
            }
        }
    }
}
