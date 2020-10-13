<Query Kind="Program">
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

HttpClient _httpClient = new HttpClient{
	Timeout = TimeSpan.FromSeconds(5),
};

static int RequestsAllowedAtOnce = 10;

SemaphoreSlim _gate = new SemaphoreSlim(RequestsAllowedAtOnce);

void Main(){
	Task.WaitAll(CreateCalls().ToArray());
}

public IEnumerable<Task> CreateCalls(){
	for(int i = 0; i < 1000; i++){
		yield return CallSemaphoreDocs();
	}
}

public async Task CallSemaphoreDocs(){
	try{
		await _gate.WaitAsync();
		var response = await _httpClient.GetAsync("https://docs.microsoft.com/en-us/dotnet/api/system.threading.semaphoreslim?view=netcore-3.1");
		_gate.Release();
		response.StatusCode.Dump();
	} catch(Exception e){
		e.Message.Dump();
	}
}