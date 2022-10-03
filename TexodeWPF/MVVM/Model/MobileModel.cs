using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using TexodeWPF.MVVM.ViewModel;

namespace TexodeWPF.MVVM.Model
{
    public class MobileModel : BaseViewModel, IDataErrorInfo
    {

        [JsonPropertyName("ID")]
        private Guid _id;
        public Guid ID
        {
            get { return _id; }
            set { 
                _id = value;
                OnPropertyChanged("ID");
            }
        }
        [JsonPropertyName("Name")]
        [Required]
        private string _name;
        public string Name
        {
            get { return _name; }
            set { 
                _name = value; 
                OnPropertyChanged("Name");
            }
        }
        [JsonPropertyName("Image")]
        [Required]
        private string _image;
        public string Image 
        { 
            get { return _image; }
            set { _image = value;
                OnPropertyChanged("Image");
            }
        }


        [JsonIgnore]
        public string this[string columnName]
        {
            get
            {
                string msg = null;
                switch (columnName)
                {
                    case "Name":
                        {
                            if (string.IsNullOrEmpty(this.Name) || string.IsNullOrWhiteSpace(this.Name))
                            {
                                msg = "Строка с именем не может быть пустой!";
                            }
                            break;
                        }
                }
                return msg;
            }
        }

        [JsonIgnore]
        public string Error
        {
            get { return null; }
        }
    }
}
