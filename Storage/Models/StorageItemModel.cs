using Microsoft.CodeAnalysis;
using Storage.Data.StorageDb;
using System.ComponentModel.DataAnnotations;

namespace Storage.Models
{
    /// <summary>
    /// Модель данных для использования моделью в EditForm 
    /// </summary>
    internal class StorageItemModel
    {
        public User User { get; set; }
        [Required(ErrorMessage = "Необходимо ввести название предмета")]
        public string StorageItemName { get; set; }
        public string? StorageItemImageUrl { get; set; } = null;
        public string? StorageItemDescription { get; set; }
        [Required(ErrorMessage = "Необходимо ввести название помещения в котором будет храниться предмет")]
        public string? LocationName { get; set; } = null;
        public string? LocationDescription { get; set; }
        public string? LocationRoomName { get; set; } = null;
        public string? LocationRoomDescription { get; set; }
        public string? LocationRoomPlaceName { get; set; } = null;
        public string? LocationRoomPlaceImageUrl { get; set; } = null;
        public string? LocationRoomPlaceCellName { get; set; }
    }
}
