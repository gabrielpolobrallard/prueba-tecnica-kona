using PruebaTecnica.Commands;
using PruebaTecnica.DAL;
using PruebaTecnica.Models;
using PruebaTecnica.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace PruebaTecnica.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public RootDTOViewModel RootDTOVm;

        public MainViewModel()
        {
            DataAccess.CreateIfNotExists();
            RootDTOVm = new RootDTOViewModel();
            LoadUserFromRest();
        }


        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new CommandHandler(() => Save(), true));
            }
        }

        public void Save()
        {
            try
            {
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
            catch (Exception ex)
            {
                //TODO: LOG
            }
        }

        private async void LoadUserFromRest()
        {
            RootObject = await UserService.GetResults();
        }

        private Root _rootObject { get; set; }
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

        protected void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }

}
