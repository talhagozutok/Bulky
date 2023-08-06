window.addEventListener("load", () => {
    const uri = document.getElementById("qrCodeData").getAttribute('data-url');
    var qrcode = new QRCode(document.getElementById("qrCode"),{
        text: uri,
        width: 150,
        height: 150,
        padding: 100,
        colorDark: "#000000",
        colorLight: "#ffffff",
        correctLevel: QRCode.CorrectLevel.H
    });
});