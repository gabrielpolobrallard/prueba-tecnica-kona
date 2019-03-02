using PruebaTecnica.Commands;
using PruebaTecnica.DAL;
using PruebaTecnica.Models;
using PruebaTecnica.Service;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PruebaTecnica.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ICommand _clearBDCommand;

        private ICommand _nextPageCommand;

        private int _page;

        private ICommand _saveCommand;

        private object lockObject = new object();


        public MainViewModel()
        {
            DataAccess.CreateIfNotExists();
            IsLoading = true;
            LoadUserFromRest();
            RootDTOList = new ObservableCollection<RootDTO>();
            UpdateRootDTOList();
            _page = 1;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ClearBDCommand
        {
            get
            {
                return _clearBDCommand ?? (_clearBDCommand = new CommandHandler(() => ClearBD(), true));
            }
        }

        public ICommand NextPageCommand
        {
            get
            {
                return _nextPageCommand ?? (_nextPageCommand = new CommandHandler(() => NextPage(), true));
            }
        }

        public ObservableCollection<RootDTO> RootDTOList { get; set; }
        public Root RootObject
        {
            get { return this._rootObject; }
            set
            {
                if (value != _rootObject)
                {
                    _rootObject = value;
                    NotifyPropertyChanged("RootObject");
                }
            }
        }

        private bool _isLoading { get; set; }
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                NotifyPropertyChanged("IsLoading");
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new CommandHandler(() => Save(), true));
            }
        }

        private Root _rootObject { get; set; }

        public void Save()
        {
            try
            {
                lock (lockObject)
                {
                    DataAccess.CreateIfNotExists();
                    foreach (var item in RootObject.Results)
                    {
                        RootDTO tmp = new RootDTO();
                        tmp.Name = item?.Name?.First + item?.Name?.Last;
                        tmp.Direccion = item?.Location?.Street;
                        tmp.Imagen = item?.Picture?.Medium.ToString();
                        DataAccess.SaveRootDTO(tmp);
                        tmp = null;
                    }
                }
                UpdateRootDTOList();
            }
            catch (Exception ex)
            {
                //TODO: LOG
            }
        }
        protected void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        private void ClearBD()
        {
            DataAccess.ClearDB();
            RootDTOList.Clear();
        }

        private async void LoadUserFromRest()
        {
            var results = await Task.Run(async () => await UserService.GetResults());
            RootObject = results;
            IsLoading = false;
        }

        private async void NextPage()
        {
            _page = _page + 1;
            var results = await Task.Run(async () => await UserService.GetResults(_page));
            RootObject = results;
        }
        private void UpdateRootDTOList()
        {
            RootDTOList.Clear();
            foreach (var item in DataAccess.GetRootCollection())
            {
                RootDTOList.Add(item);
            }
            NotifyPropertyChanged("RootDTOList");
        }
    }
}