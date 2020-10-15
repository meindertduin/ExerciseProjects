let recorder;

window.screenCapture = {
    async startCapture(dotnetHelper){
        try {
            const videoElem = document.getElementById("video");
            const stream  = await navigator.mediaDevices.getDisplayMedia({
                video: { mediaSource: "screen" }
            });

            const image = new Image();
            
            recorder = new MediaRecorder(stream);
            recorder.ondataavailable = async event => {
                const url = URL.createObjectURL(event.data);
                console.log(event.data.type);
                await dotnetHelper.invokeMethodAsync('HandleBlobUrl', url);
                URL.revokeObjectURL(url);
            };
            
            recorder.start(500);
            
        }catch (err) {
            console.log(err);
        }
    },
    async stopCapture(dotnetHelper){
        recorder.stop();
        await dotnetHelper.invokeMethodAsync('StopStream')
    },
    revokeBlobUrl(url){
        URL.revokeObjectURL(url);
    }
}