using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Common.Events;

namespace TodoList.ViewModels
{
    public class NavigationViewModel : BindableBase, INavigationAware
    {
        public NavigationViewModel(IContainerProvider provider)
        {
            this.provider = provider;
            aggregator = provider.Resolve<IEventAggregator>();
        }

        public readonly IContainerProvider provider;
        public readonly IEventAggregator aggregator;

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {

        }
        public void UpdateLoading(bool isOpen)
        {
            aggregator.UpdateLoading(new UpdateModel() { IsOpen = isOpen });
        }
    }
}
