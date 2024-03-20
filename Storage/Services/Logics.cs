using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using Storage.Data.StorageDb;
using Storage.Models;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mono.TextTemplating;
using Microsoft.Extensions.Logging.TraceSource;
using Storage.Data;

namespace Storage.Services
{
    internal class Logics
    {
        private IWebHostEnvironment WebHostEnvironment { get; set; }
        private AuthenticationStateProvider AuthenticationStateProvider { get; }
        private ILogger<Logics> Logger { get; }
        private StorageDbCrudLogic StorageDbCrudLogic { get; }
        //private StorageDbContext StorageDbContext { get; }
        private ApplicationDbContext ApplicationDbContext { get; }

		public Logics(
            AuthenticationStateProvider _authenticationStateProvider,
            IWebHostEnvironment _WebHostEnvironment,
            StorageDbCrudLogic _StorageDbCrudLogic,
			//StorageDbContext _StorageDbContext,
			ApplicationDbContext _ApplicationDbContext,

			 ILogger<Logics> _logger
            )
        {
            AuthenticationStateProvider = _authenticationStateProvider;
            WebHostEnvironment = _WebHostEnvironment;
            StorageDbCrudLogic = _StorageDbCrudLogic;
            //StorageDbContext = _StorageDbContext;
            ApplicationDbContext = _ApplicationDbContext;

			Logger = _logger;
        }
        /// <summary>
        /// возвращает Id текущего пользователя
        /// </summary>
        public async Task<string> GetCurrentUserId()
        {
            System.Security.Claims.ClaimsPrincipal user;
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            user = authState.User;
            string? name = user.Identity?.Name;
            if ( name != null ) 
            {
                return name.ToUpper();
            }
            Logger.LogError("await AuthenticationStateProvider.GetAuthenticationStateAsync(); вернул null");
            //TODO: сделать обработку появления null
            return name.ToUpper(); 
        }
        /// <summary>
        /// пропорционально изменяет размер изображения 
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        public byte[] ScaleImage(byte[] imageBytes, int maxWidth, int maxHeight)
        {
            Image image = Image.Load(imageBytes);


            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            image.Mutate(x => x.Resize(newWidth, newHeight));

            using var ms = new MemoryStream();
            image.Save(ms, new JpegEncoder());
            return ms.ToArray();
            //использую библиотеку using SixLabors.ImageSharp;
            //using SixLabors.ImageSharp.Formats.Jpeg;
            //using SixLabors.ImageSharp.Processing;
        }
        /// <summary>
        /// конвертирует файл на который указывает IBrowserFile file в массив byte[]
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileSize"></param>
        /// <returns></returns>
        public async Task<byte[]?> ConvertIBrowserFileToByteArray(IBrowserFile file, int fileSize = 30)//30мб
        {
            if (file != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    var bufferSize = 1024 * 250; // Размер блока - 250 кБ
                    var buffer = new byte[bufferSize];
                    var totalBytesRead = 0;
                    var bytesRead = 0;
                    using (var fileStream = file.OpenReadStream(1024 * 1024 * fileSize))
                    {
                        while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await memoryStream.WriteAsync(buffer, 0, bytesRead);
                            totalBytesRead += bytesRead;
                            // Проверка на превышение лимита
                            if (totalBytesRead >= 1024 * 1024 * fileSize)
                            {
                                // Достигнут лимит
                                break;
                            }
                        }
                    }
                    return memoryStream.ToArray();
                }
            }
            return null;
        }

        /// <summary>
        /// Преобразовывает изображение лежащее в свойстве "src" тега <img>
        /// </summary>
        /// <param name="dataUrlString"></param>
        /// <returns></returns>
        public async Task<byte[]?> ConvertImageDataTagSrcToByteArray(string dataUrlString)
        {
            int commaIndex = dataUrlString.IndexOf(',') + 1;
            if (commaIndex == -1) { return null; }
            string dataImageString = dataUrlString.Substring(commaIndex);
            byte[] result = Convert.FromBase64String(dataImageString);
            return result;
        }
        /// <summary>
        /// загружает изображение из ImageLoaderHelper в папку под названием Id-юзера
        /// и меняет в модели StorageItemModel название картинки на путь к сохраненной картинке
        /// </summary>
        /// <param name="imageLoaderHelper"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<string?> UploadImageToWwwroot(ImageLoaderHelper imageLoaderHelper, string? filename)
        {
            if (imageLoaderHelper.ImageDataUrl == "" || imageLoaderHelper.ImageDataUrl == null) { return null; }
            //получаю название папки (ее название это Id юзера)
            string imagesFolderName = await GetCurrentUserId();
            string path = $"\\Images\\UserImages\\{imagesFolderName}";
			//нахожу полный путь для CreateDirectory(path)
            string createDirectoryPath = GetAbsoluteUserImageFolderPath();
			//проверяю есть ли папка с именем userId, если нет, создаю ее
			Directory.CreateDirectory(createDirectoryPath);
            //добавляю к path имя файла
            string fullPath = Path.Combine(createDirectoryPath, filename);
            //получаю изображение вв виде массива байтов
            byte[]? imageBytes =await ConvertImageDataTagSrcToByteArray(imageLoaderHelper.ImageDataUrl);
            //создаю файл изображения
            Random rand = new();
            while (true)
            {
                try
                {
                    // Пытаюсь создать файл с именем fullPath
                    using (FileStream fs = File.Open(fullPath, FileMode.CreateNew))
                    {
                        break;
                    }
                }
                catch (IOException)//TODO обработать ошибку слишком длинного названия файла
                {
                    //меняю название файла
                    filename = rand.Next(10).ToString() + filename;
                    fullPath = Path.Combine(createDirectoryPath, filename);
                    continue;
                }
            }
            //сохраняю изображение в файл
            await SaveImageToFile(imageBytes, fullPath);
            return Path.Combine(path, filename);

        }
        /// <summary>
        /// сохраняет изображение представленное массивом byte по указанному адресу
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task SaveImageToFile(byte[] imageBytes, string path)
        {
            Image image = Image.Load(imageBytes);
            await image.SaveAsJpegAsync(path);
        }
        /// <summary>
        /// из относительного статического пути (/wwwroot/images/img.jpg) 
        /// делает полный путь (c:/.../wwwroot/images/img.jpg)
        /// </summary>
        /// <param name="path">относительный статического пути (/wwwroot/images/img.jpg)</param>
        /// <param name="AuthenticationStateProvider"></param>
        /// <returns></returns>
        public string WwwPathToLocalPath(string path)
        {
            string filename = Path.GetFileName(path);
            string wwwrootImagesFolder = GetCurrentUserId().Result;
            filename = Path.Combine(WebHostEnvironment.WebRootPath,
                $"Images\\UserImages\\{wwwrootImagesFolder}",
                filename);
            return filename;
        }

		public string GetAbsoluteUserImageFolderPath()
		{
            string path;
			string wwwrootImagesFolder = GetCurrentUserId().Result;
			path = Path.Combine(WebHostEnvironment.WebRootPath,
				$"Images\\UserImages\\{wwwrootImagesFolder}");
			return path;
		}
        /// <summary>
        /// возвращает все StorageItems переданного юзера
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
		public async Task<IQueryable<StorageItem>?> GetAllUserStorageItems()
		{
			string userId = await GetCurrentUserId();
            //var items = StorageDbContext.StorageItems
            var items = ApplicationDbContext.StorageItems
            .Include(item => item.Location)
            .ThenInclude(location => location.Room)
            .Include(item => item.Location)
            .ThenInclude(Location => Location.StoragePlace)
            .Include(item => item.Location)
            .ThenInclude(Location => Location.StoragePlaceCell)
            .Where(item => item.User.UserId == userId)
            .Include(item => item.User)
            .Where(item => item.User.UserId == userId);
			return items;
		}
        /// <summary>
        /// Возвращает список объектов StorageItem в названии которых есть text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
		public async Task<IQueryable<StorageItem>?> FindStorageItemsWithItemName(string text)
		{
            string userId = await GetCurrentUserId();
			//var items = StorageDbContext.StorageItems
			var items = ApplicationDbContext.StorageItems
			.Include(item => item.Location)
			.ThenInclude(location => location.Room)
			.Include(item => item.Location)
			.ThenInclude(Location => Location.StoragePlace)
			.Include(item => item.Location)
			.ThenInclude(Location => Location.StoragePlaceCell)
			.Where(item => 
                item.User.UserId == userId && 
                item.Name.ToLower().Contains(text.ToLower()));
			return items;
		}
        /// <summary>
        /// Возвращает список объектов StorageItem в которых есть text
        /// поиск ведется по всем полям объекта
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public async Task<IQueryable<StorageItem>?> FindStorageItemsWithAllItemText(string text)
		{
            string searchedText = text.ToLower();
			string userId = await GetCurrentUserId();
			//var items = StorageDbContext.StorageItems
			var items = ApplicationDbContext.StorageItems
			.Include(item => item.Location)
			.ThenInclude(location => location.Room)
			.Include(item => item.Location)
			.ThenInclude(Location => Location.StoragePlace)
			.Include(item => item.Location)
			.ThenInclude(Location => Location.StoragePlaceCell)
			.Where(item =>
			    item.User.UserId == userId &&
			    item.Name.ToLower().Contains(text.ToLower())
                ||
			    item.User.UserId == userId &&
			    item.Description.ToLower().Contains(text.ToLower())
			    ||
			    item.User.UserId == userId &&
			    item.Location.Name.ToLower().Contains(text.ToLower())
			    ||
			    item.User.UserId == userId &&
			    item.Location.Description.ToLower().Contains(text.ToLower())
			    ||
			    item.User.UserId == userId &&
			    item.Location.Room.Name.ToLower().Contains(text.ToLower())
			    ||
			    item.User.UserId == userId &&
			    item.Location.Room.Description.ToLower().Contains(text.ToLower())
                ||
			    item.User.UserId == userId &&
			    item.Location.StoragePlace.Name.ToLower().Contains(text.ToLower())
				||
				item.User.UserId == userId &&
				item.Location.StoragePlaceCell.Name.ToLower().Contains(text.ToLower())
			);
			return items;
		}
        public StorageItem ConvertStorageItemModelToStorageItem(StorageItemModel storageItemModel)
        {
			StorageItem storageItem = new();
			storageItem.Name = storageItemModel.StorageItemName;
			storageItem.ImageUrl = storageItemModel.StorageItemImageUrl;
			storageItem.Description = storageItemModel.StorageItemDescription;
			storageItem.InitDate = DateTime.Now;
            storageItem.User = new();
            storageItem.User.Name = storageItemModel.User.Name;
            storageItem.User.UserId = storageItemModel.User.UserId;
			storageItem.Location = new();
			storageItem.Location.Name = storageItemModel.LocationName;
			storageItem.Location.Description = storageItemModel.LocationDescription;
			if (storageItemModel.LocationRoomName != null)
			{
				storageItem.Location.Room = new();
				storageItem.Location.Room.Name = storageItemModel.LocationRoomName;
				storageItem.Location.Description = storageItemModel.LocationRoomDescription;
				if (storageItemModel.LocationRoomPlaceImageUrl != null)
				{
					if (storageItemModel.LocationRoomPlaceName == null)
					{ storageItemModel.LocationRoomPlaceName = ""; }
				}
				if (storageItemModel.LocationRoomPlaceName != null)
				{
					storageItem.Location.StoragePlace = new();
					storageItem.Location.StoragePlace.Name = storageItemModel.LocationRoomPlaceName;
					storageItem.Location.StoragePlace.ImageUrl = storageItemModel.LocationRoomPlaceImageUrl;
					if (storageItemModel.LocationRoomPlaceCellName != null)
					{
						storageItem.Location.StoragePlaceCell = new();
						storageItem.Location.StoragePlaceCell.Name = storageItemModel.LocationRoomPlaceCellName;
					}
				}
			}
			return storageItem;
        }
        /// <summary>
        /// Удаляет ненужные файлы изображений, которые могли остаться на сервере 
        /// при изменении уже существующего StorageItem
        /// </summary>
        /// <param name="tempStorageItemImageUrl"></param>
        /// <param name="tempStorageItemPlaceImageUrl"></param>
        /// <param name="tempItem"></param>
        /// <returns></returns>
        public async Task DeleteUnnecessaryImageFiles(
            string tempStorageItemImageUrl,
            string tempStorageItemPlaceImageUrl,
            StorageItem tempItem)
        {
            Logger.LogInformation("Запуск метода удаления ненужных файлов после изменения StorageItem: public async Task DeleteUnnecessaryImageFiles");
            if (tempStorageItemImageUrl != null && tempStorageItemImageUrl != tempItem.ImageUrl)
            {
                string path = WwwPathToLocalPath(tempStorageItemImageUrl);
                File.Delete(path);
            }
            if (tempStorageItemPlaceImageUrl != null && 
                tempStorageItemPlaceImageUrl != tempItem.Location.StoragePlace.ImageUrl)
            {
                string path = WwwPathToLocalPath(tempStorageItemPlaceImageUrl);
                 File.Delete(path);
            }
        }
        /// <summary>
        /// удаляет объект StorageItem и все объекты которые в него входят из базы данных
        /// </summary>
        /// <param name="storageItemId"></param>
        /// <param name="Items"></param>
        /// <returns></returns>
        public async Task DeleteStorageItem(int storageItemId, IQueryable<StorageItem>? Items)
        {
            StorageItem storageItem = Items.Where(storageItem => storageItem.StorageItemId == storageItemId).First();
            Logger.LogInformation("Запуск метода удаления объекта StorageItem: public async Task DeleteStorageItem(int storageItemId, IQueryable<StorageItem>? Items)");
			//удаляю изображения
			string storageItemUrl = Items.Where(storageItem => storageItem.StorageItemId == storageItemId).First().ImageUrl;
			//string storagePlaceImageUrl = StorageDbContext.StoragePlaces.Where(storagePlace =>
			string storagePlaceImageUrl = ApplicationDbContext.StoragePlaces.Where(storagePlace =>
				storagePlace.StoragePlaceId == storageItem.Location.StoragePlace.StoragePlaceId).First().ImageUrl;
            if (storageItemUrl != null) 
            { 
                storageItemUrl = WwwPathToLocalPath(storageItemUrl);
				File.Delete(storageItemUrl);
			}
			if (storagePlaceImageUrl != null)
            {
				storagePlaceImageUrl = WwwPathToLocalPath(storagePlaceImageUrl);
				File.Delete(storagePlaceImageUrl);
			}
            //удаляю StorageItem
            Items.Where(storageItem => storageItem.StorageItemId == storageItemId).ExecuteDelete();
            //удаляю Location
            //StorageDbContext.Locations.Where(location =>
            ApplicationDbContext.Locations.Where(location =>
                location.LocationId == storageItem.Location.LocationId).ExecuteDelete();
            //удаляю Room
            if (storageItem.Location.Room != null)
            {
                //StorageDbContext.Rooms.Where(room =>
                ApplicationDbContext.Rooms.Where(room =>
                room.RoomId == storageItem.Location.Room.RoomId).ExecuteDelete();
                if (storageItem.Location.StoragePlace != null)
                {
                    //StorageDbContext.StoragePlaces.Where(storagePlace =>
                    ApplicationDbContext.StoragePlaces.Where(storagePlace =>
                    storagePlace.StoragePlaceId ==
                                 storageItem.Location.StoragePlace.StoragePlaceId).ExecuteDelete();
                    if (storageItem.Location.StoragePlaceCell != null)
                    {
                        //StorageDbContext.StoragePlacesCells.Where(storagePlaceCell =>
                        ApplicationDbContext.StoragePlacesCells.Where(storagePlaceCell =>
                            storagePlaceCell.StoragePlaceCellId ==
                                     storageItem.Location.StoragePlaceCell.StoragePlaceCellId).ExecuteDelete();
                    }
                }
            }
        }



	}
}
