using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using WebClient.Domain;

namespace WebClient.JsInterop
{
    public class ScreenShareInterop : IDisposable
    {
        private readonly IJSRuntime _jsRuntime;
        private DotNetObjectReference<StartRecordingHelper> objRef;

        public ScreenShareInterop(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public ValueTask<string> StartRecording()
        {
            objRef = DotNetObjectReference.Create(new StartRecordingHelper());
            return _jsRuntime.InvokeAsync<string>("screenCapture.startCapture", objRef);
        }
        
        
        public void Dispose()
        {
            objRef?.Dispose();
        }
    }
}