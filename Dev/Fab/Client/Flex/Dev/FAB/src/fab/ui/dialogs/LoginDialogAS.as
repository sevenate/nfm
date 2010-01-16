// ActionScript file
import flash.events.KeyboardEvent;
import mx.controls.Alert;
import mx.events.ValidationResultEvent;
import mx.rpc.events.FaultEvent;

private var vResult:ValidationResultEvent;

private function handleCreationComplete():void 
{
	this.addEventListener(KeyboardEvent.KEY_UP, handleKeyDown);
	//this.txtEmail.setFocus();
	//this.txtEmail.maxChars = 30;
	//this.txtPassword.maxChars = 20;
	//this.txtPassword.width = this.txtEmail.width;
}

private function handleKeyDown(event:KeyboardEvent):void
{
    if (event.charCode == Keyboard.ENTER)
	{
		//this.validateAndSubmit();
	}
}

public function validateAndSubmit():void 
{
	/*vResult = loginEmailValidator.validate();
    if (vResult.type==ValidationResultEvent.INVALID) 
        return;

    vResult = passwordValidator.validate();
    if (vResult.type==ValidationResultEvent.INVALID) 
        return;
   
	//var url: String = "http://geneva/timeserver/services/ETS?wsdl";
	var etsService: ETSService = new ETSService();
	
	var request: Login_request = new Login_request();
	request.username = this.txtEmail.text.toLowerCase();
	request.password = this.txtPassword.text;
	etsService.login_request_var = request;
		
	etsService.addloginEventListener(this.loginEventHandler);
	etsService.addETSServiceFaultEventListener(this.faultHandler);
	
	etsService.login_send();*/
	
}

/*private function loginEventHandler(e: LoginResultEvent):void
{
	EclipseTimeServer(this.parentApplication).iProjectMatrix.account = e.result as Account;
	EclipseTimeServer(this.parentApplication).switchStateTo(EclipseTimeServer.STATE_PROJECT_MATRIX); 
	//EclipseTimeServer(this.parentApplication).switchStateTo(EclipseTimeServer.STATE_TIME_REPORTS);
}
*/

private function faultHandler(e: FaultEvent):void
{
	Alert.show("Wrong email or password"); 
}

private function clearInputFields():void
{
	//this.txtEmail.text = "";
	//this.txtPassword.text = "";
}