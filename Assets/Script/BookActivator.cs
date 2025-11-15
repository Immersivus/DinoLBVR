using UnityEngine;

public class BookActivator : MonoBehaviour
{

    [SerializeField] int indexToActivate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject bookHolder = GameObject.FindGameObjectWithTag("BookHolder");
            bookHolder.GetComponent<BookTrack>().ActivateBook(indexToActivate);
        }
    }
}
