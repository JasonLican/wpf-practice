using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Common.ModelDto;

namespace TodoList.ViewModels
{
    public class MemoViewModel : BindableBase
    {
        public MemoViewModel()
        {
            AddCommand = new DelegateCommand(Add);
            MemoDtos = new ObservableCollection<MemoDto>();
            InitData();
        }
        public ObservableCollection<MemoDto> MemoDtos { get; private set; }
        private bool isRightDrawerOpen;

        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }

        public DelegateCommand AddCommand { get; set; }
        private void Add()
        {
            IsRightDrawerOpen = true;
        }

        void InitData()
        {
            for (int i = 0; i < 20; i++)
            {
                MemoDtos.Add(new MemoDto() { Content = "测试信息。。。", Title = "备忘录" + i });
            }
        }
    }
}
