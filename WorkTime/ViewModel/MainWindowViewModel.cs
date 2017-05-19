using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTime.UserContorller;

namespace WorkTime.ViewModel
{
    class MainWindowViewModel
    {
        public ContorllerItem[] Items { get; }


        public MainWindowViewModel()
        {
            Items = new[]
            {
                new ContorllerItem("Home", new Home(){ DataContext = new HomeViewModel() })
            };
            

        }

    }
}
