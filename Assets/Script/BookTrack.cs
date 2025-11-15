using System.Collections.Generic;
using UnityEngine;

public class BookTrack : MonoBehaviour
{
    [SerializeField] List<GameObject> bookVideos;

    [SerializeField] GameObject bookObject;


    Transform playerTransform;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        else
        {
            Vector3 newPosition = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
            transform.position = newPosition;
            transform.rotation = playerTransform.rotation;
        }
    }


    public void ActivateBook(int index)
    {
        if (!bookVideos[index].activeSelf)
        {
            foreach (var book in bookVideos)
            {
                book.SetActive(false);
            }

            bookVideos[index].SetActive(true);
          
        }
        bookObject.GetComponent<Animator>().SetTrigger("APPEAR");
    }
}
