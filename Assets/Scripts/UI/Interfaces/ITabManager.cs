
public interface ITabManager 
{
    ITabManager ObserverTab { get; set; }

    void CloseCurrentTab(object[] data = null);
}
