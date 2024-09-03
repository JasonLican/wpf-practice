using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TodoList.Common;
using TodoList.Common.ModelDto;
using TodoList.Common.Models;
using TodoList.Extension;

namespace TodoList.ViewModels
{
    public class IndexViewModel : NavigationViewModel
    {
        private readonly IDialogHostService dialog;
        private readonly IRegionManager _regionManager;
        public IndexViewModel(IDialogHostService dialog, IContainerProvider provider) : base(provider)
        {
            taskBars = new ObservableCollection<TaskBar>();
            ExcuteCommand = new DelegateCommand<string>(Excute);
            EditToDoCommand = new DelegateCommand<ToDoDto>(AddToDo);
            EditMemoCommand = new DelegateCommand<MemoDto>(AddMemo);
            TodoCompleteCommand = new DelegateCommand<ToDoDto>(TodoComplete);
            NavigateCommand = new DelegateCommand<TaskBar>(Navigate);
            _regionManager = provider.Resolve<IRegionManager>();
            CreateInitData();
            this.dialog = dialog;
        }

        private void Navigate(TaskBar bar)
        {
            if (string.IsNullOrEmpty(bar.Target)) return;
            NavigationParameters param = new NavigationParameters();
            if (bar.Title == "已完成")
            {
                param.Add("Value", 1);
            }

            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(bar.Target, param);
        }

        private ObservableCollection<TaskBar> taskBars;

        public ObservableCollection<TaskBar> TaskBars
        {
            get { return taskBars; }
            set { taskBars = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<MemoDto> memoDtos;

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<ToDoDto> todoDtos;

        public ObservableCollection<ToDoDto> TodoDtos
        {
            get { return todoDtos; }
            set { todoDtos = value; RaisePropertyChanged(); }
        }
        public DelegateCommand<string> ExcuteCommand { get; private set; }
        public DelegateCommand<ToDoDto> EditToDoCommand { get; private set; }
        public DelegateCommand<MemoDto> EditMemoCommand { get; private set; }
        public DelegateCommand<ToDoDto> TodoCompleteCommand { get; private set; }
        public DelegateCommand<TaskBar> NavigateCommand { get; private set; }
        void CreateInitData()
        {

            MemoDtos = new ObservableCollection<MemoDto>();
            TodoDtos = new ObservableCollection<ToDoDto>();
            for (int i = 0; i < 10; i++)
            {
                MemoDtos.Add(new MemoDto() { Title = "备忘记录：" + i, Content = "待办信息。。。。" });
                TodoDtos.Add(new ToDoDto() { Id = i + 1, Title = "待办:" + i, Content = "事件", Status = 0 });
            }

            taskBars.Add(new TaskBar() { Icon = "ClockFast", Title = "汇总", Color = "#FF1ECA3A", Target = "ToDoView" });
            taskBars.Add(new TaskBar() { Icon = "ClockCheckOutline", Title = "已完成", Color = "#FF0CA0FF", Target = "ToDoView" });
            taskBars.Add(new TaskBar() { Icon = "ChartLineVariant", Title = "完成率", Color = "#FF02C6DC", Target = "" });
            taskBars.Add(new TaskBar() { Icon = "PlaylistStar", Title = "备忘录", Color = "#FFFFA000", Target = "MemoView" });

            Refresh();
        }

        private void Excute(string obj)
        {
            switch (obj)
            {
                case "AddToDo": AddToDo(null); break;
                case "AddMemo": AddMemo(null); break;
            }
        }
        async void AddToDo(ToDoDto model)
        {
            DialogParameters param = new DialogParameters();
            if (model != null)
            {
                param.Add("Value", model);
            }
            var dialogResult = await dialog.ShowDialog("AddToDoView", param);
            if (dialogResult.Result == ButtonResult.OK)
            {
                var toDo = dialogResult.Parameters.GetValue<ToDoDto>("Value");
                if (toDo.Id > 0)
                {
                    var temp = memoDtos.FirstOrDefault(p => p.Id == toDo.Id);
                    temp.Status = toDo.Status;
                    temp.Title = toDo.Title;
                    temp.Content = toDo.Content;
                }
                else
                {
                    TodoDtos.Add(toDo);
                }
                Refresh();
            }
        }
        async void AddMemo(MemoDto model)
        {
            DialogParameters param = new DialogParameters();
            if (model != null)
            {
                param.Add("Value", model);
            }
            var dialogResult = await dialog.ShowDialog("AddMemoView", param);
            if (dialogResult.Result == ButtonResult.OK)
            {
                var memo = dialogResult.Parameters.GetValue<MemoDto>("Value");
                if (memo.Id > 0)
                {
                    var temp = memoDtos.FirstOrDefault(p => p.Id == memo.Id);
                    temp.Status = memo.Status;
                    temp.Title = memo.Title;
                    temp.Content = memo.Content;
                }
                else
                {
                    MemoDtos.Add(memo);
                }
            }
            Refresh();
        }
        private void TodoComplete(ToDoDto dto)
        {
            var temp = TodoDtos.FirstOrDefault(p => p.Id == dto.Id);
            todoDtos.Remove(temp);
            Refresh();
        }
        void Refresh()
        {
            TaskBars[0].Content = TodoDtos.Count.ToString();
            TaskBars[1].Content = TodoDtos.Where(p => p.Status == 1).Count().ToString();
            TaskBars[2].Content = "100%";
            TaskBars[3].Content = MemoDtos.Count().ToString();
        }
    }
}
