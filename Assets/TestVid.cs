using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TestVid : MonoBehaviour
{
    public GameObject VideoPlay;
    // Start is called before the first frame update

    void Start()
    {
        print("PlayStart");
        var videoPlayer = VideoPlay.AddComponent<VideoPlayer>();
        videoPlayer.Prepare();
        videoPlayer.Play();
        print("VidPlayEnabled");

        print(videoPlayer.isPlaying);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
