// ActionScript file
import flash.events.MouseEvent;
import mx.events.FlexEvent;
public static var STATE_REVENUE_VIEW: String = "RevenueView";
public static var STATE_EXPENSE_VIEW: String = "ExpenseView";
public static var STATE_BALANCE_VIEW: String = "BalanceView";

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

public function switchStateTo(state: String):void
{
	this.currentState = state;
	
	switch(this.currentState)
	{
		case STATE_REVENUE_VIEW:
			//this.initProjects();
			break;
		case STATE_REVENUE_VIEW:
			//this.initReports();
			break;
		case STATE_BALANCE_VIEW:
			//this.initReports();
			break;
		//add here other states
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
