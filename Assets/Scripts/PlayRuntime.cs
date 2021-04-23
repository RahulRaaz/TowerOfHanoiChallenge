using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayRuntime : MonoBehaviour
{
    public GameObject VideoPlay;
    // Start is called before the first frame update
    private void Start()
    {
        var videoPlayer = VideoPlay.AddComponent<VideoPlayer>();
        print("start vid");
        videoPlayer.playOnAwake = false;
        videoPlayer.Play();
    }
}
