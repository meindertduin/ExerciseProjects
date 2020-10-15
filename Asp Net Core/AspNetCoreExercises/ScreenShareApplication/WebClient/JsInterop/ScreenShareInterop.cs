using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace WebClient.JsInterop
{
    public class ScreenShareInterop : IDisposable
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly ScreenShareHelper _screenShareHelper;
        private DotNetObjectReference<ScreenShareHelper> objRef;

        public ScreenShareInterop(IJSRuntime jsRuntime, ScreenShareHelper screenShareHelper)
        {
            _jsRuntime = jsRuntime;
            _screenShareHelper = screenShareHelper;
        }

        public ValueTask<string> StartRecording()
        {
            objRef = DotNetObjectReference.Create(_screenShareHelper);
            return _jsRuntime.InvokeAsync<string>("screenCapture.startCapture", objRef);
        }

        public ValueTask<string> StopRecording()
        {
            objRef = DotNetObjectReference.Create(_screenShareHelper);
            return _jsRuntime.InvokeAsync<string>("screenCapture.stopCapture", objRef);
        }
        
        public void Dispose()
        {
            objRef?.Dispose();
        }
    }
}