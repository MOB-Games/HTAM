using System;
using System.Collections.Generic;

public static class TownEvents
{
    public static event Action<TownInfo, bool, string> OnPublishTownInfo;
    public static event Action<List<CharacterTownInfo>> OnLoadedCharacters;
    public static event Action OnOpenSignpost;
    public static event Action OnCloseSignpost;
    public static event Action OnOpenInn;
    public static event Action OnCloseInn;
    public static event Action OnOpenShop;
    public static event Action OnCloseShop;
    public static event Action OnOpenBlacksmith;
    public static event Action OnCloseBlacksmith;
    public static event Action<CharacterTownInfo> OnCharacterSelected;
    public static event Action<int> OnGoldSpent;
   
    public static void PublishTownInfo(TownInfo townInfo, bool pathCleared, string previousPathSignpost)
    {
        OnPublishTownInfo?.Invoke(townInfo, pathCleared, previousPathSignpost);
    }
    
    public static void LoadedCharacters(List<CharacterTownInfo> characterTownInfos)
    {
        OnLoadedCharacters?.Invoke(characterTownInfos);
    }
    
    public static void OpenSignpost()
    {
        OnOpenSignpost?.Invoke();
    }
    
    public static void CloseSignpost()
    {
        OnCloseSignpost?.Invoke();
    }
    
    public static void OpenInn()
    {
        OnOpenInn?.Invoke();
    }
    
    public static void CloseInn()
    {
        OnCloseInn?.Invoke();
    }
    
    public static void OpenShop()
    {
        OnOpenShop?.Invoke();
    }
    
    public static void CloseShop()
    {
        OnCloseShop?.Invoke();
    }
    
    public static void OpenBlacksmith()
    {
        OnOpenBlacksmith?.Invoke();
    }
    
    public static void CloseBlacksmith()
    {
        OnCloseBlacksmith?.Invoke();
    }
    
    public static void CharacterSelected(CharacterTownInfo characterTownInfo)
    {
        OnCharacterSelected?.Invoke(characterTownInfo);
    }
    
    public static void GoldSpent(int amount)
    {
        OnGoldSpent?.Invoke(amount);
    }
}
