using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{
    public static AudioClip playerJump;
    public static AudioClip playerHit;
    public static AudioClip playerFall;
    public static AudioClip lit;
    public static AudioClip coin;
    public static bool isMuted;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {

        playerFall = Resources.Load<AudioClip>("Drop");
        playerJump = Resources.Load<AudioClip>("jump2");
        playerHit = Resources.Load<AudioClip>("hit2");
        lit = Resources.Load<AudioClip>("fire");
        coin = Resources.Load<AudioClip>("coin");

        audioSrc = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void PlaySound(string clip)
    {
        if(isMuted) return;

        switch (clip)
        {
            case "hit":
                audioSrc.PlayOneShot(clip: playerHit);
                break;
            case "jump":
                audioSrc.PlayOneShot(clip: playerJump);
                break;
            case "lit":
                audioSrc.PlayOneShot(clip: lit);
                break;
            case "coin":
                audioSrc.PlayOneShot(clip: coin);
                break;
            case "fall":
                audioSrc.PlayOneShot(clip: playerFall);
                break;
        }
    }
}