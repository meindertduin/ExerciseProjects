let recorder;
let stream;

window.screenCapture = {
    async startCapture(dotnetHelper){
        try {
            const videoElem = document.getElementById("video");
            stream  = await navigator.mediaDevices.getDisplayMedia({
                video: { mediaSource: "screen" }
            });
            
            recorder = new MediaRecorder(stream);
            recorder.ondataavailable = async event => {
                const url = URL.createObjectURL(event.data);
                console.log(event.data.type);
                await dotnetHelper.invokeMethodAsync('HandleBlobUrl', url);
                this.revokeBlobUrl(url);
            };
            
            recorder.start(1000);
            
        }catch (err) {
            console.log(err);
        }
    },
    async stopCapture(dotnetHelper){
        if (recorder === null || stream === null) return;
        recorder.stop();
        const tracks = stream.getTracks();
        tracks.forEach(track => track.stop());
        await dotnetHelper.invokeMethodAsync('StopStream')
    },
    revokeBlobUrl(url){
        URL.revokeObjectURL(url);
    }
}