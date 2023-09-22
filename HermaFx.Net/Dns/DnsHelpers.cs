#region License
/**********************************************************************
 * Copyright (c) 2010, j. montgomery                                  *
 * All rights reserved.                                               *
 *                                                                    *
 * Redistribution and use in source and binary forms, with or without *
 * modification, are permitted provided that the following conditions *
 * are met:                                                           *
 *                                                                    *
 * + Redistributions of source code must retain the above copyright   *
 *   notice, this list of conditions and the following disclaimer.    *
 *                                                                    *
 * + Redistributions in binary form must reproduce the above copyright*
 *   notice, this list of conditions and the following disclaimer     *
 *   in the documentation and/or other materials provided with the    *
 *   distribution.                                                    *
 *                                                                    *
 * + Neither the name of j. montgomery's employer nor the names of    *
 *   its contributors may be used to endorse or promote products      *
 *   derived from this software without specific prior written        *
 *   permission.                                                      *
 *                                                                    *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS*
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT  *
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS  *
 * FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE     *
 * COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,*
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES           *
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR *
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) *
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,*
 * STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)      *
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED*
 * OF THE POSSIBILITY OF SUCH DAMAGE.                                 *
 **********************************************************************/
#endregion

using System;
using System.Diagnostics;
using System.Text;

namespace HermaFx.Net.Dns
{
	internal static class DnsHelpers
	{
		private const long Epoch = 621355968000000000;

		internal static byte[] CanonicaliseDnsName(string name, bool lowerCase)
		{
			if (!name.EndsWith("."))
			{
				name += ".";
			}

			if (name == ".")
			{
				return new byte[1];
			}

			StringBuilder sb = new StringBuilder();

			sb.Append('\0');

			for (int i = 0, j = 0; i < name.Length; i++, j++)
			{
				if (lowerCase)
				{
					sb.Append(char.ToLower(name[i]));
				}
				else
				{
					sb.Append(name[i]);
				}

				if (name[i] == '.')
				{
					sb[i - j] = (char)(j & 0xff);
					j = -1;
				}
			}

			sb[sb.Length - 1] = '\0';

			return Encoding.ASCII.GetBytes(sb.ToString());
		}

		internal static String DumpArrayToString(byte[] bytes)
		{
			StringBuilder builder = new StringBuilder();

			builder.Append("[");

			foreach (byte b in bytes)
			{
				builder.Append(" ");
				builder.Append((sbyte)b);
				builder.Append(" ");
			}

			builder.Append("]");

			return builder.ToString();
		}

		/// <summary>
		/// Converts a instance of a <see cref="DateTime"/> class to a 48 bit format time since epoch.
		/// Epoch is defined as 1-Jan-70 UTC.
		/// </summary>
		/// <param name="dateTimeToConvert">The <see cref="DateTime"/> instance to convert to DNS format.</param>
		/// <param name="timeHigh">The upper 16 bits of time.</param>
		/// <param name="timeLow">The lower 32 bits of the time object.</param>
		internal static void ConvertToDnsTime(DateTime dateTimeToConvert, out int timeHigh, out long timeLow)
		{
			long secondsFromEpoch = (dateTimeToConvert.ToUniversalTime().Ticks - Epoch) / 10000000;
			timeHigh = (int)(secondsFromEpoch >> 32);
			timeLow = (secondsFromEpoch & 0xFFFFFFFFL);

			Trace.WriteLine(String.Format("Date: {0}", dateTimeToConvert));
			Trace.WriteLine(String.Format("secondsFromEpoch: {0}", secondsFromEpoch));
			Trace.WriteLine(String.Format("timeHigh: {0}", timeHigh));
			Trace.WriteLine(String.Format("timeLow: {0}", timeLow));
		}

		/// <summary>
		/// Convert from DNS 48 but time format to a <see cref="DateTime"/> instance.
		/// </summary>
		/// <param name="timeHigh">The upper 16 bits of time.</param>
		/// <param name="timeLow">The lower 32 bits of the time object.</param>
		/// <returns>The converted date time</returns>
		internal static DateTime ConvertFromDnsTime(long timeLow, long timeHigh)
		{
			long time = (timeHigh << 32) + timeLow;
			time = time * 10000000;
			time += Epoch;

			return new DateTime(time);
		}

		/// <summary>
		/// Convert from DNS 48 but time format to a <see cref="DateTime"/> instance.
		/// </summary>
		/// <param name="dnsTime">The upper 48 bits of time.</param>
		/// <returns>The converted date time</returns>
		internal static DateTime ConvertFromDnsTime(long dnsTime)
		{
			dnsTime = dnsTime * 10000000;
			dnsTime += Epoch;

			return new DateTime(dnsTime);
		}
	}
}
