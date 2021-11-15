using UnityEngine;

namespace UI
{
    public class UIGameController :  MonoBehaviourSingleton<UIGameController>
    {
        [SerializeField] private GameObject retryWindow;
        
        
        public override void InitializeSingleton()
        {
            SetAsPersistentSingleton();
        }


        public void TurnWindowOn()
        {
            retryWindow.SetActive(true);
        }
        
        public void TurnWindowOff()
        {
            retryWindow.SetActive(false);
        }
    }
}
