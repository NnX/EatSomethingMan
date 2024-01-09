using System.Collections.Generic;
using Src.Model.Objects;
using UnityEngine;
using UnityEngine.Events;

namespace Src.View
{
    [System.Serializable]
    public class CoinCollectedEvent : UnityEvent<int, int>{}

    public class Pair {
        public int X { get; set; }
        public int Y { get; set; }

        public Pair(int x , int y)
        {
            X = x;
            Y = y;
        }
    }

    public interface IVisualManager
    {
        void Init(Game.Model.IEventManager eventsManager, float iterationTime);
        void SpawnCoins(int cherryPosition);
        void RotatePacMan(float degrees);
        int SpawnCherry(bool isInit);
        void ScareGhosts();
    }

    // #################################################
    public class VisualManager : MonoBehaviour, IVisualManager
    {
        [SerializeField] Transform _gameObjectsParent;
        [SerializeField] CharactersFactory _charactersFactory;
        [SerializeField] PositionManager _positionManager;

        public GameObject _coinPrefab;
        public GameObject _cherryPrefab;
        public CoinCollectedEvent _coinEvent;
        
        private Sprite _cherryGhostSprite;
        private Sprite _GhostASprite;
        private Sprite _GhostBSprite;
        private float _iterationTime;
        private float _degrees = 0;
        private IPacMan _pacMan;
        private IGhostA _ghostA;
        private IGhostB _ghostB;
        private readonly List<Pair> _freeSquares = new();

        // =============================================

        ICharactersFactory CharactersFactory => _charactersFactory;
        IPositionManager PositionManager => _positionManager;

        public void SpawnCoins(int cherryPosition)
        {
            for (var x = 0; x < Constant.FieldWidth; x++)
            {
                for (var y = 0; y < Constant.FieldHeight; y++)
                {
                    if(((x * Constant.FieldHeight) + y) != cherryPosition)
                    {
                        var position = PositionManager.GetPosition(x, y);
                        var c = Instantiate(_coinPrefab) as GameObject;
                        var bitcoin = c.GetComponent<Coin>();
                        bitcoin.coinEvent = _coinEvent;
                        bitcoin.X = x;
                        bitcoin.Y = y;
                        c.transform.localPosition = position;
                    }
                }
            }
        }

        public int SpawnCherry(bool isInit)
        {
            int cherryPositionX;
            int cherryPositionY;
            if(isInit)
            {
                cherryPositionX = Random.Range(Constant.FieldWidth / 2, Constant.FieldWidth - 1);
                cherryPositionY = Random.Range(Constant.FieldHeight / 2, Constant.FieldHeight - 1);

            } else
            {
                var spawnCherryCoordinates = _freeSquares[Random.Range(0, _freeSquares.Count)];
                cherryPositionX = spawnCherryCoordinates.X;
                cherryPositionY = spawnCherryCoordinates.Y;
            }

            var position = PositionManager.GetPosition(cherryPositionX, cherryPositionY);
            var g = Instantiate(_cherryPrefab);
            g.transform.localPosition = position;
            return (cherryPositionX * Constant.FieldHeight) + cherryPositionY;
        }

        // ============ IVisualManager =================

        void IVisualManager.Init(Game.Model.IEventManager eventsManager, float iterationTime)
        {
            _coinEvent.AddListener(CoinCollected);
            _iterationTime = iterationTime;
            _cherryGhostSprite = Resources.Load<Sprite>("Sprites/GhostCherry");
            _GhostASprite = Resources.Load<Sprite>("Sprites/Ghost1");
            _GhostBSprite = Resources.Load<Sprite>("Sprites/Ghost2");

            eventsManager.Get<Game.Model.IPacManEvents>().OnCreatePacMan += OnCreatePacMan;
            eventsManager.Get<Game.Model.IPacManEvents>().OnUpdatePacManPosition += OnUpdatePacManPosition;

            eventsManager.Get<Game.Model.IPacManEvents>().OnCreateGhostA += OnCreateGhostA;
            eventsManager.Get<Game.Model.IPacManEvents>().UpdateGhostAPosition += UpdateGhostAPosition;

            eventsManager.Get<Game.Model.IPacManEvents>().OnCreateGhostB += OnCreateGhostB;
            eventsManager.Get<Game.Model.IPacManEvents>().UpdateGhostBPosition += UpdateGhostBPosition;
        }

        private void UpdateGhostBPosition(int x, int y)
        {
            if(_ghostB.IsActive)
            {
                var position = PositionManager.GetPosition(x, y);
                _ghostB.UpdatePosition(position, _iterationTime);
            }
        }

        private void OnCreateGhostB(int x, int y)
        {
            var position = PositionManager.GetPosition(x, y);
            _ghostB = CharactersFactory.CreateGhostB(_gameObjectsParent, position);
        }

        private void UpdateGhostAPosition(int x, int y)
        {
            if(_ghostA.IsActive)
            {
                var position = PositionManager.GetPosition(x, y);
                _ghostA.UpdatePosition(position, _iterationTime);
            }
        }

        private void OnCreateGhostA(int x, int y)
        {
            var position = PositionManager.GetPosition(x, y);
            _ghostA = CharactersFactory.CreateGhostA(_gameObjectsParent, position);
        }

        // =============================================

        private void OnCreatePacMan(int x, int y, UnityEvent unityEvent)
        {
            var position = PositionManager.GetPosition(x, y);
            _pacMan = CharactersFactory.CreatePacMan(_gameObjectsParent, position, unityEvent);
        }

        void OnUpdatePacManPosition(int x, int y)
        {
            var position = PositionManager.GetPosition(x, y);
            _pacMan.UpdatePosition(position, _iterationTime);
            _pacMan.Rotate(_degrees);
        }

        public void RotatePacMan(float degrees)
        {
            _degrees = degrees;
        }

        private void CoinCollected(int x, int y)
        {
            _freeSquares.Add(new Pair(x, y));
        }

        public void ScareGhosts()
        {
            _ghostA.IsScared = true;
            _ghostB.IsScared = true;

            _ghostA.UpdateSprite(_cherryGhostSprite);
            _ghostB.UpdateSprite(_cherryGhostSprite);
        }

        public void ReturnGhostsToNormal()
        {
            _ghostA.IsScared = false;
            _ghostB.IsScared = false;

            _ghostA.UpdateSprite(_GhostASprite);
            _ghostB.UpdateSprite(_GhostBSprite);
        }

        public void ActivateGhosts()
        {
            if(!_ghostA.IsActive)
            {
                _ghostA.IsActive = true;
            }

            if(!_ghostB.IsActive)
            {
                _ghostB.IsActive = true;
            }
        }
    }
}