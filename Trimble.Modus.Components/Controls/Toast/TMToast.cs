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
        public void Show(ImageSource leftIconSource, string message, string rightIconText)
        {
           MopupService.Instance.PushAsync(new TMToastContents( leftIconSource  , message, rightIconText));
        }
        public void Show(ImageSource leftIconSource, string message, ImageSource rightIconSource)
        {
            MopupService.Instance.PushAsync(new TMToastContents(leftIconSource , message, rightIconSource));
        }
    }
}
