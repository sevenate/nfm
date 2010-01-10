// ActionScript file
import flash.events.MouseEvent;
import mx.events.FlexEvent;


private function clickHandler(me: MouseEvent): void
{
	trace("-clickHandler: "+me.target.name);
	var name: String = me.target.name;
	
	switch(name)
	{
		case 'lbtnRevenue':
			this.currentState = name.substr(4) + "View";
			break;
		case 'lbtnExpense':
			this.currentState = name.substr(4) + "View";
			break;
		case 'lbtnBalance':
			this.currentState = name.substr(4) + "View";
			break;
	}
	
} 

private function creationCompleteHandler(event:FlexEvent):void
{
	trace("creationCompleteHandler "+event);
	lbtnRevenue.addEventListener (MouseEvent.CLICK, clickHandler);
	lbtnExpense.addEventListener (MouseEvent.CLICK, clickHandler);
	lbtnBalance.addEventListener (MouseEvent.CLICK, clickHandler);
	trace(this.currentState);
}
