using System;
using System.Runtime.InteropServices;

namespace Dm
{
	internal class DmClientLexCall
	{
		[DllImport("dmclientlex.dll")]
		public static extern int clex_for_provider_init(out IntPtr clexProvider);

		[DllImport("dmclientlex.dll")]
		public static extern void clex_for_provider_deinit(IntPtr handle);

		[DllImport("dmclientlex.dll")]
		public static extern int clex_for_provider(IntPtr handle, string sql_txt);

		[DllImport("dmclientlex.dll")]
		public static extern int clex_for_provider_get_next_node(IntPtr handle, out string data, out IntPtr flag);

		[DllImport("dmclientlex.dll")]
		public static extern int clex_for_provider_get_first_node(IntPtr handle, out string data, out IntPtr flag);

		[DllImport("dmclientlex.dll")]
		public static extern int version_to_number_for_provider(string version);
	}
}
