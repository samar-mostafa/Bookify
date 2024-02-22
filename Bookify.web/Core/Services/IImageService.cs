namespace Bookify.web.Core.Services
{
	public interface IImageService
	{
		public Task<(bool isUploaded, string? errorMessage)> UploadAsync
			(IFormFile image, string imageName, string folderPath, bool hasThumbnail);
		
	}
}
