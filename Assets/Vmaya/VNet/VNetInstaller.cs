using Mirror;
using UnityEngine;
using Zenject;
using Vmaya.NetMsg;

namespace Vmaya.VNet
{
    public class VNetInstaller : MonoInstaller
    {
        [SerializeField] private VUIManager _uiManager;
        [SerializeField] private VSubscriptionClient _subscription;

        public override void InstallBindings()
        {
            Container.Bind<VUIManager>()
                .FromInstance(_uiManager)
                .AsSingle()
                .NonLazy();

            Container.Bind<VSubscriptionClient>()
                .FromInstance(_subscription)
                .AsSingle()
                .NonLazy();
        }
    }
}