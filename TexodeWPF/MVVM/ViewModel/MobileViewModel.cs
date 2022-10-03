using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TexodeWPF.MVVM.Model;
using TexodeWPF.MVVM.Commands;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace TexodeWPF.MVVM.ViewModel
{
    public class MobileViewModel : BaseViewModel
    {
        private HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("http://localhost:5000/api/")
        };

        private ObservableCollection<MobileModel> _mobiles;
        public ObservableCollection<MobileModel> Mobiles
        {
            get {
                if (_mobiles == null)
                {
                    _mobiles = new ObservableCollection<MobileModel>();
                }
                return _mobiles;
            }
            set
            {
                _mobiles = value;
                OnPropertyChanged("Mobiles");
            }
        }

        public MobileViewModel()
        {
            IsNewItemWindowVisible = Visibility.Collapsed;
        }

        private MobileModel _selectedItem;
        public MobileModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

       
        private MobileModel _newMobile;
        public MobileModel NewMobile
        {
            get {
                if (_newMobile == null)
                {
                    _newMobile = new MobileModel();
                }
                return _newMobile;
            }
            set
            {
                _newMobile = value;
                OnPropertyChanged("NewMobile");
            }
        }


        private RelayCommand _getMobilesCommand;
        public RelayCommand GetMobilesCommand
        {
            get
            {
                return _getMobilesCommand ?? (_getMobilesCommand = new RelayCommand(async obj =>
                {
                    HttpResponseMessage response = await client.GetAsync("mobile");
                    if (response.IsSuccessStatusCode)
                    {
                        using (StreamReader str = new StreamReader(await response.Content.ReadAsStreamAsync()))
                        {
                            Mobiles = await JsonSerializer.DeserializeAsync<ObservableCollection<MobileModel>>(str.BaseStream);
                        }
                    }
                    else
                    {
                        try
                        {
                            response.EnsureSuccessStatusCode();
                        }
                        catch (HttpRequestException exc)
                        {
                            MessageBox.Show(exc.Message);
                        }
                    }
                }));
            }
        }

        private Visibility _isNewItemWindowVisible;
        public Visibility IsNewItemWindowVisible
        {
            get { return _isNewItemWindowVisible; }
            set
            {
                _isNewItemWindowVisible = value;
                OnPropertyChanged("IsNewItemWindowVisible");
            }
        }


        private RelayCommand _showNewItemWindowCommand;
        public RelayCommand ShowNewItemWindowCommand
        {
            get
            {
                return _showNewItemWindowCommand ?? (_showNewItemWindowCommand = new RelayCommand(obj =>
                {
                    IsNewItemWindowVisible = IsNewItemWindowVisible == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                }));
            }

        }

        private RelayCommand _selectImageCommand;
        public RelayCommand SelectImageCommand
        {
            get
            {
                return _selectImageCommand ?? (_selectImageCommand = new RelayCommand(obj =>
                {
                    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                    dlg.Filter = "Images|*.jpeg; *.png;";
                    var result = dlg.ShowDialog();
                    if (result == true)
                    {
                        if (IsNewItemWindowVisible == Visibility.Visible)
                        {
                            NewMobile.Image = Convert.ToBase64String(File.ReadAllBytes(dlg.FileName));
                        }
                        else
                        {
                            SelectedItem.Image = Convert.ToBase64String(File.ReadAllBytes(dlg.FileName).ToArray());
                            UpdateMobileCommand.Execute(this);
                        }
                        
                    }
                }));
            }
        }

        private RelayCommand _postMobileCardCommand;
        public RelayCommand PostMobileCardCommand
        {
            get
            {
                return _postMobileCardCommand ?? (_postMobileCardCommand = new RelayCommand(async obj =>
                {
                    NewMobile.ID = Guid.NewGuid();
                    if (string.IsNullOrEmpty(NewMobile.Name) || string.IsNullOrWhiteSpace(NewMobile.Name))
                    {
                        MessageBox.Show("Пожалуйста, добавьте имя карты");
                        return;
                       
                    }else if(string.IsNullOrEmpty(NewMobile.Image))
                    {
                        MessageBox.Show("Пожалуйста, добавьте изображение");
                        return;
                    }
                    string _jsonContent = JsonSerializer.Serialize(NewMobile);
                    HttpContent _postContent = new StringContent(_jsonContent, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("mobile", _postContent);
                    if (response.IsSuccessStatusCode)
                    {
                        Mobiles.Add(NewMobile);
                        IsNewItemWindowVisible = Visibility.Collapsed;
                        NewMobile = null;
                        MessageBox.Show("Новая карточка добавлена!");
                    }
                    else
                    {
                        try
                        {
                            response.EnsureSuccessStatusCode();
                        }
                        catch (HttpRequestException exc)
                        {
                            MessageBox.Show(exc.Message);
                        }
                    }
                }));
            }
        }

        private RelayCommand _sortCardsByNameCommand;
        public RelayCommand SortCardsByNameCommand
        {
            get
            {
                return _sortCardsByNameCommand ?? (_sortCardsByNameCommand = new RelayCommand(obj =>
                {
                    Mobiles = new ObservableCollection<MobileModel>(Mobiles.OrderBy(i => i.Name));
                }));
            }
        }


        private RelayCommand _updateMobileCommand;
        public RelayCommand UpdateMobileCommand
        {
            get
            {
                return _updateMobileCommand ?? (_updateMobileCommand = new RelayCommand(async obj =>
                {
                    if(string.IsNullOrEmpty(SelectedItem.Name) || string.IsNullOrWhiteSpace(SelectedItem.Name))
                    {
                        MessageBox.Show("Поле имя не может быть пустым");
                        return;
                    }
                    string _jsonContent = JsonSerializer.Serialize(SelectedItem);
                    HttpContent _putContent = new StringContent(_jsonContent, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync("mobile", _putContent);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Данные карточки успешно изменены!");
                    }
                    else
                    {
                        try
                        {
                            response.EnsureSuccessStatusCode();
                        }
                        catch (HttpRequestException exc)
                        {
                            MessageBox.Show(exc.Message);
                        }
                    }
                }));
            }
        }

        private RelayCommand _removeMobileCommand;
        public RelayCommand RemoveMobileCommand
        {
            get
            {
                return _removeMobileCommand ?? (_removeMobileCommand = new RelayCommand(async obj =>
                {

                    if (obj != null && ((System.Collections.IList)obj).Count > 0)
                    {

                        List<MobileModel> itemsToDelete = new List<MobileModel>();
                        foreach (var item in (dynamic)obj)
                        {
                            itemsToDelete.Add(item);
                        }
                        HttpContent _removeContent = new StringContent(JsonSerializer.Serialize(itemsToDelete), Encoding.UTF8, "application/json");
                        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, client.BaseAddress + "mobile") 
                        { 
                            Content = _removeContent,
                        };
                        var response = await client.SendAsync(requestMessage);
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Карточка успешно удалена");
                            foreach (var item in itemsToDelete)
                            {
                                Mobiles.Remove(item);
                            }
                        }
                        else
                        {
                            try
                            {
                                response.EnsureSuccessStatusCode();
                            }
                            catch (HttpRequestException exc)
                            {
                                MessageBox.Show(exc.Message);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Выберите элемент для удаления");
                    }
                }));
            }
        }
    }
}
