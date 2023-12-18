import xapi from 'xapi';
/*
This macro is used to communicate with a Crestron Processor running the CiscoHTTPComms Simpl+ module. 

Instructions:
  -Please update the constant variables below:
    -Button Definitions: Include the button IDs of every btn you want to trigger an HTTP request.
    -Processor IP, self explanatory.
    -Port Number

*/

const ProcessorIP = "192.168.0.159";
const Port = "8080";

const ButtonDefinitions = [
  "btn_lights_1", 
  "btn_lights_2",
  "btn_lights_3"
];


//Main
//************************************************************************************** */

/* This Hosname list must be cleared or else the command won't send. If you need to have items in this hostname list. You will need to Add the processor http url to the allowed hostnames */
xapi.Command.HttpClient.Allow.Hostname.Clear();
xapi.Config.HttpClient.Mode.set("On");
xapi.Config.HttpClient.AllowHTTP.set("True");

const ProcessorURL = "http://" + ProcessorIP + ":" + Port;
console.log(`Raw Request URL ${ProcessorURL}`);

FN_GUISubscribe() //Subscribe to GUI presses.

//HTTP
//************************************************************************************ */

async function FN_SendCommandToProcessor(widgetId)
{
      var mac = await xapi.Status.Network[1].Ethernet.MacAddress.get();
      console.log(mac);

      var commandBodyObject = {
        mac,
        widgetId,
      }

      var header = 'Content-Type: application/json';
      xapi.Command.HttpClient.Put({ AllowInsecureHTTPS: "True", Header: header, Url: ProcessorURL }, JSON.stringify(commandBodyObject))
      .then((response) => {if(response.StatusCode == 200){
                    FN_SetButtonFeedback(widgetId, "active")
      }})
      .catch((error) => FN_ShowErrorMessage(error))

}




//GUI
//************************************************************************************ */
function FN_ShowErrorMessage(error)
{
  console.log(error);
  FN_ClearInterlockedButtons();
  xapi.Command.UserInterface.Message.Alert.Display(
    { Duration: 5, Text: error.message, Title: "Error" });
}

function FN_SetButtonFeedback(widgetId, value)
{

  FN_ClearInterlockedButtons();

  xapi.Command.UserInterface.Extensions.Widget.SetValue(
    { Value: value, WidgetId: widgetId });
}

function FN_ClearInterlockedButtons()
{
  for(var i = 0; i < ButtonDefinitions.length; i++)
  {
        xapi.Command.UserInterface.Extensions.Widget.SetValue(
    { Value: "inactive", WidgetId: ButtonDefinitions[i] });
  }
}


function FN_GUISubscribe() {
  xapi.Event.UserInterface.Extensions.on((event) => {
    var guiEvent = JSON.stringify(event);
    //console.log(guiEvent);
    if(guiEvent.includes("Widget") && guiEvent.includes("clicked"))
    {
          if(ButtonDefinitions.includes(event.Widget.Action.WidgetId))
          {
                console.log(`Button Pressed ${event.Widget.Action.WidgetId}`);
                FN_SendCommandToProcessor(event.Widget.Action.WidgetId);
          }
    }
  })
}




