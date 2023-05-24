namespace Sources.Lesson4GameSystem.GameSystem
{
    public interface IGameListener
    {
    }
    
    public interface IGameStartedListener : IGameListener
    {
        public void OnGameStarted();
    }
    
    public interface IGameFinishedListener : IGameListener
    {
        public void OnGameFinished();
    }
    
    public interface IGamePausedListener : IGameListener
    {
        public void OnGamePaused();
    }
    
    public interface IGameResumedListener : IGameListener
    {
        public void OnGameResumed();
    }

    public interface IGameUpdatedListener : IGameListener
    {
        public void OnGameUpdated(float deltaTime);
    }
}
