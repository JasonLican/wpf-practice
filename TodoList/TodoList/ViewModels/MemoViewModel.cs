using MaterialDesignColors;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using TodoList.Common.ModelDto;

namespace TodoList.ViewModels
{
    public class MemoViewModel : BindableBase
    {
        public MemoViewModel()
        {
            //AddCommand = new DelegateCommand(Add);
            MemoDtos = new ObservableCollection<MemoDto>();


            ExcuteCommand = new DelegateCommand<string>(Excute);
            MemoDtos = new ObservableCollection<MemoDto>();
            SelectedCommand = new DelegateCommand<MemoDto>(Selected);
            DelCommand = new DelegateCommand<MemoDto>(Del);

            InitData();
        }

        private void Excute(string obj)
        {
            switch (obj)
            {
                case "Add": Add(); break;
                case "Search": Query(); break;
                case "Save": Save(); break;
            }
        }

        private ObservableCollection<MemoDto> memoDtos;

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }

        private bool isRightDrawerOpen;

        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }

        private string search;

        public string Search
        {
            get { return search; }
            set { search = value; RaisePropertyChanged(); }
        }



        private MemoDto currentDto;

        public MemoDto CurrentDto
        {
            get { return currentDto; }
            set { currentDto = value; RaisePropertyChanged(); }
        }

        public DelegateCommand<string> ExcuteCommand { get; set; }
        public DelegateCommand<MemoDto> SelectedCommand { get; set; }
        public DelegateCommand<MemoDto> DelCommand { get; set; }
        private void Add()
        {
            CurrentDto = new MemoDto();
            IsRightDrawerOpen = true;
        }
        private void Del(MemoDto dto)
        {
            if (dto != null)
                memoDtos.Remove(dto);
        }

        private void Selected(MemoDto dto)
        {
            IsRightDrawerOpen = true;
            CurrentDto = MemoDtos.FirstOrDefault(p => p.Id == dto.Id);
        }
        private void Save()
        {
            if (string.IsNullOrEmpty(CurrentDto.Title) || string.IsNullOrEmpty(CurrentDto.Content))
                return;

            if (CurrentDto.Id > 0)
            {
                var temp = MemoDtos.FirstOrDefault(p => p.Id == CurrentDto.Id);
                if (temp != null)
                {
                    temp.Title = CurrentDto.Title;
                    temp.Content = CurrentDto.Content;
                    temp.Status = CurrentDto.Status;
                }
            }
            else
            {
                MemoDtos.Add(CurrentDto);
            }

            IsRightDrawerOpen = false;
        }

        private void Query()
        {
            if (string.IsNullOrEmpty(Search))
            {
                MemoDtos.Clear();
                InitData();
                return;
            }

            var temp = memoDtos.Where(p => p.Title.Contains(Search)).ToList();

            memoDtos.Clear();
            foreach (var item in temp)
            {
                memoDtos.Add(item);
            }
        }

        void InitData()
        {
            for (int i = 0; i < 20; i++)
            {
                MemoDtos.Add(new MemoDto() { Id = i + 1, Content = "测试信息。。。", Title = "备忘录" + i });
            }
        }
    }
}
