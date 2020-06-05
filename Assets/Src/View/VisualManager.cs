using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Game.View
{
    [System.Serializable]
    public class CoinCollectedEvent : UnityEvent<int, int>
    {
    }

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
        void Init(Model.IEventManager eventsManager, float iterationTime);
        void SpawnCoins(int cherryPosition);
        void RotatePacMan(float degrees);
        int SpawnCherry(bool isInit);
        void ScareGhosts();
    }

    // #################################################

    public class VisualManager : MonoBehaviour, IVisualManager
    {
        [SerializeField]
        Transform _gameObjectsParent;
        [SerializeField]
        CharactersFactory _charactersFactory;
        [SerializeField]
        PositionManager _positionManager;

        Sprite _cherryGhostSprite;
        Sprite _GhostASprite;
        Sprite _GhostBSprite;

        float _iterationTime;
        float _degrees = 0;
        IPacMan _pacMan;
        public GameObject _coinPrefab;
        public GameObject _cherryPrefab;
        IGhostA _ghostA;
        IGhostB _ghostB;
        public CoinCollectedEvent _coinEvent;
        List<Pair> _freeSqares = new List<Pair>();

        // =============================================

        ICharactersFactory CharactersFactory => _charactersFactory;
        IPositionManager PositionManager => _positionManager;

        public void SpawnCoins(int cherryPosition)
        {
            for (int x = 0; x < Constant.FieldWidth; x++)
            {
                for (int y = 0; y < Constant.FieldHeight; y++)
                {
                    if(((x * Constant.FieldHeight) + y) != cherryPosition)
                    {
                        Vector2 position = PositionManager.GetPosition(x, y);
                        GameObject c = Instantiate(_coinPrefab) as GameObject;
                        coin bitcoin = c.GetComponent<coin>();
                        bitcoin._coinEvent = _coinEvent;
                        bitcoin.X = x;
                        bitcoin.Y = y;
                        c.transform.localPosition = position;
                    }
                }
            }
        }

        public int SpawnCherry(bool isInit)
        {
            int cherryPositionX = 0;
            int cherryPositionY = 0;
            if(isInit)
            {
                cherryPositionX = UnityEngine.Random.Range(Constant.FieldWidth / 2, Constant.FieldWidth - 1);
                cherryPositionY = UnityEngine.Random.Range(Constant.FieldHeight / 2, Constant.FieldHeight - 1);

            } else
            {
                Pair spawnCHerryCoordinates = _freeSqares[UnityEngine.Random.Range(0, _freeSqares.Count)];
                cherryPositionX = spawnCHerryCoordinates.X;
                cherryPositionY = spawnCHerryCoordinates.Y;
            }

            Vector2 position = PositionManager.GetPosition(cherryPositionX, cherryPositionY);
            GameObject g = Instantiate(_cherryPrefab);
            g.transform.localPosition = position;
            return (cherryPositionX * Constant.FieldHeight) + cherryPositionY;
        }

        // ============ IVisualManager =================

        void IVisualManager.Init(Model.IEventManager eventsManager, float iterationTime)
        {
            _coinEvent.AddListener(CoinCollected);
            _iterationTime = iterationTime;
            _cherryGhostSprite = Resources.Load<Sprite>("Sprites/GhostCherry");
            _GhostASprite = Resources.Load<Sprite>("Sprites/Ghost1");
            _GhostBSprite = Resources.Load<Sprite>("Sprites/Ghost2");

            eventsManager.Get<Model.IPacManEvents>().OnCreatePacMan += OnCreatePacMan;
            eventsManager.Get<Model.IPacManEvents>().OnUpdatePacManPosition += OnUpdatePacManPosition;

            eventsManager.Get<Model.IPacManEvents>().OnCreateGhostA += OnCreateGhostA;
            eventsManager.Get<Model.IPacManEvents>().UpdateGhostAPosition += UpdateGhostAPosition;

            eventsManager.Get<Model.IPacManEvents>().OnCreateGhostB += OnCreateGhostB;
            eventsManager.Get<Model.IPacManEvents>().UpdateGhostBPosition += UpdateGhostBPosition;
        }

        private void UpdateGhostBPosition(int x, int y)
        {
            if(_ghostB.isActive)
            {
                Vector2 position = PositionManager.GetPosition(x, y);
                _ghostB.UpdatePosition(position, _iterationTime);
            }
        }

        private void OnCreateGhostB(int x, int y)
        {
            Vector2 position = PositionManager.GetPosition(x, y);
            _ghostB = CharactersFactory.CreateGhostB(_gameObjectsParent, position);
        }

        private void UpdateGhostAPosition(int x, int y)
        {
            if(_ghostA.isActive)
            {
                Vector2 position = PositionManager.GetPosition(x, y);
                _ghostA.UpdatePosition(position, _iterationTime);
            }
        }

        private void OnCreateGhostA(int x, int y)
        {
            Vector2 position = PositionManager.GetPosition(x, y);
            _ghostA = CharactersFactory.CreateGhostA(_gameObjectsParent, position);
        }

        // =============================================

        void OnCreatePacMan(int x, int y, UnityEvent unityEvent)
        {
            Vector2 position = PositionManager.GetPosition(x, y);
            _pacMan = CharactersFactory.CreatePacMan(_gameObjectsParent, position, unityEvent);
        }

        void OnUpdatePacManPosition(int x, int y)
        {
            Vector2 position = PositionManager.GetPosition(x, y);
            _pacMan.UpdatePosition(position, _iterationTime);
            _pacMan.Rotate(_degrees);
        }

        public void RotatePacMan(float degrees)
        {
            _degrees = degrees;
        }

        public void CoinCollected(int x, int y)
        {
            _freeSqares.Add(new Pair(x, y));
        }

        public void ScareGhosts()
        {
            _ghostA.isScared = true;
            _ghostB.isScared = true;

            _ghostA.UpdateSprite(_cherryGhostSprite);
            _ghostB.UpdateSprite(_cherryGhostSprite);
        }

        public void ReturnGhostsToNormal()
        {
            _ghostA.isScared = false;
            _ghostB.isScared = false;

            _ghostA.UpdateSprite(_GhostASprite);
            _ghostB.UpdateSprite(_GhostBSprite);
        }

        public void ActivateGhosts()
        {
            if(!_ghostA.isActive)
            {
                _ghostA.isActive = true;
            }

            if(!_ghostB.isActive)
            {
                _ghostB.isActive = true;
            }
        }
    }
}