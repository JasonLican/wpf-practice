using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using TodoList.Common.Models;
using TodoList.Extension;

namespace TodoList.ViewModels
{
    public class SettingsViewModel : BindableBase
    {
        public SettingsViewModel(IRegionManager regionManager)
        {
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            this.regionManager = regionManager;
            menuBars = new ObservableCollection<MenuBar>();
            CreateMenuBars();
        }
        private void Navigate(MenuBar bar)
        {
            if (bar == null || string.IsNullOrWhiteSpace(bar.NameSpace))
                return;
            regionManager.Regions[PrismManager.SettingViewRegionName].RequestNavigate(bar.NameSpace);


        }
        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
        private readonly IRegionManager regionManager;
        private ObservableCollection<MenuBar> menuBars;

        public ObservableCollection<MenuBar> MenuBars
        {
            get { return menuBars; }
            set { menuBars = value; RaisePropertyChanged(); }
        }
        void CreateMenuBars()
        {
            menuBars.Add(new MenuBar() { Icon = "Palette", Title = "个性化", NameSpace = "SkinView" });
            menuBars.Add(new MenuBar() { Icon = "Cog", Title = "系统设置", NameSpace = "" });
            menuBars.Add(new MenuBar() { Icon = "Information", Title = "关于更多", NameSpace = "AboutView" });
        }
    }
}
