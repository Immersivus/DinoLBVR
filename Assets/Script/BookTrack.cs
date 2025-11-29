using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookTrack : MonoBehaviour
{
    [SerializeField] List<GameObject> bookVideos;

    [SerializeField] GameObject bookObject;

    bool tracking;
    Transform playerTransform;

    private float _fadeDuration = 100.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }
        else
        {
            if (!tracking)
            {
                StartCoroutine(MoveToPlayer());
                tracking = true;
            }

            Vector3 newPosition = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
            transform.position = newPosition;
        }
    }

    IEnumerator MoveToPlayer()
    {
        while (true)
        {
            Quaternion stabilizedRotation = new Quaternion(transform.rotation.x, playerTransform.rotation.y, playerTransform.rotation.z, playerTransform.rotation.w);
            transform.rotation = Quaternion.Lerp(transform.rotation, stabilizedRotation, Time.deltaTime);
            yield return null;
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
