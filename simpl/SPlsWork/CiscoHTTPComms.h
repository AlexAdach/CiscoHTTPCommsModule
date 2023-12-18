namespace CiscoHTTPComms;
        // class declarations
         class Module;
     class Module 
    {
        // class delegates
        delegate FUNCTION HTTPButtonPressReceivedDelegate ( SIMPLSHARPSTRING RoomKitMac , SIMPLSHARPSTRING WidgetID );

        // class events

        // class functions
        INTEGER_FUNCTION Initialize ( INTEGER port );
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();
        STRING_FUNCTION ToString ();

        // class variables
        INTEGER __class_id__;

        // class properties
        INTEGER DebugEnable;
        DelegateProperty HTTPButtonPressReceivedDelegate HTTPButtonPressReceivedHandler;
    };

namespace CiscoHTTPComms.HTTPObjects;
        // class declarations
         class HTTPButtonPress;
     class HTTPButtonPress 
    {
        // class delegates

        // class events

        // class functions
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();
        STRING_FUNCTION ToString ();

        // class variables
        INTEGER __class_id__;

        // class properties
        STRING RoomkitMac[];
        STRING WidgetID[];
    };

