using PruebaTecnica.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Collections.Specialized;
namespace PruebaTecnica.DAL
{
    public class RootDTOViewModel : INotifyPropertyChanged
    {
        ObservableCollection<RootDTO> RootDTOList { get; set; } = new ObservableCollection<RootDTO>();
        public RootDTOViewModel()
        {
            
            UpdateRootDTOList();
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}