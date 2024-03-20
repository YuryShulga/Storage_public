using System.Text;

namespace Storage.Data.StorageDb
{
    /// <summary>
    /// Вещь, запись о местоположении которой хранится на сервере,
    /// например: отвертка, очки, гаечный ключ
    /// </summary>
    public class StorageItem
    {
        public int StorageItemId { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public DateTime InitDate { get; set; }
        //
        public User User { get; set; }
        public Location Location { get; set; }

		public override string ToString()
		{
            StringBuilder sb = new();
            sb.Append($"StorageItemId={StorageItemId}\n");
            sb.Append($"Name=\"{Name}\"\n");
            sb.Append($"ImageUrl=\"{ImageUrl}\"\n");
            sb.Append($"Description=\"{Description}\"\n");
            sb.Append($"InitDate=\"{InitDate}\"\n");
            sb.Append($"LocationId=\"{Location.LocationId}\"\n");
            sb.Append($"Location.Name=\"{Location.Name}\"\n");
            sb.Append($"Location.Description=\"{Location.Description}\"\n");
            sb.Append($"Location.Room?.RoomId=\"{Location.Room?.RoomId}\"\n");
            sb.Append($"Location.Room?.Name=\"{Location.Room?.Name}\"\n");
            sb.Append($"Location.Room?.Description=\"{Location.Room?.Description}\"\n");
            sb.Append($"Location.StoragePlace?.StoragePlaceId=\"{Location.StoragePlace?.StoragePlaceId}\"\n");
            sb.Append($"Location.StoragePlace?.Name=\"{Location.StoragePlace?.Name}\"\n");
            sb.Append($"Location.StoragePlace?.ImageUrl=\"{Location.StoragePlace?.ImageUrl}\"\n");
            sb.Append($"Location.StoragePlaceCell?.StoragePlaceCellId=\"{Location.StoragePlaceCell?.StoragePlaceCellId}\"\n");
            sb.Append($"Location.StoragePlaceCell?.Name=\"{Location.StoragePlaceCell?.Name}\"\n");
            return sb.ToString();
		}
	}
    
}
