using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    private Transform icon;

    [SerializeField]
    private bool load;

    void Start()
    {
        //if (load)
        //{
        //    if (PersistentInfo.levelToLoad != null && Application.isLoadingLevel == false)
        //        Application.LoadLevelAsync(PersistentInfo.levelToLoad);
        //}
    }

    void Update()
    {
        //icon.Rotate(Vector3.forward * Time.deltaTime * 100, Space.Self);

        if (PersistentInfo.levelToLoad != null && Application.isLoadingLevel == false)
            Application.LoadLevelAsync(PersistentInfo.levelToLoad);
    }
}
