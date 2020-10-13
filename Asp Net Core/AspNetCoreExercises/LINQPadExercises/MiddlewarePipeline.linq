<Query Kind="Program" />

void Main(){
	var pipe = new PipeBuilder(MainAction)
						.AddPipe(typeof(Wrap1))
						.AddPipe(typeof(Wrap2))
						.Build();
	
	pipe("main action msg");
}

public void MainAction(string msg){
	$"executing {msg}".Dump("main function");
}

public class PipeBuilder{
	Action<string> _mainAction;
	List<Type> _pipeTypes;
	public PipeBuilder(Action<string> mainAction){
		_mainAction = mainAction;
		_pipeTypes = new List<Type>();
	}
	
	public PipeBuilder AddPipe(Type pipeType){
		_pipeTypes.Add(pipeType);
		return this;
	}
	
	private Action<string> CreatePipe(int index){
		if(index < _pipeTypes.Count -1){
			var childPipeHandle = CreatePipe(index + 1);
			var pipe = (Pipe) Activator.CreateInstance(_pipeTypes[index], childPipeHandle);
			return pipe.Handle;
		}
		else{
			var finalPipe = (Pipe) Activator.CreateInstance(_pipeTypes[index], _mainAction);
			return finalPipe.Handle;
		}
	}
	
	public Action<string> Build(){
		return CreatePipe(0);
	}
}

public abstract class Pipe{
	protected Action<string> _action;
	public Pipe(Action<string> action){
		_action = action;
	}
	
	public abstract void Handle(string msg);
}

public class Wrap1 : Pipe{
	public Wrap1(Action<string> action) : base(action){}
	
	public override void Handle(string msg){
		"before".Dump("Wrap1");
		_action(msg);
		"after".Dump("Wrap1");
	}
}

public class Wrap2 : Pipe{
	public Wrap2(Action<string> action) : base(action){}
	
	public override void Handle(string msg){
		"before".Dump("Wrap2");
		_action(msg);
		"after".Dump("Wrap2");
	}
}