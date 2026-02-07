using SupanthaPaul;
using VContainer;
using VContainer.Unity;
using UnityEngine;

public class StageLifetimeScope : LifetimeScope
{
    [SerializeField] private CameraFollow CameraFollow;
    [SerializeField] private StageManager StageManager;
    [SerializeField] private SceneSetup SceneSetup;
    [SerializeField] private MapLoader MapLoader;

    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);

        builder.RegisterComponent(MapLoader);
        builder.RegisterComponent(CameraFollow);
        builder.RegisterComponent(StageManager);
        builder.RegisterComponent(SceneSetup);
    }
}
