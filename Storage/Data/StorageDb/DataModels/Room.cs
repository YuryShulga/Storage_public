namespace Storage.Data.StorageDb
{
	/// <summary>
	/// название комнаты в помещении класса Location,
	/// например: Location = мой дом, room = спальня
	/// </summary>
	public class Room
    {
        public int RoomId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}