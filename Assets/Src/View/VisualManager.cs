using System;
using Game.Misc;
using Game.Model;
using UnityEngine;

namespace Game.View
{
    public interface IVisualManager
    {
        void Init(Model.IEventManager eventsManager, float iterationTime);
        void SpawnCoins(int cherryPosition);
        void RotatePacMan(float degrees);
        int SpawnCherry();
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

        float _iterationTime;
        float _degrees = 0;
        IPacMan _pacMan;
        public GameObject _coinPrefab;
        public GameObject _cherryPrefab;
        IGhostA _ghostA;
        IGhostB _ghostB;

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
                        c.transform.localPosition = position;
                    }

                }
            }
        }

        public int SpawnCherry()
        {
            int cherryPositionX = UnityEngine.Random.Range(Constant.FieldWidth / 2, Constant.FieldWidth - 1);
            int cherryPositionY = UnityEngine.Random.Range(Constant.FieldHeight / 2, Constant.FieldHeight - 1);

            Vector2 position = PositionManager.GetPosition(cherryPositionX, cherryPositionY);
            GameObject g = Instantiate(_cherryPrefab);
            g.transform.localPosition = position;

            return (cherryPositionX * Constant.FieldHeight) + cherryPositionY;
        }

        // ============ IVisualManager =================

        void IVisualManager.Init(Model.IEventManager eventsManager, float iterationTime)
        {
            _iterationTime = iterationTime;

            eventsManager.Get<Model.IPacManEvents>().OnCreatePacMan += OnCreatePacMan;
            eventsManager.Get<Model.IPacManEvents>().OnUpdatePacManPosition += OnUpdatePacManPosition;

            eventsManager.Get<Model.IPacManEvents>().OnCreateGhostA += OnCreateGhostA;
            eventsManager.Get<Model.IPacManEvents>().UpdateGhostAPosition += UpdateGhostAPosition;

            eventsManager.Get<Model.IPacManEvents>().OnCreateGhostB += OnCreateGhostB;
            eventsManager.Get<Model.IPacManEvents>().UpdateGhostBPosition += UpdateGhostBPosition;
            int cherryPosition = SpawnCherry();
            SpawnCoins(cherryPosition);
        }
 

        private void UpdateGhostBPosition(int x, int y)
        {
            Vector2 position = PositionManager.GetPosition(x, y);
            _ghostB.UpdatePosition(position, _iterationTime);
        }

        private void OnCreateGhostB(int x, int y)
        {
            Vector2 position = PositionManager.GetPosition(x, y);
            _ghostB = CharactersFactory.CreateGhostB(_gameObjectsParent, position);
        }

        private void UpdateGhostAPosition(int x, int y)
        {
            Vector2 position = PositionManager.GetPosition(x, y);
            _ghostA.UpdatePosition(position, _iterationTime);
        }

        private void OnCreateGhostA(int x, int y)
        {
            Vector2 position = PositionManager.GetPosition(x, y);
            _ghostA = CharactersFactory.CreateGhostA(_gameObjectsParent, position);
        }

        // =============================================

        void OnCreatePacMan(int x, int y)
        {
            Vector2 position = PositionManager.GetPosition(x, y);
            _pacMan = CharactersFactory.CreatePacMan(_gameObjectsParent, position);
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
    }
}