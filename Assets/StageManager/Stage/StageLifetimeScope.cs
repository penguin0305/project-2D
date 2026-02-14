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
    //[SerializeField] private EndPortal EndPortal;
    [SerializeField] private Boss Boss;
    

    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);

        //builder.RegisterComponentInHierarchy<EndPortal>().AsImplementedInterfaces();

        builder.RegisterComponent(Boss);
        builder.RegisterComponent(MapLoader);
        builder.RegisterComponent(CameraFollow);
        builder.RegisterComponent(StageManager);
        builder.RegisterComponent(SceneSetup);
    }
}
