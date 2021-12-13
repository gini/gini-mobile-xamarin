using ObjCRuntime;

namespace GiniBank.iOS
{
	[Native]
	public enum GiniCaptureDocumentTypeProxy : long
	{
		Pdf = 0,
		Image = 1,
		Qrcode = 2
	}

	[Native]
	public enum GiniCaptureImportFileTypesProxy : long
	{
		None = 0,
		Pdf = 1,
		Pdf_and_images = 2
	}
}
