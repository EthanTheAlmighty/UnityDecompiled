using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Holds data for a single application crash event and provides access to all gathered crash reports.</para>
	/// </summary>
	public sealed class CrashReport
	{
		private static List<CrashReport> internalReports;

		private static object reportsLock = new object();

		private readonly string id;

		/// <summary>
		///   <para>Time, when the crash occured.</para>
		/// </summary>
		public readonly DateTime time;

		/// <summary>
		///   <para>Crash report data as formatted text.</para>
		/// </summary>
		public readonly string text;

		/// <summary>
		///   <para>Returns all currently available reports in a new array.</para>
		/// </summary>
		public static CrashReport[] reports
		{
			get
			{
				CrashReport.PopulateReports();
				object obj = CrashReport.reportsLock;
				CrashReport[] result;
				lock (obj)
				{
					result = CrashReport.internalReports.ToArray();
				}
				return result;
			}
		}

		/// <summary>
		///   <para>Returns last crash report, or null if no reports are available.</para>
		/// </summary>
		public static CrashReport lastReport
		{
			get
			{
				CrashReport.PopulateReports();
				object obj = CrashReport.reportsLock;
				lock (obj)
				{
					if (CrashReport.internalReports.Count > 0)
					{
						return CrashReport.internalReports[CrashReport.internalReports.Count - 1];
					}
				}
				return null;
			}
		}

		private CrashReport(string id, DateTime time, string text)
		{
			this.id = id;
			this.time = time;
			this.text = text;
		}

		private static int Compare(CrashReport c1, CrashReport c2)
		{
			long ticks = c1.time.Ticks;
			long ticks2 = c2.time.Ticks;
			if (ticks > ticks2)
			{
				return 1;
			}
			if (ticks < ticks2)
			{
				return -1;
			}
			return 0;
		}

		private static void PopulateReports()
		{
			object obj = CrashReport.reportsLock;
			lock (obj)
			{
				if (CrashReport.internalReports == null)
				{
					string[] reports = CrashReport.GetReports();
					CrashReport.internalReports = new List<CrashReport>(reports.Length);
					string[] array = reports;
					for (int i = 0; i < array.Length; i++)
					{
						string text = array[i];
						double value;
						string text2;
						CrashReport.GetReportData(text, out value, out text2);
						DateTime dateTime = new DateTime(1970, 1, 1);
						DateTime dateTime2 = dateTime.AddSeconds(value);
						CrashReport.internalReports.Add(new CrashReport(text, dateTime2, text2));
					}
					CrashReport.internalReports.Sort(new Comparison<CrashReport>(CrashReport.Compare));
				}
			}
		}

		/// <summary>
		///   <para>Remove all reports from available reports list.</para>
		/// </summary>
		public static void RemoveAll()
		{
			CrashReport[] reports = CrashReport.reports;
			for (int i = 0; i < reports.Length; i++)
			{
				CrashReport crashReport = reports[i];
				crashReport.Remove();
			}
		}

		/// <summary>
		///   <para>Remove report from available reports list.</para>
		/// </summary>
		public void Remove()
		{
			if (CrashReport.RemoveReport(this.id))
			{
				object obj = CrashReport.reportsLock;
				lock (obj)
				{
					CrashReport.internalReports.Remove(this);
				}
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string[] GetReports();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetReportData(string id, out double secondsSinceUnixEpoch, out string text);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool RemoveReport(string id);
	}
}
