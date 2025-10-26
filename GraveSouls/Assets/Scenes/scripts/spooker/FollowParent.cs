using UnityEngine;

public class FollowParent : MonoBehaviour
{
    [Header("Parent Reference")]
    public Transform parentTransform;

    [Header("Children to Follow")]
    public Transform[] children;

    void Update()
    {
        if (parentTransform == null || children == null) return;

        foreach (Transform child in children)
        {
            if (child == null) continue;

            // Match parent position and rotation
            child.position = parentTransform.position;
            child.rotation = parentTransform.rotation;
        }
    }
}
