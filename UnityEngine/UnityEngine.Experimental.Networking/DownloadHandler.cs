using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace UnityEngine.Experimental.Networking
{
	/// <summary>
	///   <para>Manage and process HTTP response body data received from a remote server.</para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public class DownloadHandler : IDisposable
	{
		[NonSerialized]
		internal IntPtr m_Ptr;

		/// <summary>
		///   <para>Returns true if this DownloadHandler has been informed by its parent UnityWebRequest that all data has been received, and this DownloadHandler has completed any necessary post-download processing. (Read Only)</para>
		/// </summary>
		public extern bool isDone
		{
			[WrapperlessIcall]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		/// <summary>
		///   <para>Returns the raw bytes downloaded from the remote server, or null. (Read Only)</para>
		/// </summary>
		public byte[] data
		{
			get
			{
				return this.GetData();
			}
		}

		/// <summary>
		///   <para>Convenience property. Returns the bytes from data interpreted as a UTF8 string. (Read Only)</para>
		/// </summary>
		public string text
		{
			get
			{
				return this.GetText();
			}
		}

		internal DownloadHandler()
		{
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void InternalCreateString();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void InternalCreateScript();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void InternalCreateTexture(bool readable);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void InternalCreateWebStream(string url, uint crc);

		internal void InternalCreateWebStream(string url, Hash128 hash, uint crc)
		{
			DownloadHandler.INTERNAL_CALL_InternalCreateWebStream(this, url, ref hash, crc);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_InternalCreateWebStream(DownloadHandler self, string url, ref Hash128 hash, uint crc);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InternalDestroy();

		~DownloadHandler()
		{
			this.InternalDestroy();
		}

		/// <summary>
		///   <para>Signals that this [DownloadHandler] is no longer being used, and should clean up any resources it is using.</para>
		/// </summary>
		public void Dispose()
		{
			this.InternalDestroy();
			GC.SuppressFinalize(this);
		}

		/// <summary>
		///   <para>Callback, invoked when the data property is accessed.</para>
		/// </summary>
		/// <returns>
		///   <para>Byte array to return as the value of the data property.</para>
		/// </returns>
		protected virtual byte[] GetData()
		{
			return null;
		}

		/// <summary>
		///   <para>Callback, invoked when the text property is accessed.</para>
		/// </summary>
		/// <returns>
		///   <para>String to return as the return value of the text property.</para>
		/// </returns>
		protected virtual string GetText()
		{
			byte[] data = this.GetData();
			if (data != null && data.Length > 0)
			{
				return Encoding.UTF8.GetString(data, 0, data.Length);
			}
			return string.Empty;
		}

		/// <summary>
		///   <para>Callback, invoked as data is received from the remote server.</para>
		/// </summary>
		/// <param name="data">A buffer containing unprocessed data, received from the remote server.</param>
		/// <param name="dataLength">The number of bytes in data which are new.</param>
		/// <returns>
		///   <para>True if the download should continue, false to abort.</para>
		/// </returns>
		protected virtual bool ReceiveData(byte[] data, int dataLength)
		{
			return true;
		}

		/// <summary>
		///   <para>Callback, invoked with a Content-Length header is received.</para>
		/// </summary>
		/// <param name="contentLength">The value of the received Content-Length header.</param>
		protected virtual void ReceiveContentLength(int contentLength)
		{
		}

		/// <summary>
		///   <para>Callback, invoked when all data has been received from the remote server.</para>
		/// </summary>
		protected virtual void CompleteContent()
		{
		}

		/// <summary>
		///   <para>Callback, invoked when UnityWebRequest.downloadProgress is accessed.</para>
		/// </summary>
		/// <returns>
		///   <para>The return value for UnityWebRequest.downloadProgress.</para>
		/// </returns>
		protected virtual float GetProgress()
		{
			return 0.5f;
		}

		protected static T GetCheckedDownloader<T>(UnityWebRequest www) where T : DownloadHandler
		{
			if (www == null)
			{
				throw new NullReferenceException("Cannot get content from a null UnityWebRequest object");
			}
			if (!www.isDone)
			{
				throw new InvalidOperationException("Cannot get content from an unfinished UnityWebRequest object");
			}
			if (www.isError)
			{
				throw new InvalidOperationException(www.error);
			}
			return (T)((object)www.downloadHandler);
		}
	}
}
