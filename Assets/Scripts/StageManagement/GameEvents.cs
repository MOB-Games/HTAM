using System;

public static class GameEvents
{
    public static event Action OnLoadStage;
    
    public static event Action OnOpenLoadingScreen;
    
    public static void LoadStage()
    {
        OnLoadStage?.Invoke();
    }
    
    public static void OpenLoadingScreen()
    {
        OnOpenLoadingScreen?.Invoke();
    }
}
