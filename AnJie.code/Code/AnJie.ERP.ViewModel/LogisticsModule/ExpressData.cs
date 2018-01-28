using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AnJie.ERP.ViewModel.LogisticsModule
{
	public class ExpressData
	{
		public IEnumerable<ExpressDataItem> ExpressDataItems
		{
			get;
			set;
		}

		public string Message
		{
			get;
			set;
		}

		public bool Success
		{
			get;
			set;
		}

		public ExpressData()
		{
            ExpressDataItems = new ExpressDataItem[0];
		}
	}
}