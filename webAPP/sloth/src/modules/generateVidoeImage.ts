export default function drawVideoImage(text?: string): string {
    const videoImage = document.createElement('canvas');
    videoImage.width = 640;
    videoImage.height = 480;
    const ctx = videoImage.getContext('2d');
    ctx.fillStyle = '#FFF';
    ctx.fillRect(0, 0, videoImage.width, videoImage.height);
    ctx.fillStyle = '#000';
    ctx.font = '48px serif';
    ctx.fillText(text || 'Video', 10, 50);
    return videoImage.toDataURL();
}
