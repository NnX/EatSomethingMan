using System;
using System.Collections.Generic;
using Game.Model;
using Src.Misc;
using Src.Model.Objects;
using UnityEngine;
using UnityEngine.Events;

namespace Src.View
{
    [System.Serializable]
    public class CoinCollectedEvent : UnityEvent<int, int> {}

    public interface IVisualManager
    {
        void Init(Game.Model.IEventManager eventsManager, float iterationTime, Action onCherryConsumed);
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
        [SerializeField] private GameObject coinPrefab;
        [SerializeField] private GameObject cherryPrefab;
        [SerializeField] private CoinCollectedEvent coinEvent;

        private Sprite _cherryGhostSprite;
        private Sprite _ghostASprite;
        private Sprite _ghostBSprite;
        private float _iterationTime;
        private float _degrees;
        private Action _onCherryConsumed;
        private IPacMan _pacMan;
        private IGhost _ghostA;
        private IGhost _ghostB;
        private readonly List<Vector2Int> _freeSquares = new();

        private ICharactersFactory CharactersFactory => charactersFactory;
        private IPositionManager PositionManager => positionManager;

        public void SpawnCoins(int cherryPosition)
        {
            for (var x = 0; x < Constant.FieldWidth; x++)
            {
                for (var y = 0; y < Constant.FieldHeight; y++)
                {
                    if (((x * Constant.FieldHeight) + y) == cherryPosition)
                    {
                        continue;
                    }

                    var position = PositionManager.GetPosition(x, y);
                    var coin = Instantiate(coinPrefab, gameObjectsParent);
                    if (coin.TryGetComponent<Coin>(out var coinView))
                    {
                        coinView.coinEvent = coinEvent;
                        coinView.X = x;
                        coinView.Y = y;
                    }

                    coin.transform.localPosition = position;
                }
            }
        }

        public int SpawnCherry(bool isInit)
        {
            int cherryPositionX;
            int cherryPositionY;
            if (isInit)
            {
                cherryPositionX = UnityEngine.Random.Range(Constant.FieldWidth / 2, Constant.FieldWidth - 1);
                cherryPositionY = UnityEngine.Random.Range(Constant.FieldHeight / 2, Constant.FieldHeight - 1);
            }
            else
            {
                if (_freeSquares.Count == 0)
                {
                    Debug.LogWarning("No free squares available to respawn the cherry.");
                    return -1;
                }

                var spawnCherryCoordinates = _freeSquares[UnityEngine.Random.Range(0, _freeSquares.Count)];
                cherryPositionX = spawnCherryCoordinates.x;
                cherryPositionY = spawnCherryCoordinates.y;
            }

            var position = PositionManager.GetPosition(cherryPositionX, cherryPositionY);
            var cherry = Instantiate(cherryPrefab, gameObjectsParent);
            cherry.transform.localPosition = position;
            return (cherryPositionX * Constant.FieldHeight) + cherryPositionY;
        }

        void IVisualManager.Init(Game.Model.IEventManager eventsManager, float iterationTime, Action onCherryConsumed)
        {
            coinEvent ??= new CoinCollectedEvent();
            coinEvent.RemoveListener(CoinCollected);
            coinEvent.AddListener(CoinCollected);
            _freeSquares.Clear();
            _iterationTime = iterationTime;
            _onCherryConsumed = onCherryConsumed;
            _cherryGhostSprite = Resources.Load<Sprite>("Sprites/GhostCherry");
            _ghostASprite = Resources.Load<Sprite>("Sprites/Ghost1");
            _ghostBSprite = Resources.Load<Sprite>("Sprites/Ghost2");

            var pacManEvents = eventsManager.Get<Game.Model.IPacManEvents>();
            pacManEvents.OnCreatePacMan += OnCreatePacMan;
            pacManEvents.OnUpdatePacManPosition += OnUpdatePacManPosition;
            pacManEvents.OnCreateGhostA += OnCreateGhostA;
            pacManEvents.OnUpdateGhostAPosition += UpdateGhostAPosition;
            pacManEvents.OnCreateGhostB += OnCreateGhostB;
            pacManEvents.OnUpdateGhostBPosition += UpdateGhostBPosition;
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
            if (gameObjectsParent.TryGetComponent<GameField>(out var gameField))
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

        private void CoinCollected(int x, int y)
        {
            _freeSquares.Add(new Vector2Int(x, y));
        }

        private void UpdateGhostBPosition(int x, int y)
        {
            if (_ghostB.IsActive)
            {
                var position = PositionManager.GetPosition(x, y);
                _ghostB.UpdatePosition(position, _iterationTime);
            }
        }

        private void UpdateGhostAPosition(int x, int y)
        {
            if (_ghostA.IsActive)
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

        private void OnCreatePacMan(int x, int y)
        {
            var position = PositionManager.GetPosition(x, y);
            _pacMan = CharactersFactory.CreatePacMan(gameObjectsParent, position, _onCherryConsumed);
        }

        private void OnUpdatePacManPosition(int x, int y)
        {
            var position = PositionManager.GetPosition(x, y);
            _pacMan.UpdatePosition(position, _iterationTime);
            _pacMan.Rotate(_degrees);
        }
    }
}
