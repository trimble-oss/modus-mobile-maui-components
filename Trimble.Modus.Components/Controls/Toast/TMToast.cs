
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trimble.Modus.Components.Popup.Services;

namespace Trimble.Modus.Components.Controls.Toast
{
    public class TMToast
    {

        PopupNavigation popupNavigation;
        public TMToast() {

            popupNavigation  = new PopupNavigation();
        }
        public void Show (string message, ImageSource leftIconSource = null , string rightIconText = null)
        {
           popupNavigation.PushAsync(new TMToastContents( leftIconSource  , message, rightIconText,popupNavigation),false);
        }
      
    }
}
