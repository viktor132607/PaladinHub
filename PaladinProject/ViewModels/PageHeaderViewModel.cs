namespace PaladinProject.ViewModels
{
	public class PageHeaderViewModel
	{
		public string? CoverImage { get; set; }
		public string Title { get; set; } = string.Empty;
		public List<NavButton> Buttons { get; set; } = new();
	}

	public class NavButton
	{
		public string Url { get; set; } = string.Empty;
		public string Text { get; set; } = string.Empty;
		public string Icon { get; set; } = string.Empty;
	}
}
