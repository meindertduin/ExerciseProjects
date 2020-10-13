<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

SemaphoreSlim gate = new SemaphoreSlim(1);

async Task Main(){
	for(int i = 0; i < 10; i++){
		"Start".Dump(String.Concat("person: ", (i  + 1).ToString()));
		await gate.WaitAsync();
		"Do some work".Dump();
		gate.Release();
		"Finish".Dump();
	}
	
}