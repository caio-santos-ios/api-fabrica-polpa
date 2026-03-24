namespace PulpaAPI.src.Handlers
{
    public class UploadHandler(IWebHostEnvironment env)
    {
        public async Task<string> UploadAttachment(string parent, IFormFile attachment)
        {
            string webRoot = Path.Combine(env.ContentRootPath, "wwwroot");
            string uploadPath = Path.Combine(webRoot, "uploads", parent);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(attachment.FileName)}";
            string filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await attachment.CopyToAsync(stream);

            return Path.Combine("uploads", parent, fileName);
        }
    }
}
