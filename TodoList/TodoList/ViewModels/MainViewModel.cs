using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Common;
using TodoList.Common.Models;
using TodoList.Extension;

namespace TodoList.ViewModels
{
    public class MainViewModel : BindableBase, IConfigureService
    {
        public MainViewModel(IRegionManager regionManager)
        {
            menuBars = new ObservableCollection<MenuBar>();
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            this.regionManager = regionManager;

            GoBackCommand = new DelegateCommand(() =>
            {
                if (journal.CanGoBack)
                    journal.GoBack();
            });
            GoForWardCommand = new DelegateCommand(() =>
            {
                if (journal.CanGoForward)
                    journal.GoForward();
            });
        }

        private void Navigate(MenuBar bar)
        {
            if (bar == null || string.IsNullOrWhiteSpace(bar.NameSpace))
                return;
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(bar.NameSpace, callBack =>
            {
                journal = callBack.Context.NavigationService.Journal;
            });


        }

        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand GoForWardCommand { get; private set; }

        private readonly IRegionManager regionManager;
        private ObservableCollection<MenuBar> menuBars;
        private IRegionNavigationJournal journal;

        public ObservableCollection<MenuBar> MenuBars
        {
            get { return menuBars; }
            set { menuBars = value; RaisePropertyChanged(); }
        }
        void CreateMenuBars()
        {
            menuBars.Add(new MenuBar() { Icon = "Home", Title = "首页", NameSpace = "IndexView" });
            menuBars.Add(new MenuBar() { Icon = "Notebook", Title = "代办事项", NameSpace = "ToDoView" });
            menuBars.Add(new MenuBar() { Icon = "NotebookPlus", Title = "备忘录", NameSpace = "MemoView" });
            menuBars.Add(new MenuBar() { Icon = "CogOutline", Title = "设置", NameSpace = "SettingsView" });
        }

        public void Configure()
        {
            CreateMenuBars();
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("IndexView");
        }
    }
}
