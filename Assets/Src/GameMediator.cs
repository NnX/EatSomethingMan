using System.Collections;
using Game.Misc;
using Game.Model;
using Src.View;
using UnityEngine;
using UnityEngine.Events;

namespace Src
{
    public class GameMediator : MonoBehaviour
    {
        public const int FieldWidth = 16;
        private const float DinnerTime = 5f;
        private const float ActivateGhostTime = 10f;
        private const float IterationTime = 0.5f;

        [SerializeField] VisualManager _visualManager;
        [SerializeField] GameObject playerSelectionMenu;

        public UnityEvent _cherryEvent = new UnityEvent();
        private eDirection _currentDirection = eDirection.RIGHT;
        private eDirection _ghostDirection = eDirection.DOWN;
        private readonly IModelPacMan _model = new ModelPacMan();
        private bool _isCherryConsumed;
        private float _dinnerTimeStart; // TODO fix reset activate ghost timer if cherry is eaten again? Bug or feature?
        // ====================================

        IVisualManager VisualManager => _visualManager;

        // ====================================

        private IEnumerator Start()
        {
            Time.timeScale = 0;
            VisualManager.Init(_model.EventManager, IterationTime);
            _cherryEvent.AddListener(CherryConsumed);
            _model.Init(_cherryEvent);
            _model.InitGhostA();
            _model.InitGhostB();

            //int cherryPosition = _visualManager.SpawnCherry(true);
            const int cherryPosition = -1;
            //int cherryPosition = -1;
            _visualManager.SpawnCoins(cherryPosition);

            while (true)
            {
                if(_isCherryConsumed)
                {
                    var elapsedTime = Time.realtimeSinceStartup - _dinnerTimeStart;
                    if (elapsedTime > DinnerTime)
                    {
                        _isCherryConsumed = false;
                        _visualManager.SpawnCherry(false);
                        _visualManager.ReturnGhostsToNormal();
                    }

                }

                if(_dinnerTimeStart > 0 && (Time.realtimeSinceStartup - _dinnerTimeStart > ActivateGhostTime))
                {
                    _visualManager.ActivateGhosts();
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

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _currentDirection = eDirection.UP;
                _visualManager.RotatePacMan(90);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _currentDirection = eDirection.DOWN;
                _visualManager.RotatePacMan(270);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _currentDirection = eDirection.RIGHT;
                _visualManager.RotatePacMan(0);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _currentDirection = eDirection.LEFT;
                _visualManager.RotatePacMan(180);
            }
        }

        private void CherryConsumed()
        {
            print("[print][GameMediator] Cherry consumed");
            _isCherryConsumed = true;
            _dinnerTimeStart = Time.realtimeSinceStartup;
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
