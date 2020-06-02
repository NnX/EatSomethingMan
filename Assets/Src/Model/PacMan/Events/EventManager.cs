namespace Game.Model
{
    public delegate void dCreatePacMan(int x, int y);
    public delegate void dCreateGhostA(int x, int y);
    public delegate void dCreateGhostB(int x, int y);
    public delegate void dUpdateGhostAPosition(int x, int y);
    public delegate void dUpdateGhostBPosition(int x, int y);
    public delegate void dUpdatePacManPosition(int x, int y);

    public interface IPacManEvents
    {
        event dCreatePacMan OnCreatePacMan;
        event dUpdatePacManPosition OnUpdatePacManPosition;
 
        event dCreateGhostA OnCreateGhostA;
        event dCreateGhostB OnCreateGhostB;
        event dUpdatePacManPosition UpdateGhostAPosition;
        event dUpdatePacManPosition UpdateGhostBPosition;
    }

    public interface IPacManEventsWritable
    {
        void CreatePacMan(int x, int y);
        void UpdatePacManPosition(int x, int y);
 
        void CreateGhostA(int x, int y);
        void CreateGhostB(int x, int y);
        void UpdateGhostAPosition(int x, int y);
        void UpdateGhostBPosition(int x, int y);
    }

    // #############################################

    class PacManEvents : IPacManEvents, IPacManEventsWritable
    {
        // ========= IPacManEvents ================

        public event dCreatePacMan OnCreatePacMan;
        public event dUpdatePacManPosition OnUpdatePacManPosition;
        public event dCreateGhostA OnCreateGhostA;
        public event dCreateGhostB OnCreateGhostB;
        public event dUpdatePacManPosition UpdateGhostAPosition;
        public event dUpdatePacManPosition UpdateGhostBPosition;

        // ========= IPacManEventsWritable =========

        void IPacManEventsWritable.CreatePacMan(int x, int y)
        { OnCreatePacMan?.Invoke(x, y); }

        void IPacManEventsWritable.UpdatePacManPosition(int x, int y)
        { OnUpdatePacManPosition?.Invoke(x, y); }

        public void CreateGhostA(int x, int y)
        { OnCreateGhostA?.Invoke(x,y); }

        public void CreateGhostB(int x, int y)
        { OnCreateGhostB?.Invoke(x, y); }

        void IPacManEventsWritable.UpdateGhostAPosition(int x, int y)
        { UpdateGhostAPosition?.Invoke(x, y); }

        void IPacManEventsWritable.UpdateGhostBPosition(int x, int y)
        { UpdateGhostBPosition?.Invoke(x, y); }
    }
}
