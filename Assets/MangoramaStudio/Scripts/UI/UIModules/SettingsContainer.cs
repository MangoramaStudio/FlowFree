using UnityEngine;
using System.Collections;

public class SettingsContainer : MonoBehaviour
{
    [SerializeField] private Transform _activeContainer;
    [SerializeField] private Transform _inActiveContainer;

    public void Initialize(bool isContainerEnabled)
    {
        DeactivateContainers();

        if (isContainerEnabled)
        {
            _activeContainer.gameObject.SetActive(true);
        }
        else
        {
            _inActiveContainer.gameObject.SetActive(true);
        }
    }

    public void DeactivateContainers()
    {
        _activeContainer.gameObject.SetActive(false);
        _inActiveContainer.gameObject.SetActive(false);
    }
}
