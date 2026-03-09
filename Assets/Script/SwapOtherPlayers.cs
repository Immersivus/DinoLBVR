using UnityEngine;

public class SwapOtherPlayers : MonoBehaviour
{
    int handIndex;

    [SerializeField] GameObject jungleAvatar;
    [SerializeField] GameObject waterAvatar;

    [SerializeField] SkinnedMeshRenderer leftHandMesh;
    [SerializeField] SkinnedMeshRenderer rightHandMesh;

    [SerializeField] Material handJungleMaterial;
    [SerializeField] Material handWaterMaterial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeHandMaterials()
    {
        handIndex = handIndex == 0 ? 1 : 0;
        switch (handIndex)
        {
            case 0:
                jungleAvatar.SetActive(true);
                waterAvatar.SetActive(false);

                Material[] leftHandMats = leftHandMesh.materials;
                leftHandMats[0] = handJungleMaterial;
                leftHandMesh.materials = leftHandMats;
                Material[] rightHandMats = rightHandMesh.materials;
                rightHandMats[0] = handJungleMaterial;
                rightHandMesh.materials = rightHandMats;
                break;
            case 1:
                jungleAvatar.SetActive(false);
                waterAvatar.SetActive(true);

                Material[] leftHandMats2 = leftHandMesh.materials;
                leftHandMats2[0] = handWaterMaterial;
                leftHandMesh.materials = leftHandMats2;
                Material[] rightHandMats2 = rightHandMesh.materials;
                rightHandMats2[0] = handWaterMaterial;
                rightHandMesh.materials = rightHandMats2;
                break;
        }

       
    }
}
