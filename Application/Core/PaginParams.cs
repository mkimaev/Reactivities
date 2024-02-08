namespace Application.Core
{
	public class PaginParams
	{
		private const int MaxPageSize = 50;
		private int _pageSize = 10;

        public int PageSize 
		{
			get => this._pageSize;
			set => this._pageSize = (value > MaxPageSize) ? MaxPageSize : value;
		}
		public int PageNumber { get; set; } = 1;
    }
}
