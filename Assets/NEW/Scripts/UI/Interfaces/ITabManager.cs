
public interface ITabManager 
{
    bool IsCurrentTabOpen { get; }
    ITabManager ObserverTab { get; set; }   

    void CloseCurrentTab(object[] data = null);
}
