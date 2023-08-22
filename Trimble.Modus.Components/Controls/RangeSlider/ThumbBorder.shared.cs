namespace Trimble.Modus.Components
{
	sealed class ThumbBorder: Border
	{
		public ThumbBorder()
		{
			#region Required work-around to prevent linker from removing the platform-specific implementation
//#if __ANDROID__
//			if (System.DateTime.Now.Ticks < 0)
//				_ = new ThumbFrameRenderer(XCT.Context ?? throw new NullReferenceException());
//#endif
			#endregion
		}
	}
}
