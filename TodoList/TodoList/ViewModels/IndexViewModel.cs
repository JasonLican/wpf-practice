using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Common.ModelDto;
using TodoList.Common.Models;

namespace TodoList.ViewModels
{
    public class IndexViewModel : BindableBase
    {
        public IndexViewModel()
        {
            taskBars = new ObservableCollection<TaskBar>();
            CreateInitData();
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
        //void CreateTaskBars()
        //{

        //}
        void CreateInitData()
        {
            taskBars.Add(new TaskBar() { Icon = "ClockFast", Title = "汇总", Content = "9", Color = "#FF0CA0FF", Target = "ToDoView" });
            taskBars.Add(new TaskBar() { Icon = "ClockCheckOutline", Title = "已完成", Content = "90", Color = "#FF1ECA3A", Target = "ToDoView" });
            taskBars.Add(new TaskBar() { Icon = "ChartLineVariant", Title = "完成率", Content = "100%", Color = "#FF02C6DC", Target = "" });
            taskBars.Add(new TaskBar() { Icon = "PlaylistStar", Title = "备忘录", Content = "10", Color = "#FFFFA000", Target = "MemoView" });


            MemoDtos = new ObservableCollection<MemoDto>();
            TodoDtos = new ObservableCollection<ToDoDto>();
            for (int i = 0; i < 10; i++)
            {
                MemoDtos.Add(new MemoDto() { Title = "待办：" + i, Content = "待办信息。。。。" });
                TodoDtos.Add(new ToDoDto() { Title = "备忘记录:" + i, Content = "事件" });
            }
        }
    }
}
