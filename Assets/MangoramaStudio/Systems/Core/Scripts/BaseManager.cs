using UnityEngine;
using UnityEngine.UI;

namespace MangoramaStudio.Scripts.Managers
{

    public class BaseManager : MonoBehaviour
    {
        public GameManager GameManager { get; set; }

        public virtual void Initialize(GameManager gameManager)
        {
            GameManager = gameManager;
        }
    }
}