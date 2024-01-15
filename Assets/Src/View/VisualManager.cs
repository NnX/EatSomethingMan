using System.Collections.Generic;
using Game.Model;
using Src.Misc;
using Src.Model.Objects;
using UnityEngine;
using UnityEngine.Events;

namespace Src.View
{
    [System.Serializable]
    public class CoinCollectedEvent : UnityEvent<int, int>{}

    public class Pair {
        public int X { get; }
        public int Y { get; }

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
        LevelModelObject GetCurrentLevel();
    }

    public class VisualManager : MonoBehaviour, IVisualManager
    {
        [SerializeField] private Transform gameObjectsParent;
        [SerializeField] private CharactersFactory charactersFactory;
        [SerializeField] private PositionManager positionManager;

        [SerializeField] public GameObject coinPrefab;
        [SerializeField] public GameObject cherryPrefab;
        [SerializeField] public CoinCollectedEvent coinEvent;
        
        private Sprite _cherryGhostSprite;
        private Sprite _ghostASprite;
        private Sprite _ghostBSprite;
        private float _iterationTime;
        private float _degrees;
        private IPacMan _pacMan;
        private IGhost _ghostA;
        private IGhost _ghostB;
        private readonly List<Pair> _freeSquares = new();

        private ICharactersFactory CharactersFactory => charactersFactory;
        private IPositionManager PositionManager => positionManager;

        public void SpawnCoins(int cherryPosition)
        {
            for (var x = 0; x < Constant.FieldWidth; x++)
            {
                for (var y = 0; y < Constant.FieldHeight; y++)
                {
                    if(((x * Constant.FieldHeight) + y) != cherryPosition)
                    {
                        var position = PositionManager.GetPosition(x, y);
                        var c = Instantiate(coinPrefab);
                        if (c.TryGetComponent<Coin>(out var bitcoin))
                        {
                            bitcoin.coinEvent = coinEvent;
                            bitcoin.X = x;
                            bitcoin.Y = y; 
                        }
                        c.transform.localPosition = position;
                    }
                }
            }
        }

        public int SpawnCherry(bool isInit)
        {
            int cherryPositionX;
            int cherryPositionY;
            if (isInit)
            {
                cherryPositionX = Random.Range(Constant.FieldWidth / 2, Constant.FieldWidth - 1);
                cherryPositionY = Random.Range(Constant.FieldHeight / 2, Constant.FieldHeight - 1);
            }
            else
            {
                var spawnCherryCoordinates = _freeSquares[Random.Range(0, _freeSquares.Count)];
                cherryPositionX = spawnCherryCoordinates.X;
                cherryPositionY = spawnCherryCoordinates.Y;
            }

            var position = PositionManager.GetPosition(cherryPositionX, cherryPositionY);
            var g = Instantiate(cherryPrefab);
            g.transform.localPosition = position;
            return (cherryPositionX * Constant.FieldHeight) + cherryPositionY;
        }

        void IVisualManager.Init(Game.Model.IEventManager eventsManager, float iterationTime)
        {
            coinEvent.AddListener(CoinCollected);
            _iterationTime = iterationTime;
            _cherryGhostSprite = Resources.Load<Sprite>("Sprites/GhostCherry");
            _ghostASprite = Resources.Load<Sprite>("Sprites/Ghost1");
            _ghostBSprite = Resources.Load<Sprite>("Sprites/Ghost2");

            eventsManager.Get<Game.Model.IPacManEvents>().OnCreatePacMan += OnCreatePacMan;
            eventsManager.Get<Game.Model.IPacManEvents>().OnUpdatePacManPosition += OnUpdatePacManPosition;

            eventsManager.Get<Game.Model.IPacManEvents>().OnCreateGhostA += OnCreateGhostA;
            eventsManager.Get<Game.Model.IPacManEvents>().UpdateGhostAPosition += UpdateGhostAPosition;

            eventsManager.Get<Game.Model.IPacManEvents>().OnCreateGhostB += OnCreateGhostB;
            eventsManager.Get<Game.Model.IPacManEvents>().UpdateGhostBPosition += UpdateGhostBPosition;
        }

        public void RotatePacMan(float degrees)
        {
            _degrees = degrees;
        }

        public void ScareGhosts()
        {
            _ghostA.IsScared = true;
            _ghostB.IsScared = true;

            _ghostA.UpdateSprite(_cherryGhostSprite);
            _ghostB.UpdateSprite(_cherryGhostSprite);
        }

        public LevelModelObject GetCurrentLevel()
        {
            if(gameObjectsParent.TryGetComponent<GameField>(out var gameField))
            {
                return gameField.GetFirstLevel();
            }
            
            Debug.LogError("Error, no levels data");
            return null;
        }

        public void ReturnGhostsToNormal()
        {
            _ghostA.IsScared = false;
            _ghostB.IsScared = false;

            _ghostA.UpdateSprite(_ghostASprite);
            _ghostB.UpdateSprite(_ghostBSprite);
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
        

        private void CoinCollected(int x, int y)
        {
            _freeSquares.Add(new Pair(x, y));
        }
        private void UpdateGhostBPosition(int x, int y)
        {
            if(_ghostB.IsActive)
            {
                var position = PositionManager.GetPosition(x, y);
                _ghostB.UpdatePosition(position, _iterationTime);
            }
        }
        
        private void UpdateGhostAPosition(int x, int y)
        {
            if(_ghostA.IsActive)
            {
                var position = PositionManager.GetPosition(x, y);
                _ghostA.UpdatePosition(position, _iterationTime);
            }
        }

        private void OnCreateGhostB(int x, int y)
        {
            var position = PositionManager.GetPosition(x, y);
            _ghostB = CharactersFactory.CreateGhostB(gameObjectsParent, position);
        }

        private void OnCreateGhostA(int x, int y)
        {
            var position = PositionManager.GetPosition(x, y);
            _ghostA = CharactersFactory.CreateGhostA(gameObjectsParent, position);
        }

        private void OnCreatePacMan(int x, int y, UnityEvent unityEvent)
        {
            var position = PositionManager.GetPosition(x, y);
            _pacMan = CharactersFactory.CreatePacMan(gameObjectsParent, position, unityEvent);
        }

        private void OnUpdatePacManPosition(int x, int y)
        {
            var position = PositionManager.GetPosition(x, y);
            _pacMan.UpdatePosition(position, _iterationTime);
            _pacMan.Rotate(_degrees);
        }
    }
}