
	public static class StringExtensions
    {
		public static string MD5(this string s)
		{
			using (var provider = System.Security.Cryptography.MD5.Create())
			{
				var builder = new System.Text.StringBuilder();

				foreach (byte b in provider.ComputeHash(System.Text.Encoding.UTF8.GetBytes(s)))
					builder.Append(b.ToString("x2").ToLower());

				return builder.ToString();
			}
		}
	}
