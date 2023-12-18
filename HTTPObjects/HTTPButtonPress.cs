using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiscoHTTPComms.HTTPObjects
{

    public class HTTPButtonPress
    {
        [JsonProperty("mac")]
        public string RoomkitMac { get; set; }

        [JsonProperty("widgetId")]
        public string WidgetID { get; set; }


    }
}
