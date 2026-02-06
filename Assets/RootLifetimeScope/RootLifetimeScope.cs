using VContainer;
using VContainer.Unity;
using UnityEngine;

public class RootLifetimeScope : LifetimeScope
{
    [SerializeField] private GameSceneManager GameSceneManager;
    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);

        builder.RegisterComponent(GameSceneManager);
        builder.Register<PlayerProvider>(Lifetime.Singleton);

        builder.Register<itemData>(Lifetime.Singleton);
        builder.Register<EquipStats>(Lifetime.Singleton);
        builder.Register<PlayerData>(Lifetime.Singleton);
    }
}
