using System.Collections.Generic;
using UnityEngine;

public class BookTrack : MonoBehaviour
{
    [SerializeField] List<GameObject> books;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ActivateBook(int index)
    {
        if (!books[index].activeSelf)
        {
            foreach (var book in books)
            {
                book.SetActive(false);
            }

            books[index].SetActive(true);

            books[index].GetComponent<Animator>().SetTrigger("APPEAR");
        }
       
    }
}
