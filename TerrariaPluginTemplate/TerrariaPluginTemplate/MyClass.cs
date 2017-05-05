using System;
using System.Linq;


using Terraria;
using TerrariaApi.Server;


namespace TerrariaPluginTemplate
{

	[ApiVersion(2, 1)]
	public class MyClass : TerrariaPlugin
	{

		public override string Author  {
			get{ return "Zerif-Shinu"; }
		}

		public override string Description  {
			get{ return "Template for TShock Plugins for Terraria Server with Terraria Version 1.3.5.3"; }
		}

		public override string Name  {
			get { return "Terraria Plugin Template"; }
		}

		
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Methods to perform when the Plugin is disposed i.e. unhooks
			}
		}

		public MyClass(Main game) : base(game)
		{
			// Load priority. smaller numbers loads earlier
			Order = 1;
		}

		public override void Initialize()
		{
			// Methods to perform when plugin is initzialising i.e. hookings
		}
	}
}
