using Enums;

namespace Input {
    public class InputManager : MonoSingleton<InputManager> {
        private GameActions _gameActions;
        private UIActions _uiActions;

        void Start() {
            _gameActions = GetComponent<GameActions>();
            _uiActions = GetComponent<UIActions>();
        }

        public void SwitchContext(GameContext newGameContext) {
            if (newGameContext == GameContext.UI) {
                _gameActions.enabled = false;
                _uiActions.enabled = true;
            }
            else {
                _gameActions.enabled = true;
                _uiActions.enabled = false;
            }
        }
    }
}
