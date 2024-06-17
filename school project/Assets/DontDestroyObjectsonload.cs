using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Debug.Log(gameObject.name + " is set to not be destroyed on load.");
    }
}
