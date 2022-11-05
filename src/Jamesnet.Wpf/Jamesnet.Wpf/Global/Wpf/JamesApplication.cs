﻿using Jamesnet.Wpf.Global.Location;
using Jamesnet.Wpf.Global.Evemt;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System.Collections.Generic;
using Unity;

namespace Jamesnet.Wpf.Controls
{
    public interface ISmartContainerFactory
    {

    }

    public class SmartContainerFactory : ISmartContainerFactory
    {
        public SmartContainerFactory()
        {

        }
    }

    public abstract class JamesApplication : PrismApplication
    {
        private List<IModule> _modules = new();

        public JamesApplication AddInversionModule<T>() where T : IModule, new()
        {
            IModule module = new T();
            _modules.Add(module);

            return this;
        }

        public JamesApplication AddWireDataContext<T>() where T : ViewModelLocationScenario, new()
        {
            ViewModelLocationScenario scenario = new T();
            _ = scenario.Publish();

            return this;
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            foreach (IModule module in _modules)
            {
                moduleCatalog.AddModule(module.GetType());
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(typeof(IEventHub), EventAggregatorBuilder.Get(containerRegistry.GetContainer().Resolve<IEventAggregator>()));
        }
    }
}
