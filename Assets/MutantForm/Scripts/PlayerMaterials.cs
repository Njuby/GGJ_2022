using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerMaterials", fileName = "PlayerMaterials")]
public class PlayerMaterials : ScriptableObject
{
    public Material hairMat;
    public Material bodyMat;
    public Material headMat;
    public Material eyesMat;
    public Material tearMat;
    public Material pistolMat;

    public void SetMaterials(SkinnedMeshRenderer body, MeshRenderer pistol)
    {
        Material[] materials = new Material[6];

        materials[0] = hairMat;
        materials[1] = bodyMat;
        materials[2] = headMat;
        materials[3] = eyesMat;
        materials[4] = tearMat;
        materials[5] = hairMat;

        body.materials = materials;

        pistol.material = pistolMat;
    }
}