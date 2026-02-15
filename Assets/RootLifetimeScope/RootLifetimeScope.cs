using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class RootLifetimeScope : LifetimeScope
{
    [SerializeField] private GameSceneManager GameSceneManager;
    [SerializeField] private List<EquipData> initialEquipments;
    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);

        builder.RegisterComponent(GameSceneManager);

        builder.Register<PlayerProvider>(Lifetime.Singleton);
        builder.Register<itemData>(Lifetime.Singleton);
        builder.Register<PlayerData>(Lifetime.Singleton);
        builder.Register<EquipStats>(Lifetime.Singleton).AsSelf();
        builder.RegisterBuildCallback(container =>
        {
            var stats = container.Resolve<EquipStats>();
            stats.InitializeBaseEquipment(initialEquipments);
        });
    }
}
