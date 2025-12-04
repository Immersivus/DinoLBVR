using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    [SerializeField] Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            Vector3 newPosition = new Vector3(player.transform.position.x, 0, player.transform.position.z);

            transform.position = newPosition;
        }
    }
}
