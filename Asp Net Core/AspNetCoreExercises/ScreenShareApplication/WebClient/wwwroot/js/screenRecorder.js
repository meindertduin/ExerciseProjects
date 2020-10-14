window.screenCapture = {
    async startCapture(displayMediaOption){
        let captureStream = null;
        console.log(displayMediaOption);
        try {
            captureStream = await navigator.mediaDevices.getUserMedia(displayMediaOption);
        }catch (err) {
            console.log(err);
        }
        return captureStream;
    },
    async getConnectedDevices(type, callback){
        navigator.mediaDevices.enumerateDevices()
            .then(devices => {
                const filtered = devices.filter(devices => devices.kind === type);
            }).catch(err => console.log(err));
    },
    async displayConnectedDevices(){
        await this.getConnectedDevices('videoInput', cameras => console.log(cameras));
    } 
}