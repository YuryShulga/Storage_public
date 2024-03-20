using System.ComponentModel.DataAnnotations;

namespace Storage.Models
{
	/// <summary>
	/// модель данных для формы поиска
	/// </summary>
	public class FindModel
	{
		[Required(ErrorMessage = "Введите текст для поиска")]
		public string Text { get; set; }

		public bool FindInDescriptionChecked { get; set; } = false;
	}
}
