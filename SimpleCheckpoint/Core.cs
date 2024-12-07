using MelonLoader;
using Rewired;
using UnityEngine;

[assembly: MelonInfo(typeof(SimpleCheckpoint.Core), "SimpleCheckpoint", "1.0.0", "Penny", null)]
[assembly: MelonGame("Isto", "Get To Work")]

namespace SimpleCheckpoint
{
    public class Core : MelonMod
    {
        private bool _initialized;
        private GameObject _playerObject;
        private Rigidbody _playerRigidbody;
        private Controller.Button _buttonY;
        private Controller.Button _buttonA;
        private Vector3? _savedPosition;
        
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Loaded!");
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (sceneName != "PlayerEssentials") return;

            _playerObject = GameObject.Find("Player/PlayerPhysics");
            _playerRigidbody = _playerObject.GetComponent<Rigidbody>();
            
            foreach (Controller controller in ReInput.controllers.Controllers)
            {
                if (controller.identifier.hardwareIdentifier != "WindowsXInputGamepadGamepad") continue;
                _buttonA = controller.Buttons[0];
                _buttonY = controller.Buttons[3];
            }
            
            _initialized = true;
            LoggerInstance.Msg("Initialized!");
        }

        public override void OnUpdate()
        {
            if (!_initialized) return;
            if (_playerObject == null) return;
            if (Input.GetKeyDown(KeyCode.PageDown) || _buttonA.value)
            {
                if (_savedPosition == null) return;
                _playerObject.transform.position = (Vector3)_savedPosition;
                _playerRigidbody.velocity = Vector3.zero;
                LoggerInstance.Msg("Teleported!");
            }
            if (Input.GetKeyDown(KeyCode.PageUp) || _buttonY.value)
            {
                _savedPosition = _playerObject.transform.position;
                LoggerInstance.Msg("Saved Position!" + _savedPosition.Value);
            }
        }
    }
}