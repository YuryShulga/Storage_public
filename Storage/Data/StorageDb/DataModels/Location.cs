namespace Storage.Data.StorageDb
{
	/// <summary>
	/// Описание места, в которое положили предмет, 
	/// например: дома, в гараже, либо отдали какомуто человеку
	/// </summary>
	public class Location
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        //
        public Room? Room { get; set; }
        public StoragePlace? StoragePlace { get; set; }
        public StoragePlaceCell? StoragePlaceCell { get; set; }
    }
}