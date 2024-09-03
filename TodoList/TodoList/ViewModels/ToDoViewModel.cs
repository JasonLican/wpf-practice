using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
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
using TodoList.Common;
using TodoList.Common.ModelDto;
using TodoList.Extension;

namespace TodoList.ViewModels
{
    public class ToDoViewModel : NavigationViewModel
    {
        private readonly IDialogHostService dialogHost;
        public ToDoViewModel(IContainerProvider provider) : base(provider)
        {
            ExcuteCommand = new DelegateCommand<string>(Excute);
            ToDoDtos = new ObservableCollection<ToDoDto>();
            SelectedCommand = new DelegateCommand<ToDoDto>(Selected);
            DelCommand = new DelegateCommand<ToDoDto>(Del);
            dialogHost = provider.Resolve<IDialogHostService>();
        }

        private void Excute(string obj)
        {
            switch (obj)
            {
                case "Add": AddTodo(); break;
                case "Search": Query(); break;
                case "Save": Save(); break;
            }
        }

        private int selectIndex;

        public int SelectIndex
        {
            get { return selectIndex; }
            set { selectIndex = value; }
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
        public DelegateCommand<ToDoDto> DelCommand { get; private set; }

        void InitData()
        {
            UpdateLoading(true);
            for (int i = 0; i < 20; i++)
            {
                ToDoDtos.Add(new ToDoDto() { Id = i + 1, Title = "标题" + i, Content = "测试内容。。。。", Status = 0 });
            }

            UpdateLoading(false);
        }

        private void AddTodo()
        {
            CurrentDto = new ToDoDto();
            IsRightDrawerOpen = true;
        }
        private void Selected(ToDoDto dto)
        {
            IsRightDrawerOpen = true;
            CurrentDto = ToDoDtos.FirstOrDefault(p => p.Id == dto.Id);
        }
        private void Save()
        {
            if (string.IsNullOrEmpty(CurrentDto.Title) || string.IsNullOrEmpty(CurrentDto.Content))
                return;

            if (CurrentDto.Id > 0)
            {
                var temp = ToDoDtos.FirstOrDefault(p => p.Id == CurrentDto.Id);
                if (temp != null)
                {
                    temp.Title = CurrentDto.Title;
                    temp.Content = CurrentDto.Content;
                    temp.Status = CurrentDto.Status;
                }
            }
            else
            {
                toDoDtos.Add(CurrentDto);
            }

            IsRightDrawerOpen = false;
        }

        private void Query()
        {
            if (string.IsNullOrEmpty(Search) && selectIndex == 0)
            {
                toDoDtos.Clear();
                InitData();
                return;
            }
            int? status = selectIndex == 0 ? null : selectIndex == 1 ? 0 : 1;

            var temp = toDoDtos.Where(p => (string.IsNullOrEmpty(Search) ? true : p.Title.Contains(Search)) && (status == null ? true : p.Status == status)).ToList();


            toDoDtos.Clear();
            foreach (var item in temp)
            {
                toDoDtos.Add(item);
            }
        }
        private async void Del(ToDoDto obj)
        {
            var dialogResult = await dialogHost.Question("温馨提示", $"确认删除待办事项:{obj.Title} ?");
            if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;
            if (obj != null)
                toDoDtos.Remove(obj);
        }
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            InitData();
        }
    }
}
