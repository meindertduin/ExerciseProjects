window.screenCapture = {
    async startCapture(dotnetHelper){
        try {
            const videoElem = document.getElementById("video");
            const stream  = await navigator.mediaDevices.getDisplayMedia({
                video: { mediaSource: "screen" }
            });

            const image = new Image();
            
            const recorder = new MediaRecorder(stream);
            recorder.ondataavailable = event => {
                const url = URL.createObjectURL(event.data);
                dotnetHelper.invokeMethodAsync('GiveBlobUrl', url);
            };
            
            recorder.start(500);
            
        }catch (err) {
            console.log(err);
        }
    },
    revokeBlobUrl(url){
        URL.revokeObjectURL(url);
    }
}