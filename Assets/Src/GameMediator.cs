using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Game.Model;
using Game.Misc;

namespace Game
{
    public class GameMediator : MonoBehaviour
    {
        public const int FIELD_WIDTH = 16;
        public const float DINNER_TIME = 5f;
        public const float ACTIVATE_GHOST_TIME = 10f;
        const float ITERATION_TIME = 0.5f;
        eDirection current_direction = eDirection.RIGHT;
        eDirection ghost_direction = eDirection.DOWN;
        [SerializeField] View.VisualManager _visualManager;
        [SerializeField] GameObject playerSelectionMenu;

        public UnityEvent _cherryEvent = new UnityEvent();
        IModelPacMan _model = new ModelPacMan();
        bool _isCherryConsumed = false;
        float _dinnerTimestart; // TODO fix reset activate ghost timer if cherry is eaten again? Bug or feature?
        // ====================================

        View.IVisualManager VisualManager => _visualManager;

        // ====================================

        IEnumerator Start()
        {
            Time.timeScale = 0;
            VisualManager.Init(_model.EventManager, ITERATION_TIME);
            _cherryEvent.AddListener(CherryConsumed);
            _model.Init(_cherryEvent);
            _model.InitGhostA();
            _model.InitGhostB();

            //int cherryPosition = _visualManager.SpawnCherry(true);
            int cherryPosition = -1;
            //int cherryPosition = -1;
            _visualManager.SpawnCoins(cherryPosition);

            while (true)
            {
                if(_isCherryConsumed)
                {
                    float elapsedTime = Time.realtimeSinceStartup - _dinnerTimestart;
                    if (elapsedTime > DINNER_TIME)
                    {
                        _isCherryConsumed = false;
                        _visualManager.SpawnCherry(false);
                        _visualManager.ReturnGhostsToNormal();
                    }

                }

                if(_dinnerTimestart > 0 && (Time.realtimeSinceStartup - _dinnerTimestart > ACTIVATE_GHOST_TIME))
                {
                    _visualManager.ActivateGhosts();
                    _dinnerTimestart = 0;
                }
                _model.Update(current_direction);
                _model.UpdateGhostA(ghost_direction, _isCherryConsumed);
                _model.UpdateGhostB(_isCherryConsumed);
                yield return new WaitForSeconds(ITERATION_TIME);
 
            }
        }

        void Update()
        {
            bool isRight = (Input.GetAxis("DPadX") < -0.1f) ? true : false;
            bool isLeft = (Input.GetAxis("DPadX") > 0.1f) ? true : false;
            bool isDown = (Input.GetAxis("DPadY") < -0.1f) ? true : false;
            bool isUp = (Input.GetAxis("DPadY") > 0.1f) ? true : false;

            if (isUp)
            {
                ghost_direction = eDirection.UP;
            }
            if (isDown)
            {
                ghost_direction = eDirection.DOWN;
            }
            if (isRight)
            {
                ghost_direction = eDirection.RIGHT;
            }
            if (isLeft)
            {
                ghost_direction = eDirection.LEFT;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                current_direction = eDirection.UP;
                _visualManager.RotatePacMan(90);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                current_direction = eDirection.DOWN;
                _visualManager.RotatePacMan(270);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                current_direction = eDirection.RIGHT;
                _visualManager.RotatePacMan(0);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                current_direction = eDirection.LEFT;
                _visualManager.RotatePacMan(180);
            }
        }

        public void CherryConsumed()
        {
            print("[print][GameMediator] Cherry consumed");
            _isCherryConsumed = true;
            _dinnerTimestart = Time.realtimeSinceStartup;
            _visualManager.ScareGhosts();
        }

        public void OnPlayerSelectionClick(int playersAmount)
        {
            Time.timeScale = 1f;
            Constant.IsTwoPlayers = playersAmount == 2;
            playerSelectionMenu.SetActive(false);
        }
    }
}
