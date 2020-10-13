<Query Kind="Program">
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main(){
	var channel = new Channel<string>();
	Task.WaitAll(Consumer(channel), Producer(channel));	
}

public Task Producer(IWrite<string> writer){
	for(int i = 0; i < 100; i++){
		writer.Push(i.ToString());
		Task.Delay(100).GetAwaiter().GetResult();
	}
	
	writer.Complete();
	return Task.CompletedTask;
}

public async Task Consumer(IRead<string> reader){
	while(reader.IsComplete() == false){
		var msg = await reader.Read();
		msg.Dump("msg");
	}
}	

public interface IRead<T>{
	Task<T> Read();
	bool IsComplete();
}

public interface IWrite<T>{
	void Push(T msg);
	void Complete();
}

public class Channel<T> : IRead<T>, IWrite<T>{

	private ConcurrentQueue<T> queue = new ConcurrentQueue<T>();
	private SemaphoreSlim gate = new SemaphoreSlim(0);
	private bool finished = false;
	
	
	public void Push(T msg){
		queue.Enqueue(msg);
		gate.Release();
	}
	
	public async Task<T> Read(){
		await gate.WaitAsync();
		if(queue.TryDequeue(out var msg)){
			return msg;
		}
		
		return default;
	}

	public void Complete(){
		finished = true;
	}
	
	public bool IsComplete(){
		return finished && queue.IsEmpty;
	}
	
}