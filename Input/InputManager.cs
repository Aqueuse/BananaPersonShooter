using Enums;

namespace Input {
    public class InputManager : MonoSingleton<InputManager> {
        public GameContext gameContext;

        private GameActions _gameActions;
        private UIActions _uiActions;

        void Start() {
            gameContext = GameContext.UI;

            _gameActions = GetComponent<GameActions>();
            _uiActions = GetComponent<UIActions>();
        }

        public void SwitchContext(GameContext newGameContext) {
            gameContext = newGameContext;
            
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
