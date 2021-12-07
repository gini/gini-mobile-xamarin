using System;
using ObjCRuntime;

namespace GiniBank.iOS
{
	[Native]
	public enum GiniVisionImportFileTypesProxy : long
	{
		None = 0,
		Pdf = 1,
		Pdf_and_images = 2
	}
}
