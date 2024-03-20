namespace Storage.Models
{
    /// <summary>
    /// хранит в себе данные изображения при загрузке выбранного пользователем изображения
    /// </summary>
    internal class ImageLoaderHelper
    {
        /// <summary>
        /// статус изображения: загружается или загружено
        /// </summary>
        public string ImageStatus { get; set; } = "";

        /// <summary>
        /// формат строки:  $"data:{file.ContentType};base64,{base64Image}";
        /// </summary>
        public string ImageDataUrl { get; set; }
        public ImageLoaderHelper()
        {
            ImageDataUrl = "";
        }
    }
}
