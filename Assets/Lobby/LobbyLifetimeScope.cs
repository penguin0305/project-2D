using SupanthaPaul;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class LobbyLifetimeScope : LifetimeScope
{
    [SerializeField] private SceneSetup SceneSetup;
    [SerializeField] private CameraFollow CameraFollow;
    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);

        builder.RegisterComponent(SceneSetup);
        builder.RegisterComponent(CameraFollow);
    }
}
