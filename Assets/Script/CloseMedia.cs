using RenderHeads.Media.AVProVideo;
using System.Collections.Generic;
using UnityEngine;

public class CloseMedia : MonoBehaviour
{
    [SerializeField] List<MediaPlayer> players;
    public void CloseBookMedia()
    {
        foreach(var player in players)
        {
            player.CloseMedia();
        }
    }
}
