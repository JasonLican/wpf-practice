using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Common.ModelDto;

namespace TodoList.ViewModels
{
    public class ToDoViewModel : NavigationViewModel
    {
        public ToDoViewModel(IContainerProvider provider) : base(provider)
        {
            ExcuteCommand = new DelegateCommand<string>(Excute);
            ToDoDtos = new ObservableCollection<ToDoDto>();
            SelectedCommand = new DelegateCommand<ToDoDto>(Selected);
        }

        private void Excute(string obj)
        {
            switch (obj)
            {
                case "Add": AddTodo(); break;
                case "Search": Query(); break;
            }
        }

        private void Query()
        {
            if (string.IsNullOrEmpty(Search))
            {
                toDoDtos.Clear();
                InitData();
                return;
            }
            var temp= toDoDtos.Where(p => p.Title.Contains(Search)).ToList();
            toDoDtos.Clear();
            foreach (var item in temp)
            {
                toDoDtos.Add(item);
            }
        }

        private string search;

        public string Search
        {
            get { return search; }
            set { search = value; RaisePropertyChanged(); }
        }

        private ToDoDto currentDto;

        public ToDoDto CurrentDto
        {
            get { return currentDto; }
            set { currentDto = value; RaisePropertyChanged(); }
        }


        private ObservableCollection<ToDoDto> toDoDtos;

        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value; RaisePropertyChanged(); }
        }

        public DelegateCommand<ToDoDto> SelectedCommand { get; private set; }
        private bool isRightDrawerOpen;

        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }

        public DelegateCommand<string> ExcuteCommand { get; private set; }

        void InitData()
        {
            UpdateLoading(true);
            for (int i = 0; i < 20; i++)
            {
                ToDoDtos.Add(new ToDoDto() { Id = i, Title = "标题" + i, Content = "测试内容。。。。", Status = 1 });
            }

            UpdateLoading(false);
        }

        private void AddTodo()
        {
            IsRightDrawerOpen = true;
        }
        private void Selected(ToDoDto dto)
        {
            IsRightDrawerOpen = true;
            CurrentDto = ToDoDtos.FirstOrDefault(p => p.Id == dto.Id);
        }
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            InitData();
        }
    }
}
