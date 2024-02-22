using Bookify.web.Core.Models;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp.Memory;

namespace Bookify.web.Core.Services
{
	public class ImageService : IImageService
	{
		private readonly IWebHostEnvironment webHostEnvironment;
		private List<string> _allowedImageExtensions = new() { ".jpg", ".jpeg", ".png" };
		private int _allowedImageSize = 2097152;
		public ImageService(IWebHostEnvironment webHostEnvironment)
		{
			this.webHostEnvironment = webHostEnvironment;
		}
		public async Task<(bool isUploaded, string? errorMessage)> UploadAsync(IFormFile image, string imageName, string folderPath, bool hasThumbnail)
		{
			var extension = Path.GetExtension(image.FileName);
			if (!_allowedImageExtensions.Contains(extension))
				return (isUploaded: false, errorMessage: Errors.AllowedImageExtensions);
			if (_allowedImageSize < image.Length)
				return (isUploaded: false, errorMessage: Errors.AllowedImageSize);

			
			var path = Path.Combine($"{webHostEnvironment.WebRootPath}{folderPath}", imageName);
			var thumbPath = Path.Combine($"{webHostEnvironment.WebRootPath}{folderPath}/thumb", imageName);
			using var stream = System.IO.File.Create(path);
			await image.CopyToAsync(stream);
			stream.Dispose();
			if (hasThumbnail)
			{
				using var loadedImage = Image.Load(image.OpenReadStream());
				var ratio = (float)loadedImage.Width / 200;
				var height = loadedImage.Height / ratio;
				loadedImage.Mutate(i => i.Resize(width: 200, height: (int)height));
				loadedImage.Save(thumbPath);
			}

			return (isUploaded: true, errorMessage: null);


		}
	}
}
