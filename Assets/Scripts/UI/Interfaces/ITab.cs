
public interface ITab 
{
    ITab ObserverTab { get; set; }

    void CloseCurrentTab(object[] data = null);
}
