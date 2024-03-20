namespace Storage.Data.StorageDb
{
	/// <summary>
	/// описание отдельной секции места(StoragePlace) в которое положили хранимый предмет,
	/// например: верхняя полка шкафа
	/// </summary>
	public class StoragePlaceCell
    {
        public int StoragePlaceCellId { get; set; }
        public string Name { get; set; }
    }

}