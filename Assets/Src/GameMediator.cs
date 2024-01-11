using System.Collections;
using Game.Misc;
using Game.Model;
using Src.Misc;
using Src.View;
using UnityEngine;
using UnityEngine.Events;

namespace Src
{
    public class GameMediator : MonoBehaviour
    {
        private const float DinnerTime = 5f;
        private const float ActivateGhostTime = 10f;
        private const float IterationTime = 0.5f;

        [SerializeField] private VisualManager visualManager;
        [SerializeField] private GameObject playerSelectionMenu;

        private readonly UnityEvent _cherryEvent = new();
        private eDirection _currentDirection = eDirection.RIGHT;
        private eDirection _ghostDirection = eDirection.DOWN;
        private readonly IModelPacMan _model = new ModelPacMan();
        private bool _isCherryConsumed;
        private float _dinnerTimeStart;

        private IVisualManager VisualManager => visualManager;

        private IEnumerator Start()
        {
            Time.timeScale = 0;
            VisualManager.Init(_model.EventManager, IterationTime);
            _cherryEvent.AddListener(CherryConsumed);
            _model.Init(_cherryEvent);
            _model.InitGhostA();
            _model.InitGhostB();

            var cherryPosition = visualManager.SpawnCherry(true);
            visualManager.SpawnCoins(cherryPosition);

            while (true)
            {
                if(_isCherryConsumed)
                {
                    var elapsedTime = Time.realtimeSinceStartup - _dinnerTimeStart;
                    if (elapsedTime > DinnerTime)
                    {
                        _isCherryConsumed = false;
                        visualManager.SpawnCherry(false);
                        visualManager.ReturnGhostsToNormal();
                    }
                }

                if(_dinnerTimeStart > 0 && (Time.realtimeSinceStartup - _dinnerTimeStart > ActivateGhostTime))
                {
                    visualManager.ActivateGhosts();
                    _dinnerTimeStart = 0;
                }
                _model.Update(_currentDirection);
                _model.UpdateGhostA(_ghostDirection, _isCherryConsumed);
                _model.UpdateGhostB(_isCherryConsumed);
                yield return new WaitForSeconds(IterationTime);
 
            } 
        }

        private void Update()
        {
            HandlePacManControls();
            HandleSecondPlayerControls();
        }

        private void HandlePacManControls()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _currentDirection = eDirection.UP;
                visualManager.RotatePacMan(90);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _currentDirection = eDirection.DOWN;
                visualManager.RotatePacMan(270);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _currentDirection = eDirection.RIGHT;
                visualManager.RotatePacMan(0);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _currentDirection = eDirection.LEFT;
                visualManager.RotatePacMan(180);
            }
        }

        private void HandleSecondPlayerControls()
        {
            var isRight = Input.GetAxis("DPadX") < -0.1f;
            var isLeft = Input.GetAxis("DPadX") > 0.1f;
            var isDown = Input.GetAxis("DPadY") < -0.1f;
            var isUp = Input.GetAxis("DPadY") > 0.1f;

            if (isUp)
            {
                _ghostDirection = eDirection.UP;
            }
            if (isDown)
            {
                _ghostDirection = eDirection.DOWN;
            }
            if (isRight)
            {
                _ghostDirection = eDirection.RIGHT;
            }
            if (isLeft)
            {
                _ghostDirection = eDirection.LEFT;
            }
        }

        private void CherryConsumed()
        {
            _isCherryConsumed = true;
            _dinnerTimeStart = Time.realtimeSinceStartup;
            visualManager.ScareGhosts();
        }

        public void OnPlayerSelectionClick(int playersAmount)
        {
            Time.timeScale = 1f;
            Constant.IsTwoPlayers = playersAmount == 2;
            playerSelectionMenu.SetActive(false);
        }
    }
}
