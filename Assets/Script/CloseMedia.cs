using RenderHeads.Media.AVProVideo;
using System.Collections.Generic;
using UnityEngine;

public class CloseMedia : MonoBehaviour
{
    [SerializeField] List<MediaPlayer> players;

    [SerializeField] GameObject itself;
    public void CloseBookMedia()
    {
        foreach(var player in players)
        {
            player.CloseMedia();
        }
    }

    public void OpenBookMedia()
    {
        foreach (var player in players)
        {
            player.OpenMedia();
        }
    }


    public void DisableObject() 
    {
        itself.SetActive(false);
    }
}
