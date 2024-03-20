namespace Storage.Data.StorageDb
{
	/// <summary>
	/// определенное место в комнате(Room) или помещении где хранится предмет, 
	/// например: шкаф, полка, под диваном
	/// </summary>
	public class StoragePlace
    {
        public int StoragePlaceId { get; set; }
        public string Name { get; set;}
        public string? ImageUrl { get; set;}
    }
}