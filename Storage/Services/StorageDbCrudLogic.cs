using Storage.Data;
using Storage.Data.StorageDb;
using Storage.Models;
using System.ComponentModel.DataAnnotations;


namespace Storage.Services
{
    internal class StorageDbCrudLogic
    {
        //private readonly StorageDbContext storageDbContext ;
        private readonly ApplicationDbContext ApplicationDbContext;
		private ILogger<StorageDbCrudLogic> Logger { get; }
        public StorageDbCrudLogic(
			//StorageDbContext _storageDbContext,
			ApplicationDbContext _ApplicationDbContext,
            ILogger<StorageDbCrudLogic> _logger
            ) 
        {
			//storageDbContext = _storageDbContext;
			ApplicationDbContext = _ApplicationDbContext;
			Logger = _logger;
        }
        /// <summary>
        /// добавляет в базу StorageDb записи сформированные по объекту StorageItemModel
        /// </summary>
        /// <param name="storageItemModel"></param>
        /// <returns></returns>
        public async Task AddStorageItem(StorageItemModel storageItemModel)
        {
            Logger.LogInformation("Запуск AddStorageItem(storageItemeModel)" +
            " для добавления записи о новой вещи в базу данных");
            StorageItem storageItem = new();
            storageItem.Name = storageItemModel.StorageItemName;
            storageItem.ImageUrl = storageItemModel.StorageItemImageUrl;
            storageItem.Description = storageItemModel.StorageItemDescription;
            storageItem.InitDate = DateTime.Now;
            storageItem.User = storageItemModel.User;
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
            //await storageDbContext.StorageItems.AddAsync(storageItem);
            //storageDbContext.SaveChanges();
			await ApplicationDbContext.StorageItems.AddAsync(storageItem);
			ApplicationDbContext.SaveChanges();
		}
		/// <summary>
		/// Сохраняет в базу данных StorageDb изменения внесенные в объект StorageItem
		/// </summary>
		/// <param name="editedStorageItem"></param>
		/// <param name="Items"></param>
		/// <returns></returns>
        public async Task StorageItemsSaveChanges(StorageItem editedStorageItem, IQueryable<StorageItem>? Items)
        {
			Items.Where(storageItem => storageItem.StorageItemId == editedStorageItem.StorageItemId)
			.First()
			.Name = editedStorageItem.Name;
			Items.Where(storageItem => storageItem.StorageItemId == editedStorageItem.StorageItemId)
				.First()
				.ImageUrl = editedStorageItem.ImageUrl;
			Items.Where(storageItem => storageItem.StorageItemId == editedStorageItem.StorageItemId)
				.First()
				.Description = editedStorageItem.Description;
			Items.Where(storageItem => storageItem.StorageItemId == editedStorageItem.StorageItemId)
				.First()
				.Location.Name = editedStorageItem.Location.Name;
			Items.Where(storageItem => storageItem.StorageItemId == editedStorageItem.StorageItemId)
				.First()
				.Location.Description = editedStorageItem.Location.Description;
			if (editedStorageItem.Location.Room != null)
			{
				Items.Where(storageItem => storageItem.StorageItemId == editedStorageItem.StorageItemId)
					.First()
					.Location.Room = editedStorageItem.Location.Room;
			}
			else
			{
				Items.Where(storageItem => storageItem.StorageItemId == editedStorageItem.StorageItemId)
					.First()
					.Location.Room = null;
			}
			if (editedStorageItem.Location.StoragePlace != null)
			{
				Items.Where(storageItem => storageItem.StorageItemId == editedStorageItem.StorageItemId)
					.First()
					.Location.StoragePlace = editedStorageItem.Location.StoragePlace;
			}
			else
			{
				Items.Where(storageItem => storageItem.StorageItemId == editedStorageItem.StorageItemId)
					.First()
					.Location.StoragePlace = null;
			}
			if (editedStorageItem.Location.StoragePlaceCell != null)
			{
				Items.Where(storageItem => storageItem.StorageItemId == editedStorageItem.StorageItemId)
					.First()
					.Location.StoragePlaceCell = editedStorageItem.Location.StoragePlaceCell;
			}
			else
			{
				Items.Where(storageItem => storageItem.StorageItemId == editedStorageItem.StorageItemId)
					.First()
					.Location.StoragePlaceCell = null;
			}
			//storageDbContext.SaveChanges();
			ApplicationDbContext.SaveChanges();
		}
	}
}
