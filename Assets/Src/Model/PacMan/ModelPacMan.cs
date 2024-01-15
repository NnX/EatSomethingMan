using Game.Misc;
using UnityEngine.Events;
namespace Game.Model
{
    public interface IModelPacMan
    {
        IEventManager EventManager { get; }

        void Init(UnityEvent cherryEvent, LevelModelObject levelData);
        void Update(eDirection direction);
        void InitGhostA();
        void UpdateGhostA(eDirection direction, bool isScared);
        void InitGhostB();
        void UpdateGhostB(bool isScared);
    }
     
    public partial class ModelPacMan : ModelBase, IModelPacMan
    {
        private const int DirectionMaxSteps = 7;
        private ePacmanPosition _ePacmanPosition;
        private eDirection _eDirectionGhostBLast;
        private eDirection _eDirectionGhostBCurrent;
        private int _directionCounter;
        private UnityEvent _cherryEvent;

        protected override void RegisterEvents(IEventManagerInternal eventManager)
        {
            eventManager.Register<IPacManEvents, IPacManEventsWritable>(new PacManEvents());
        }

        IEventManager IModelPacMan.EventManager => EventManager;

        void IModelPacMan.Init(UnityEvent cherryEvent, LevelModelObject levelData)
        { 
            InitWalls(levelData);
            _cherryEvent = cherryEvent;
            CreateAndExecuteTurn(
                turn => { turn.Push(new CmdCreatePacMan(0, 0, _cherryEvent)); });
        }

        void IModelPacMan.Update(eDirection direction)
        {
            CreateAndExecuteTurn(
                turn => { turn.Push(new CmdMovePacMan(direction)); });
        }

        void IModelPacMan.InitGhostA()
        {
            CreateAndExecuteTurn(
                turn =>
                {
                    ICommand cmdCreateGhostA = new CmdCreateGhostA(15, 11);
                    turn.Push(cmdCreateGhostA);
                });
        }

        void IModelPacMan.UpdateGhostA(eDirection direction, bool isScared)
        {
            _ePacmanPosition = ePacmanPosition.DownDown;
            CreateAndExecuteTurn(
                turn =>
                {
                    var cmdMoveGhostA = new CmdMoveGhostA(direction, _ePacmanPosition, isScared);
                    turn.Push(cmdMoveGhostA);
                });
        }
        void IModelPacMan.InitGhostB()
        {
            CreateAndExecuteTurn(
                turn =>
                {
                    ICommand cmdCreateGhostB = new CmdCreateGhostB(8, 7);
                    turn.Push(cmdCreateGhostB);
                });
        }

        void IModelPacMan.UpdateGhostB(bool isScared)
        {
            CreateAndExecuteTurn(
                turn =>
                {
                    var cmdMoveGhostB = new CmdMoveGhostB(_eDirectionGhostBCurrent, isScared);
                    if (!isScared)
                    {
                        _eDirectionGhostBLast = _eDirectionGhostBCurrent;
                        _eDirectionGhostBCurrent = cmdMoveGhostB.GetDirection(_eDirectionGhostBCurrent, ContextWritable);

                        if (_eDirectionGhostBCurrent == _eDirectionGhostBLast)
                        {
                            _directionCounter++;
                        }
                        else
                        {
                            _directionCounter = 0;
                        }

                        if (_directionCounter == DirectionMaxSteps) // fix sticking to borders
                        {
                            while (_eDirectionGhostBLast == _eDirectionGhostBCurrent)
                            {
                                cmdMoveGhostB.ChangeDirection();
                                _eDirectionGhostBCurrent = cmdMoveGhostB.GetDirection(_eDirectionGhostBCurrent, ContextWritable);
                            }

                            _directionCounter = 0;
                        }
                    }

                    turn.Push(cmdMoveGhostB);
                });
        } 
    }
}