import { Coupon } from "../models/coupon.model";

export function downloadCoupon(coupon: Coupon) {
    let couponContent = convertBase64ToBlob(coupon.base64Content);

    const a = document.createElement('a');
    const objectUrl = URL.createObjectURL(couponContent);
    a.href = objectUrl;
    a.download = coupon.fileName;
    a.click();
    URL.revokeObjectURL(objectUrl);
}

function convertBase64ToBlob(base64Coupon: string): Blob {
    const byteCharacters = window.atob(base64Coupon);

    const byteNumbers = new Array(byteCharacters.length);

    for (let i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
    }

    return new Blob([new Uint8Array(byteNumbers)]);
}