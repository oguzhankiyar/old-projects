using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Biletall.Helper.Requests;
using TicketApp.Controls.Enums;

namespace TicketApp.Models
{
    class AssociationUriMapper : UriMapperBase
    {
        public override Uri MapUri(Uri uri)
        {
            string tempUri = System.Net.HttpUtility.UrlDecode(uri.ToString());
            if (tempUri.Contains("ticketapp:SearchTicket"))
            {
                string code = tempUri.Split('=')[2].Split('&')[0];
                string parameter = tempUri.Split('=')[3];
                return new Uri("/Pages/TicketAction/ListPage.xaml?code=" + code + "&parameter=" + parameter, UriKind.RelativeOrAbsolute);
            }
            return uri;
        }
    }
}
