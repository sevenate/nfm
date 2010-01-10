// ActionScript file
import flash.net.URLRequest;
import mx.controls.Alert;

private function linkBtnHandler():void
{
	var url:String = "http://www.google.com.ua/";
    var request: URLRequest = new URLRequest(url);
    
    try 
    {            
        navigateToURL(request);
    }
    catch (e: Error) 
    {
        // handle error here
       Alert.show("Cannot navigate to the url '"+url+"'", "Error"+e.name);
    }
}