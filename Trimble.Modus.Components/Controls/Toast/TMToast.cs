using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trimble.Modus.Components.Controls.Toast
{
    public class TMToast
    {


        public TMToast() {

         

        }
        public void Show (string message, ImageSource leftIconSource = null , string rightIconText = null)
        {
           MopupService.Instance.PushAsync(new TMToastContents( leftIconSource  , message, rightIconText));
        }
      
    }
}
