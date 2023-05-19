
public class NewGameOptionsHandler : BaseGameOptionsHandler
{
    protected override void Execute()
    {
        ChangeScene();
    }

    private void ChangeScene()
    {
        MyScene.Manager.LoadScene(MyScene.SceneName.Game);
    }
}
