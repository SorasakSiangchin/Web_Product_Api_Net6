namespace Web_Product.interfaces
{
    public interface IUploadFileService
    {
        //ตรวจสอบมีการอัพโหลดไฟล์เข้ามาหรือไม่
        // IFormFileCollection เป็น interfaces ที่ใช้สำหรับ UploadFile
        bool IsUpload(IFormFileCollection formFiles);
        //ตรวจสอบนามสกุลไฟล์หรือรูปแบบที่่ต้องการ
        string Validation(IFormFileCollection formFiles);
        //อัพโหลดและส่งรายชื่อไฟล์ออกมา
        Task<List<string>> UploadImages(IFormFileCollection formFiles);
        Task DeleteImage(string filename);  
    }
}
